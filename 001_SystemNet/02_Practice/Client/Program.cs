using System.Security.Principal;

namespace Client;

internal class Program
{
    private static void Main(string[] args)
    {
        var serverIpAddress = "127.0.0.1";
        var serverProt = 9999;
        // Console.WriteLine("Ваше имя: ");
        // string userName = Console.ReadLine();
        var userName = WindowsIdentity.GetCurrent().Name;

        var client = new UdpChatClient(serverIpAddress, serverProt);
        client.StartClient(userName);
    }
}