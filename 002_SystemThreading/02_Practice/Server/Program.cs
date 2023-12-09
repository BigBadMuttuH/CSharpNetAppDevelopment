namespace Server;

internal static class Program
{
    private static void Main(string[] args)
    {
        var server = new UdpChatServer(9999);
        server.StartUdpServer();
    }
}