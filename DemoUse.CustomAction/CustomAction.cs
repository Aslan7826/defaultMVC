using Microsoft.Deployment.WindowsInstaller;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoUse.CustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult SetCustomAction(Session session)
        {
           



            return ActionResult.Success;
        }
    }
}
