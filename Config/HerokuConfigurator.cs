using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Bishop.Config
{
    class HerokuConfigurator
    {
        public static void Herocul(string port)
        {
            new TcpListener(int.Parse(port)).Start();
        }
    }
}
