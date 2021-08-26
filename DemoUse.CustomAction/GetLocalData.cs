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
    }
}
