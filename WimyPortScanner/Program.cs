using System;
using System.Threading.Tasks;

namespace WimyPortScanner
{
    class Program
    {
        static bool IsOpenedPort(string hostname, int port)
        {
            using (System.Net.Sockets.TcpClient tcpClient = new System.Net.Sockets.TcpClient())
            {
                try
                {
                    tcpClient.Connect(hostname, port);
                    if (tcpClient.Connected)
                    {
                        return true;
                    }
                }
                catch
                {

                }
                return false;
            }
        }

        static bool StartCheck(string hostname, int startPort, int endPort)
        {
            System.Collections.Concurrent.ConcurrentQueue<int> queue = new System.Collections.Concurrent.ConcurrentQueue<int>();
            Parallel.For(startPort, endPort, port => {
                bool isConnected = IsOpenedPort(hostname, port);
                if (isConnected)
                {
                    queue.Enqueue(port);
                    Console.Write("[{0}]", port);
                }
                else
                {
                    Console.Write(".");
                }
            });
            Console.WriteLine("");

            Console.WriteLine("---------- Result ----------");
            Console.WriteLine("hostname: {0}", hostname);
            Console.WriteLine("portRange: {0} - {1}", startPort, endPort);
            foreach (int openedPort in queue)
            {
                Console.WriteLine("{0} is opened", openedPort);
            }
            Console.WriteLine("---------- The End ----------");
            return true;
        }

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Usage: WimyPortScanner hostname startPort endPort");
                Environment.ExitCode = 1;
                return;
            }
            string hostname = args[0];
            int startPort = int.Parse(args[1]);
            int endPort = int.Parse(args[2]);
            if (startPort < 1)
            {
                startPort = 1;
            }
            if (endPort < startPort)
            {
                endPort = startPort;
            }
            if (endPort >= UInt16.MaxValue)
            {
                endPort = UInt16.MaxValue;
            }

            Console.WriteLine("hostname: {0}", hostname);
            Console.WriteLine("portRange: {0} - {1}", startPort, endPort);
            StartCheck(hostname, startPort, endPort);
        }
    }
}
