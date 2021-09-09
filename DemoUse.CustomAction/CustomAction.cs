using Microsoft.Deployment.WindowsInstaller;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DemoUse.CustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult SetCustomAction(Session session)
        {
            //�i�ϥΪ��Ѽ�
            var path = Path.Combine(session["INSTALLFOLDER"], "appsettings.json");
            var systemIP = session["SYSTEMIP"];
            var webPort = session["WEBPORT"];
           
            //�s��Json��
            var txtEditer = new TxtEditer(path);
            string txt = txtEditer.GetTxtData();
            var config = JsonConvert.DeserializeObject<JObject>(txt);
            config["URL"] = $"{systemIP}:{webPort}";
            txt = JsonConvert.SerializeObject(config, Formatting.Indented);
            txtEditer.EditTxtData(txt);

            return ActionResult.Success;
        }
    }
}
