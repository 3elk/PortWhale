using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MainSpace
{
    class PortScanner
    {
        public async Task ScanAsync(string ipAddress, int startPort, int endPort)
        {
            var openPorts = new ConcurrentBag<int>();
            var tasks = new List<Task>();

            for (int port = startPort; port <= endPort; port++)
            {
                tasks.Add(ScanPortAsync(ipAddress, port, openPorts));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine("SCAN COMPLETED! | 🚀");
        }

        private async Task ScanPortAsync(string ipAddress, int port, ConcurrentBag<int> openPorts)
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    var connectTask = tcpClient.ConnectAsync(ipAddress, port);
                    if (await Task.WhenAny(connectTask, Task.Delay(200)) == connectTask)
                    {
                        openPorts.Add(port);
                        Console.WriteLine($"PORT {port} is OPEN | ✅");
                    }
                    else
                    {
                        Console.WriteLine($"PORT {port} is CLOSED | ❌");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"PORT {port} is CLOSED | ❌");
                }
            }
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine(@"
   ___           _   __    __ _           _                  . 
  / _ \___  _ __| |_/ / /\ \ \ |__   __ _| | ___          ___:____     |""\/""
 / /_)/ _ \| '__| __\ \/  \/ / '_ \ / _` | |/ _ \       ,'        `.    \  /
/ ___/ (_) | |  | |_ \  /\  /| | | | (_| | |  __/       |  O        \___/  |
\/    \___/|_|   \__| \/  \/ |_| |_|\__,_|_|\___|~^~^~^~^~^~^~^~^~^~^~^~^~~^~^~^~

                            The Ultimate Port Scanner
                            By https://github.com/3elk  
            ");
            Console.Write("Enter IP Address >> "); string ipAddress = Console.ReadLine(); Console.Write("Enter Start Port >> "); int startPort = int.Parse(Console.ReadLine()); Console.Write("Enter End Port >> "); int endPort = int.Parse(Console.ReadLine()); PortScanner scanner = new PortScanner(); await scanner.ScanAsync(ipAddress, startPort, endPort);
        }
    }
}
