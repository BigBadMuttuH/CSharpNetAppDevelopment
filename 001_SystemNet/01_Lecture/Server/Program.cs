using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal static class Program
{
    private static void Main(string[] args)
    {
        using (var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            var localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345); //65535 -Max
            // Блокирующий vs Неблокирующий Режим:
            // listener.Blocking = false делает сокет неблокирующим.
            // Это значит, что Accept не будет блокировать поток, пока не появится подключение.
            // Вместо этого программа будет циклически проверять наличие подключения.
            // listener.Blocking = true;
            listener.Blocking = false;

            Console.WriteLine("Сокет привязан: " + listener.IsBound);
            listener.Bind(localEndPoint);
            listener.Listen(100);
            Console.WriteLine("Сокет привязан: " + listener.IsBound);
            
            Console.WriteLine("Waiting for connection ...");

            while (!listener.Poll(100, SelectMode.SelectRead))
            {
                Console.Write('*');
                Thread.Sleep(500);
            }

            Socket? socket = null;
            do
            {
                try
                {
                    socket = listener.Accept();
                    
                    // С заданием в catch не зайдем
                    // Task<Socket> task = listener.AcceptAsync();
                    // task.Wait();
                    // socket = task.Result;
                    
                    // LingerState:
                    // socket.LingerState = new LingerOption(true, 1)
                    // устанавливает поведение сокета при закрытии.
                    // Он ожидает одну секунду перед полным закрытием, чтобы убедиться, что все данные отправлены. 
                }
                catch
                {
                    Console.Write('.');
                    Thread.Sleep(1_000);
                }
            } while (socket == null);


            // var socket = listener.Accept();
            Console.WriteLine("\nConnected!");
            Console.WriteLine($"localEndPoint = {socket.LocalEndPoint}");
            Console.WriteLine($"remoteEndPoint = {socket.RemoteEndPoint}");
            
            var buffer = new byte[255];

            while (socket.Available == 0) ;
            Console.WriteLine($"Доступно {socket.Available} байт для чтения.");
            // Можно выбрать буфер по размеру доступного для чтения,
            // но лучше так не делать, а выбрать максимально возможный
            // или близкий к максимально возможному
            // var buffer = new byte[socket.Available];
            // Например так:
            // var buffer = new byte[255];

            socket.ReceiveTimeout = 5_000;

            var count = socket.Receive(buffer);

            if (count > 0)
            {
                var message = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine("Сообщение не получено!");
            }

            listener.Close();
        }
    }
}