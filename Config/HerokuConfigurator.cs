using log4net;
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
            new TcpListener(System.Net.IPAddress.Any, _port)
                .Start();

            new Thread(new ThreadStart(Ping)).Start();
        }

        private void Ping()
        {
            using (var ping = new System.Net.NetworkInformation.Ping())
            {
                ping.Send("ping.me");
            }
            
            _log.Info($"Pinged for the {counter++}th time");

            Thread.Sleep(SLEEP_TIME_MILLI);
            Ping();
        }
    }
}
