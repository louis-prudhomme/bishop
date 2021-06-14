using log4net;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Bishop.Config
{
    class HerokuConfigurator
    {
        private static readonly int SLEEP_TIME_MILLI = 600000;

        private readonly int _port;
        

        private int counter = 0;

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
