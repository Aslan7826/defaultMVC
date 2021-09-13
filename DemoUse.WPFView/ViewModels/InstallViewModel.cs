using DemoUse.WPFView.Models;
using DemoUse.WPFView.Models.Enums;
using Microsoft.TeamFoundation.Common.Internal;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DemoUse.WPFView.ViewModels
{
    public class InstallViewModel : PropertyNotifyBase
    {



        /// <summary>
        /// 需要显示在WPFWindow
        /// </summary>

        internal BootstrapperApplicationModel _Model;

        private bool isUnstalling = false;
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model"></param>
        public InstallViewModel(BootstrapperApplicationModel model)
        {
            this._Model = model;
            this._Model.DataHandler += DataChangeShow;

            /*
            //初始化命令 第一个参数是命令要触发的方法，第二个匿名函数是命令执行的条件
            UninstallCommand = new RelayCommand(param =>
            {
                this._Model.PlanAction(LaunchAction.Uninstall);
                isUnstalling = true;
            }, param => State == InstallState.Present);
            */
            CancelCommand = new RelayCommand(param =>
            {
                this._Model.LogMessage("Cancelling...");
                if (State == InstallState.Applying)
                {
                    State = InstallState.Cancelled;
                }
                else
                {
                    model.Dispatcher.InvokeShutdown();
                }
            }, param => State != InstallState.Cancelled);
        }

        #endregion


        #region Command
        /// <summary>
        /// 执行安装命令
        /// </summary>
        public ICommand UninstallCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand LaunchNewsCommand { get; private set; }
        private ICommand repairCommand;

        public ICommand RepairCommand
        {
            get
            {
                return this.repairCommand ?? (this.repairCommand = new RelayCommand(param =>
                    _Model.PlanAction(LaunchAction.Repair)
                 , param => State == InstallState.Present));
            }
        }

        #endregion

        #region 属性

        /// <summary>
        /// 版本
        /// </summary>
        public Version Version => _Model.Version;
        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                if (message != value)
                {

                    message = string.IsNullOrEmpty(value) ? value : "狀態 :" + value;
                    OnPropertyChanged("Message");
                }
            }
        }

        private string allMessage;
        public string AllMessage
        {
            get
            {
                return allMessage;
            }
            set
            {
                if (allMessage != value)
                {
                    allMessage = value;
                    OnPropertyChanged("AllMessage");
                }
            }
        }
        private InstallState state;
        public InstallState State
        {
            get
            {
                return state;
            }
            set
            {
                if (state != value)
                {
                    state = value;
                    OnPropertyChanged("State");
                }
            }
        }


        private int progress;
        public int Progress
        {
            get
            {
                return this.progress;
            }
            set
            {
                this.progress = value;
                OnPropertyChanged("Progress");

            }
        }


        #endregion
        public void DataChangeShow(object obj, BootstrapperEventArgs args)
        {
            AllMessage = args.AllMessage;
            Progress = args.Progress;
            Message = args.Message;
            State = args.State;
        }




    }

}
