using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal class UdpChatServer
{
    private readonly UdpClient udpClient;
    private bool isRunning;

    public UdpChatServer(int port)
    {
        udpClient = new UdpClient(port);
        isRunning = true;
    }

    public void StartUdpServer()
    {
        var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
        Console.WriteLine("Ждем сообщений. Нажмите 'q' для выхода.");

        var listenThread = new Thread(() =>
        {
            while (isRunning)
                if (udpClient.Available > 0)
                {
                    var clientThread = new Thread(HandleClient);
                    clientThread.Start();
                }
        });
        listenThread.Start();

        // Ожидаем нажатия клавиши 'q' для выхода
        while (Console.ReadKey(true).Key != ConsoleKey.Q)
        {
        }

        isRunning = false;
        udpClient.Close();
        Console.WriteLine("Сервер остановлен.");
    }

    private void HandleClient()
    {
        try
        {
            var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

            if (udpClient.Available > 0)
            {
                var buffer = udpClient.Receive(ref clientEndPoint);
                var message = Encoding.UTF8.GetString(buffer);
                var chatMessage = ChatMessage.FromJson(message);
                Console.WriteLine(chatMessage?.Print() ?? "Не верный формат сообщения.");

                // подтверждение для клиента
                var confirmation =
                    $"Сообщение '{chatMessage?.MessageText}' от {chatMessage?.SenderNickname} получено в {chatMessage?.Timestamp}.";
                var confirmationBytes = Encoding.UTF8.GetBytes(confirmation);
                udpClient.Send(confirmationBytes, confirmationBytes.Length, clientEndPoint);
            }
            else
            {
                // Небольшая задержка для снижения нагрузки на процессор
                Thread.Sleep(100);
            }
        }
        catch (SocketException ex) when (ex.SocketErrorCode == SocketError.Interrupted)
        {
            // Это исключение можно игнорировать, так как оно ожидаемо при закрытии сокета
            Console.WriteLine("Обработка сообщения была прервана.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обработке сообщения: {ex.Message}");
        }
    }
}