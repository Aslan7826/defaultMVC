﻿using DemoUse.WPFView.Models;
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

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model"></param>
        public InstallViewModel(BootstrapperApplicationModel model)
        {
            this._Model = model;
            this._Model.DataHandler += DataChangeShow;

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

        public ICommand CancelCommand { get; private set; }


        #endregion

        #region 属性

        public bool InstallEnabled => state == InstallState.NotPresent;
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
                    OnPropertyChanged("InstallEnabled");
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
