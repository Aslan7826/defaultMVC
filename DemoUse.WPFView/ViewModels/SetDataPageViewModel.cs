using DemoUse.CustomAction;
using DemoUse.WPFView.Models;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace MaxPower.NetPro.Setup.WPFView.ViewModels
{
    public class SetDataPageViewModel : PropertyNotifyBase
    {
        internal BootstrapperApplicationModel _model;
        /// <summary>
        /// 安裝命令
        /// </summary>
        public ICommand InstallCommand { get; private set; }
        public ICommand SelectedFolderCommand { get; private set; }

        public SetDataPageViewModel(BootstrapperApplicationModel model)
        {
            _model = model;
            InstallFollder = @"C:\Program Files (x86)\DemoUse";
            SystemIP = new GetLocalData().GetThisIP();
            InstallCommand = new RelayCommand(param => Install(), param => true);
            SelectedFolderCommand = new RelayCommand(param => Browse(), param => true);
        }

        private string _installFollder;
        public string InstallFollder
        {
            get
            {

                return _installFollder;
            }
            set
            {
                if (_installFollder != value && ValidDir(value))
                {
                    _installFollder = value;
                    OnPropertyChanged("InstallFollder");
                    _model.SetBurnVariable("InstallFolder", value);
                }
            }
        }

        private string _systemIP;
        public string SystemIP
        {
            get => _systemIP;
            set
            {
                _systemIP = value;
                OnPropertyChanged("SystemIP");
                _model.SetBurnVariable("SystemIP", value);
            }
        }


        private string _webPort;
        public string WebPort
        {
            get => _webPort;
            set
            {
                _webPort = value;
                OnPropertyChanged("WebPort");
                _model.SetBurnVariable("WebPort", value);
            }
        }

        public void Browse()
        {
            var folderBrowserDialog = new FolderBrowserDialog { SelectedPath = InstallFollder };

            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                InstallFollder = folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        /// path是否是正确的文件夹路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool ValidDir(string path)
        {
            try
            {
                string p = new DirectoryInfo(path).FullName;
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 關閉事件
        /// </summary>
        public Action Close;

        /// <summary>
        /// 安裝
        /// </summary>
        public void Install()
        {
            _model.PlanAction(LaunchAction.Install);
            Close();
        }
    }
}
