using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DemoUse.CustomAction
{
    public class GetLocalData
    {
        public string GetThisIP() 
        {
            string name = Dns.GetHostName();
            var ip = Dns.GetHostEntry(name).AddressList
                    .ToList()
                    .Where(o => o.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && !o.ToString().EndsWith(".1"))
                    .FirstOrDefault()?.ToString() ?? "127.0.0.1";
            return ip;
        }
        /// <summary>
        /// 取得機碼路徑
        /// </summary>
        /// <returns></returns>
        public string GetPathRegistryKey()
        {
            string result = string.Empty;
            var key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\WOW6432Node\\Demo\\DemoUse.Installer", false)
                    ?? Registry.LocalMachine.OpenSubKey("SOFTWARE\\Demo\\DemoUse.Installer", false);
            if (key != null)
            {
                result = key.GetValue("InstallDir").ToString();
            }
            return result;
        }
    }
}
