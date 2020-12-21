using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OnigiriBot
{
    public static class Helper
    {
        public static string GetLocalIPAddress()
        {
            return new WebClient().DownloadString("http://ipinfo.io/ip").Trim();
        }
    }
}
