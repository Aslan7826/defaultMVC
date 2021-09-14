using DemoUse.WPFView.Models.Enums;
using DemoUse.WPFView.ViewModels;
using Microsoft.TeamFoundation.Common.Internal;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace DemoUse.WPFView.Models
{
    public class BootstrapperApplicationModel
    {

        public BootstrapperEventHandler DataHandler;
        private IntPtr hwnd;
        public bool isUnstalling = false;
        public Dispatcher Dispatcher { get; set; }
        private DiaViewModel Model { get; set; }

        private Timer timer;
        private BootstrapperEventArgs _BootstrapperEventArgs;

        public BootstrapperApplicationModel(BootstrapperApplication bootstrapperApplication)
        {
            hwnd = IntPtr.Zero;
            Model = new DiaViewModel(bootstrapperApplication);
            Dispatcher = Dispatcher.CurrentDispatcher;
            BootstrapperApplication = bootstrapperApplication;
            executingPackageOrderIndex = new Dictionary<string, int>();
            _BootstrapperEventArgs = new BootstrapperEventArgs();
            EventHandlers();
        }
        public BootstrapperApplication BootstrapperApplication
        {
            get;
            private set;
        }
        /// <summary>
        /// 我们用来存储引导程序结束后Burn引擎返回的状态码 
        /// </summary>
        public int FinalResult { get; set; }

        public void SetWindowHandle(Window view)
        {
            //重新發送狀態給沒監聽到的視窗
            State = State;
            hwnd = new WindowInteropHelper(view).Handle;
        }
        /// <summary>
        /// 任务准备，例如安装，卸载，修复或者更改
        /// </summary>
        /// <param name="action"></param>
        public void PlanAction(LaunchAction action)
        {
            BootstrapperApplication.Engine.Plan(action);
        }
        /// <summary>
        /// 执行这个任务
        /// </summary>
        public void ApplyAction()
        {
            BootstrapperApplication.Engine.Apply(hwnd);
        }

        public void LogMessage(string message)
        {
            BootstrapperApplication.Engine.Log(LogLevel.Standard, message);
        }

        public void SetBurnVariable(string variableName, string value)
        {
            BootstrapperApplication.Engine.StringVariables[variableName] = value;
        }
        /// <summary>
        /// 系統檢測
        /// </summary>
        private void EventHandlers()
        {
            //26 當檢測階段完成時觸發。
            BootstrapperApplication.DetectComplete += DetectComplete;
            //27 當對特定包的檢測完成時觸發。
            BootstrapperApplication.DetectPackageComplete += DetectPackageComplete;
            //25 當引擎開始計劃安裝時觸發
            BootstrapperApplication.PlanBegin += PlanBegin;
            //30 當引擎完成特定包的安裝規劃時觸發
            BootstrapperApplication.PlanPackageComplete += PlanPackageComplete;
            //43 當引擎完成安裝規劃時觸發。
            BootstrapperApplication.PlanComplete += PlanComplete;
            //44 當引擎開始安裝包時觸發。
            BootstrapperApplication.ApplyBegin += ApplyBegin;
            //46 當引擎完成安裝特定包時觸發。
            BootstrapperApplication.ExecutePackageComplete += ExecutePackageComplete;
            //48 當 Windows Installer 發送安裝消息時觸發
            BootstrapperApplication.ExecuteMsiMessage += ExecuteMsiMessage;
            //49 當引擎更改捆綁安裝的進度時觸發。
            BootstrapperApplication.Progress += ApplyProgress;
            //52 當引擎開始安裝特定包時觸發。
            BootstrapperApplication.ExecutePackageBegin += ExecutePackageBegin;
            //54 在引擎緩存安裝源後觸發
            BootstrapperApplication.CacheComplete += CacheComplete;
            //60 當引擎有進度獲取安裝源時觸發
            BootstrapperApplication.CacheAcquireProgress += CacheAcquireProgress;
            //73 在有效載荷上執行時由引擎觸發。
            BootstrapperApplication.ExecuteProgress += ApplyExecuteProgress;
            //74 當引擎完成安裝包時觸發。
            BootstrapperApplication.ApplyComplete += ApplyComplete;
        }


        private Dictionary<string, int> executingPackageOrderIndex;
        private int cacheProgress;
        private int executeProgress;
        private int progressPhases = 1;
        private InstallState _state = InstallState.Initializing;
        /// <summary>
        /// 版本
        /// </summary>
        public Version Version { get; set; }
        public InstallState State
        {
            get => _state;
            set
            {
                _state = value;
                if (DataHandler != null)
                {
                    _BootstrapperEventArgs.StateChange(_state);
                    DataHandler(this, _BootstrapperEventArgs);
                }

            }
        }

        private string _msg;
        private string Message
        {
            get => _msg;
            set
            {
                _msg = value;
                if (DataHandler != null)
                {
                    _BootstrapperEventArgs.MessageChange(_msg);
                    DataHandler(this, _BootstrapperEventArgs);
                }
            }
        }
        private string _amsg;
        private string AllMessage
        {
            get => _amsg;
            set
            {
                _amsg += value;
                if (DataHandler != null)
                {
                    _BootstrapperEventArgs.AllMessageChange(_amsg);
                    DataHandler(this, _BootstrapperEventArgs);
                }
            }
        }


        private int _progress;
        private int Progress
        {
            get => _progress;
            set
            {
                _progress = isUnstalling ? value * 2 : value;
                if (DataHandler != null)
                {
                    _BootstrapperEventArgs.ProgressChange(_progress);
                    DataHandler(this, _BootstrapperEventArgs);
                }
            }
        }
        private bool Canceled;
        /// <summary>
        /// 初始化計算參數
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlanBegin(object sender, PlanBeginEventArgs e)
        {
            lock (this)
            {
                if (State == InstallState.NotPresent)
                {
                    this.progressPhases = (LaunchAction.Layout == Model.PlannedAction) ? 1 : 2;
                }
                this.executingPackageOrderIndex.Clear();
            }
        }
        /// <summary>
        /// 檢測是否可以安裝
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetectComplete(object sender, DetectCompleteEventArgs e)
        {
            //當檢測完成後需卸載的情況
            if (LaunchAction.Uninstall == Model.Command.Action)
            {

                Model.Engine.Log(LogLevel.Verbose, "Invoking automatic plan for uninstall");
                Model.Engine.Plan(LaunchAction.Uninstall);
            }
            else if (HResult.Succeeded(e.Status))
            {

            }
            else
            {
                State = InstallState.Failed;
            }
        }
        /// <summary>
        /// 檢測產品是否已有安裝了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DetectPackageComplete(object sender, DetectPackageCompleteEventArgs e)
        {
            //產品安裝時的名稱
            var packageId = e.PackageId;
            //对应的是MsiPackage Id="DemoInstaller"
            if (e.PackageId.Equals("DemoInstaller", StringComparison.Ordinal))
            {
                State = e.State == PackageState.Present ? InstallState.Present : InstallState.NotPresent;
            }
            else 
            {
                State = InstallState.NotPresent;
            }
        }
        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlanPackageComplete(object sender, PlanPackageCompleteEventArgs e)
        {
            if (ActionState.None != e.Execute)
            {
                lock (this)
                {
                    Debug.Assert(!this.executingPackageOrderIndex.ContainsKey(e.PackageId));
                    this.executingPackageOrderIndex.Add(e.PackageId, this.executingPackageOrderIndex.Count);
                }
            }
        }
        /// <summary>
        /// PlanAction 结束后会触发这个方法 ，開始執行任務
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PlanComplete(object sender, PlanCompleteEventArgs e)
        {
            if (State == InstallState.Cancelled)
            {
                Dispatcher.InvokeShutdown();
                return;
            }
            //State = InstallState.Applying;
            ApplyAction();
        }

        /// <summary>
        /// ApplyAction 开始 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApplyBegin(object sender, ApplyBeginEventArgs e)
        {
            State = InstallState.Applying;
            AllMessage = isUnstalling ? "準備ing" : "解壓縮ing";
            AddAllMessage(true);

        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ExecutePackageComplete(object sender, ExecutePackageCompleteEventArgs e)
        {
            if (State == InstallState.Cancelled)
            {
                e.Result = Result.Cancel;
            }
        }

        /// <summary>
        /// 安裝訊息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecuteMsiMessage(object sender, ExecuteMsiMessageEventArgs e)
        {
            lock (this)
            {
                if (e.MessageType == InstallMessage.ActionStart)
                {
                    this.Message = e.Message;
                    AllMessage = $"\n{e.Message}";
                }

                e.Result = Canceled ? Result.Cancel : Result.Ok;
            }
        }


        /// <summary>
        /// 進度條
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CacheComplete(object sender, CacheCompleteEventArgs e)
        {
            lock (this)
            {
                this.cacheProgress = 100;
                this.Progress = (this.cacheProgress + this.executeProgress) / this.progressPhases;
            }
        }

        private void CacheAcquireProgress(object sender, CacheAcquireProgressEventArgs e)
        {
            lock (this)
            {
                this.cacheProgress = e.OverallPercentage;
                this.Progress = (this.cacheProgress + this.executeProgress) / this.progressPhases;
                e.Result = Canceled ? Result.Cancel : Result.Ok;
            }
        }
        private void ApplyExecuteProgress(object sender, ExecuteProgressEventArgs e)
        {
            lock (this)
            {
                this.executeProgress = e.OverallPercentage;
                this.Progress = (this.cacheProgress + this.executeProgress) / 2; // always two phases if we hit execution.
                if (Model.Command.Display == Display.Embedded)
                {
                    Model.Engine.SendEmbeddedProgress(e.ProgressPercentage, this.Progress);
                }

                e.Result = Canceled ? Result.Cancel : Result.Ok;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ExecutePackageBegin(object sender, ExecutePackageBeginEventArgs e)
        {
            if (State == InstallState.Cancelled)
            {
                e.Result = Result.Cancel;
            }
        }
        private void ApplyProgress(object sender, ProgressEventArgs e)
        {
            lock (this)
            {
                e.Result = Canceled ? Result.Cancel : Result.Ok;
            }
        }
        /// <summary>
        /// Apply结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            FinalResult = e.Status;
            AddAllMessage(false);
            Message = string.Empty;
            if (isUnstalling) 
            { 
                AllMessage = "\n卸載完成";
                State = InstallState.NotPresent; 
            }
            else
            {
                AllMessage = "\n安裝完成";
                State = InstallState.Present;
            }
            isUnstalling = false;
            //Dispatcher.InvokeShutdown();
        }
        private void AddAllMessage(bool run)
        {

            if (timer is null)
            {
                timer = new Timer(500);
                timer.Elapsed += (obj, e) =>
                {
                    AllMessage = ".";
                };
            }

            if (run)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }

        }

        /// <summary>
        /// 等待探測完成
        /// </summary>
        public void WaitDetect() 
        {
            while(State == InstallState.Initializing) 
            {
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
