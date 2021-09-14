using DemoUse.WPFView.Models;
using DemoUse.WPFView.ViewModels;
using DemoUse.WPFView.Views;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DemoUse.WPFView
{
    public class CustomBootstrapperApplication :BootstrapperApplication
    {

        protected override void Run()
        {
            var bootstrapper = new BootstrapperApplicationModel(this)
            {
                Version = new Version("1.0.0")
            };
            this.Engine.Detect();
            bootstrapper.WaitDetect();
            var View = bootstrapper.State == Models.Enums.InstallState.Present
                     ? (Window)new RevisionView(bootstrapper)
                     :  new InstallView(bootstrapper)
                     ;
            bootstrapper.SetWindowHandle(View);
            View.Show();
            Dispatcher.Run();
            this.Engine.Quit(bootstrapper.FinalResult);
        }
    }
}
