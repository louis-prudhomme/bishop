using log4net;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Bishop.Config
{
    class HerokuConfigurator
    {
        private readonly int _port;
        
        public HerokuConfigurator(string port)
        {
            _port = int.Parse(port);
        }

        public void Herocul()
        { 
            new TcpListener(IPAddress.Any, _port).Start();
        }
    }
}
