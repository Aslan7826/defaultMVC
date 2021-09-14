using DemoUse.WPFView.Models;
using DemoUse.WPFView.Models.Enums;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DemoUse.WPFView.ViewModels
{
    public class RevisionViewModel : PropertyNotifyBase
    {
        /// <summary>
        /// 需要显示在WPFWindow
        /// </summary>

        internal BootstrapperApplicationModel _Model;



        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model"></param>
        public RevisionViewModel(BootstrapperApplicationModel model)
        {
            this._Model = model;
            this._Model.DataHandler += DataChangeShow;

            UninstallCommand = new RelayCommand(param =>
            {
                if (System.Windows.MessageBox.Show("確定狠心移除嗎？", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _Model.isUnstalling = true;
                    _Model.PlanAction(LaunchAction.Uninstall);
                }
            }, param => State == InstallState.Present);
            RepairCommand = new RelayCommand(param =>
            {

                if (System.Windows.MessageBox.Show("即將進入系統修復？", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    _Model.PlanAction(LaunchAction.Repair);
                }
            }
             , param => State == InstallState.Present);

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


        #region Command

        public ICommand UninstallCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand RepairCommand { get; private set; }

        #endregion
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

        public void DataChangeShow(object obj, BootstrapperEventArgs args)
        {
            AllMessage = args.AllMessage;
            Progress = args.Progress;
            Message = args.Message;
            State = args.State;
        }
    }
}
