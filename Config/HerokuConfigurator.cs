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

        private readonly ILog _log;
        private readonly int _port;
        

        private int counter = 0;

        public HerokuConfigurator(string port, ILog log)
        {
            _log = log;
            _port = int.Parse(port);
        }

        public void Herocul()
        {
            new Thread(new ThreadStart(Ping)).Start();
        }

        private void Ping()
        {
            using (var ping = new DisposableTcpListener(_port))
            {
                ping.Start();
            }
            
            _log.Info($"Pinged for the {++counter}th time");

            Thread.Sleep(SLEEP_TIME_MILLI);
            Ping();
        }

        public class DisposableTcpListener : IDisposable
        {
            private static readonly IPAddress ADDRESS = IPAddress.Any;
            private readonly TcpListener _underlying;

            public DisposableTcpListener(int port) 
            { 
                _underlying = new TcpListener(ADDRESS, port);
            }

            public void Dispose()
            {
                Stop();
            }

            public void Start() { _underlying.Start(); }
            public void Stop() { _underlying.Stop(); }
        }
    }
}
