using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal class UdpChatServer
{
    public void StartUdpServer()
    {
        using (var udpClient = new UdpClient(9999))
        {
            var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Console.WriteLine("Ждем сообщений от клиента.");

            while (true)
            {
                var buffer = udpClient.Receive(ref ipEndPoint);
                var message = Encoding.UTF8.GetString(buffer);
                try
                {
                    var chatMessage = ChatMessage.FromJson(message);
                    Console.WriteLine(chatMessage?.Print() ?? "Invalid message format.");

                    // подтверждение для клиента
                    var confirmation =
                        $"Сообщение '{chatMessage?.MessageText}' от {chatMessage?.SenderNickname} получено в {chatMessage?.Timestamp}.";
                    var confirmationBytes = Encoding.UTF8.GetBytes(confirmation);
                    udpClient.Send(confirmationBytes, confirmationBytes.Length, ipEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
            }
        }
    }
}