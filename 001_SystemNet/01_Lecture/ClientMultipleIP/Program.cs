using System.Net;
using System.Net.Sockets;

namespace ClientMultipleIP;

public static class Program
{
    static void ConnectYa(Socket client, IPAddress[] addresses, bool reconnect)
    {
        try
        {
            if (reconnect)
            {
                var task = client.ConnectAsync(addresses, 80);
                task.Wait();
            }
            else
            {
                client.Connect(addresses, 80);
            }
        }
        catch
        {
        }
        if (client.Connected)
        {
            Console.WriteLine("Connected!");
            Console.WriteLine($"LocalEndPoint = {client.LocalEndPoint}");
            Console.WriteLine($"remoteEndPoint = {client.RemoteEndPoint}");
        }
        else
        {
            Console.WriteLine("Connection problem!");
        }
        Console.WriteLine("Disconnecting...");
        client.Disconnect(true);
        Console.WriteLine("Disconnected!");
    }
    
    
    private static void Main(string[] args)
    {
        using (var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            Console.WriteLine("Connecting...");

            var addresses = Dns.GetHostAddresses("yandex.ru");
            Console.WriteLine("Адреса:");
            foreach (var a in addresses) Console.WriteLine(a);
            
            ConnectYa(client, addresses,false);
            ConnectYa(client, addresses,true);
        }
    }
}