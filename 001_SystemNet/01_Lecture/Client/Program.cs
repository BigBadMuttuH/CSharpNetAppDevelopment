using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

public static class Program
{
    private static void Main(string[] args)
    {
        using (var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            var remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            var localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12346);
            Console.WriteLine("Connecting...");

            // привязали клиента к сокету
            // теперь он не рандомный
            client.Bind(localEndPoint);

            // client.NoDelay = true;
            // client.ProtocolType
            // client.ReceiveBufferSize
            // client.ReceiveTimeout

            try
            {
                client.Connect(remoteEndPoint);
            }
            catch(SocketException e)
            {
                Console.WriteLine($"ErrorCode {e.ErrorCode}, message = {e.Message}");
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
                return;
            }

            var bytes = Encoding.UTF8.GetBytes("Привет.");
            Console.WriteLine("Нажмите клавишу для отправки.");
            Console.ReadKey();
            
            var count = client.Send(bytes);
            if (count == bytes.Length)
                Console.WriteLine("Отправлено!");
            else
                Console.WriteLine("Что-то пошло не так.");

            // if (client.Poll(100, SelectMode.SelectWrite) && client.Poll(100, SelectMode.SelectError))
            // {
            //     var count = client.Send(bytes);
            //
            //     if (count == bytes.Length)
            //         Console.WriteLine("Отправлено!");
            //     else
            //         Console.WriteLine("Что-то пошло не так.");
            // }
        }
    }
}