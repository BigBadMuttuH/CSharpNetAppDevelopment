namespace Server;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var server = new UdpChatServer(9999);
        await server.StartUdpServerAsync();
    }
}