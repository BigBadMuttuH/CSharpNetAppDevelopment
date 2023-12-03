using System.Net;
using System.Net.Sockets;

namespace ServerUdp;
// TODO: 1:21:00

internal static class Program
{
    private static void Main(string[] args)
    {
        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            var localEndPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 1234);
            socket.Bind(localEndPoint);

            var buffer = new byte[1];
            var count = 0;
            while (count < 200)
            {
                EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                // фильтрация 
                // var c = socket.ReceiveFrom(buffer, ref endPoint);

                // еще вариант с фильтрацией 
                var sf = new SocketFlags();
                var c = socket.ReceiveMessageFrom(buffer, 0, 1, ref sf, ref endPoint,
                    out var information);

                // int c = socket.Receive(buffer);
                if (c == 1)
                    if ((endPoint as IPEndPoint)?.Port == 2235)
                        Console.Write(buffer[0]);
                count += c;
            }

            Console.WriteLine("\nпрочитано 200 байт");
        }
    }
}