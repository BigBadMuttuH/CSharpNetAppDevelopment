using System.Net;
using System.Net.Sockets;

namespace ClientUdp;

internal class Program
{
    private static void Send(byte[] buffer, Socket socket)
    {
        for (var i = 0; i < 100; i++) socket.Send(buffer);
        socket.Close();
    }

    private static void Main(string[] args)
    {
        var socket1 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        var socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        var localEP1 = new IPEndPoint(IPAddress.Any, 2234);
        var localEP2 = new IPEndPoint(IPAddress.Any, 2235);

        socket1.Bind(localEP1);
        socket2.Bind(localEP2);

        socket1.Connect("127.0.0.1", 1234);
        socket2.Connect("127.0.0.1", 1234);

        new Thread(() => Send(new byte[] { 1 }, socket1)).Start();
        new Thread(() => Send(new byte[] { 2 }, socket2)).Start();
    }
}