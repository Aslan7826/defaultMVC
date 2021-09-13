using DemoUse.WPFView.Models;
using DemoUse.WPFView.ViewModels;
using DemoUse.WPFView.Views;
using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var View = new InstallView(bootstrapper);

            bootstrapper.HanlderStart();
            bootstrapper.SetWindowHandle(View);
            this.Engine.Detect();
            View.Show();
            Dispatcher.Run();
            this.Engine.Quit(bootstrapper.FinalResult);
        }
    }
}
