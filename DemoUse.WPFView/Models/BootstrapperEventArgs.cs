using DemoUse.WPFView.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoUse.WPFView.Models
{
    public class BootstrapperEventArgs : EventArgs
    {
        InstallState state;
        string message;
        string allMessage;
        int progress;
        public void MessageChange(string message)
        {
            this.message = message;
        }
        public void AllMessageChange(string allmessage)
        {
            this.allMessage = allmessage;
        }

        public void ProgressChange(int progress)
        {
            this.progress = progress;
        }

        public void StateChange(InstallState state)
        {
            this.state = state;
        }

        public InstallState State => state;
        public string Message => message;
        public string AllMessage => allMessage;
        public int Progress => progress;

    }
    public delegate void BootstrapperEventHandler(object obj, BootstrapperEventArgs args);
}
