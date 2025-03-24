using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Linq;

class PortScanner
{
    static void Main(string[] args)
    {

        Console.Title = "PortWhale - Port Scanner";
        Console.WriteLine(@"
   ___           _   __    __ _           _                 \'/ 
  / _ \___  _ __| |_/ / /\ \ \ |__   __ _| | ___          ___:____     |""\/""
 / /_)/ _ \| '__| __\ \/  \/ / '_ \ / _` | |/ _ \       ,'        `.    \  /
/ ___/ (_) | |  | |_ \  /\  /| | | | (_| | |  __/       |  O        \___/  |
\/    \___/|_|   \__| \/  \/ |_| |_|\__,_|_|\___|~^~^~^~^~^~^~^~^~^~^~^~^~~^~^~^~

                            Simple Port Scanner
                        BY: https://3elk.github.io

");
        Console.Write("Enter IP Address >> ");
        string ipAddress = Console.ReadLine();
        Console.Write("Enter start port >> ");
        int startPort = int.Parse(Console.ReadLine());
        Console.Write("Enter end port >> ");
        int endPort = int.Parse(Console.ReadLine());

        Console.WriteLine("\n~^~^~^~^~^~^STARTING SCAN~^~^~^~^~^~^\n");
        var openPorts = new ConcurrentBag<int>();
        Parallel.For(startPort, endPort + 1, port =>
        {
            ScanPort(ipAddress, port, openPorts);
        });

        var sortedPorts = openPorts.OrderBy(p => p).ToList();
        Console.WriteLine($"\n^~^~^~^~SCAN COMPLETED~^~^~^~^\n");
        Console.WriteLine($"!FOUND {sortedPorts.Count} OPEN PORTS!\n");

        if (sortedPorts.Any())
        {
            Console.WriteLine("OPEN PORTS >>\n");
            foreach (var port in sortedPorts)
            {
                Console.WriteLine($"PORT {port} | OPEN");
            }
        }

        Console.WriteLine("\nPress Enter to exit . . .");
        Console.ReadKey();
    }

    static void ScanPort(string ipAddress, int port, ConcurrentBag<int> openPorts)
    {
        using (TcpClient tcpClient = new TcpClient())
        {
            try
            {
                tcpClient.Connect(ipAddress, port);
                openPorts.Add(port);
                Console.WriteLine($"PORT {port} | OPEN");
            }
            catch(Exception)
            {
                Console.WriteLine($"PORT {port} | CLOSED");
            }
            finally
            {
                tcpClient.Close();
            }
        }
    }
}
