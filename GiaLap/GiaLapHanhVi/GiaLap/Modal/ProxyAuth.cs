using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaLap.Modal
{
    public class ProxyAuthent
    {
        public string ip { get; set; }
        public int port { get; set; }
        public string username { get; set; }
        public string pass { get; set; }

        public ProxyAuthent(string ip, int port, string username, string pass)
        {
            this.ip = ip;
            this.port = port;
            this.username = username;
            this.pass = pass;
        }
    }
}
