namespace Server;

internal class Program
{
    private static void Main(string[] args)
    {
        var server = new UdpChatServer();
        server.StartUdpServer();
    }
}