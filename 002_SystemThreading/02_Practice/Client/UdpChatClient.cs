using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client;

public class UdpChatClient
{
    private IPEndPoint serverEndPoint;
    private readonly UdpClient udpClient;

    public UdpChatClient(string serverIpAddress, int serverPort)
    {
        udpClient = new UdpClient();
        serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIpAddress), serverPort);
    }

    public void StartClient(string userName)
    {
        Console.Write("Введите сообщение (наберите 'exit' - для выхода): ");
        while (true)
        {
            var messageText = Console.ReadLine();
            if (messageText.ToLower() == "exit") break;

            var chatMessage = new ChatMessage(userName, messageText);
            var jsonMessage = chatMessage.ToJson();

            var buffer = Encoding.UTF8.GetBytes(jsonMessage);
            udpClient.Send(buffer, buffer.Length, serverEndPoint);
            
            // ответ от сервера
            var responseBuffer = udpClient.Receive(ref serverEndPoint);
            var responseMessage = Encoding.UTF8.GetString(responseBuffer);
            Console.WriteLine($"Ответ от сервера: {responseMessage}");
            Console.Write("Введите следующее: ");
        }
    }
}