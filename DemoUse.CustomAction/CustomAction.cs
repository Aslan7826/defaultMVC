using Microsoft.Deployment.WindowsInstaller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DemoUse.CustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult SetCustomAction(Session session)
        {
            //可使用的參數
            var path = Path.Combine(session["INSTALLFOLDER"], "appsettings.json");
            var systemIP = session["SYSTEMIP"];
            var webPort = session["WEBPORT"];
           
            //編輯Json檔
            var txtEditer = new TxtEditer(path);
            string txt = txtEditer.GetTxtData();
            var config = JsonConvert.DeserializeObject<JObject>(txt);
            config["URL"] = $"{systemIP}:{webPort}";
            txt = JsonConvert.SerializeObject(config, Formatting.Indented);
            txtEditer.EditTxtData(txt);

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult GetThisIP(Session session) 
        {
            string name = Dns.GetHostName();
            var ip = Dns.GetHostEntry(name).AddressList
                    .ToList()
                    .Where(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !o.ToString().EndsWith(".1"))
                    .FirstOrDefault()?.ToString() ?? "127.0.0.1";
            session["SYSTEMIP"] = ip;
            return ActionResult.Success;
        }

    }
}
