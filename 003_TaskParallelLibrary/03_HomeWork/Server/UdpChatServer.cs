using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal class UdpChatServer
{
    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly UdpClient udpClient;

    public UdpChatServer(int port)
    {
        udpClient = new UdpClient(port);
        cancellationTokenSource = new CancellationTokenSource();
    }

    public async Task StartUdpServerAsync()
    {
        var ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
        Console.WriteLine("Ждем сообщений. Нажмите 'q' для выхода.");

        var listenTask = ListenAsync(ipEndPoint, cancellationTokenSource.Token);

        await WaitForExitAsync();
        cancellationTokenSource.Cancel();

        try
        {
            await listenTask;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Операция была отменена.");
        }

        udpClient.Close();
        Console.WriteLine("Сервер остановлен.");
    }

    private async Task ListenAsync(IPEndPoint ipEndPoint, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
            if (udpClient.Available > 0)
            {
                var result = await udpClient.ReceiveAsync(token);
                HandleClient(result.Buffer, result.RemoteEndPoint);
            }
            else
            {
                await Task.Delay(100, token);
            }
    }

    private void HandleClient(byte[] buffer, IPEndPoint clientEndPoint)
    {
        var message = Encoding.UTF8.GetString(buffer);
        var chatMessage = ChatMessage.FromJson(message);
        Console.WriteLine(chatMessage?.Print() ?? "Не верный формат сообщения.");

        var confirmation =
            $"Сообщение '{chatMessage?.MessageText}' от {chatMessage?.SenderNickname} получено в {chatMessage?.Timestamp}.";
        var confirmationBytes = Encoding.UTF8.GetBytes(confirmation);
        udpClient.Send(confirmationBytes, confirmationBytes.Length, clientEndPoint);
    }

    private static async Task WaitForExitAsync()
    {
        await Task.Run(() =>
        {
            while (Console.ReadKey(true).Key != ConsoleKey.Q)
            {
            }
        });
    }
}