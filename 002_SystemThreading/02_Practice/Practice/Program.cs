using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Practice;

public static class Program
{
    private static readonly int[] Array1 = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };

    private static readonly int[] Array2 = { 10, 10, 10, 10 };

    // Напишите приложение для одновременного выполнения двух задач в потоках.
    // Нужно подсчитать сумму элементов каждого из массивов, а потом
    // сложить эти суммы полученные после выполнения каждого из потоков и вывести результат на экран.
    // static int ArraySum(int [] array)
    // {
    //     int sum = array.Sum();
    //     Console.WriteLine(sum);
    //     return sum;
    // }
    private static object ArraySum(object array)
    {
        var a = (int[])array;
        return a.Sum();
    }

    private static void Ex01()
    {
        var arraySum1 = 0;
        var arraySum2 = 0;
        new Thread(() => { arraySum1 = (int)ArraySum(Array1); }).Start();
        new Thread(() => { arraySum2 = (int)ArraySum(Array2); }).Start();
        Thread.Sleep(1); // без этой строчки будет 0

        Console.WriteLine($"Sum of arrays = {arraySum1 + arraySum2}");
    }

    // Напишите многопоточное приложение,
    // которое определяет все IP-адреса интернет-ресурса и определяет до которого из них лучше Ping.
    private static void Ex02()
    {
        var site = "yandex.ru";

        var ipAddress = Dns.GetHostAddresses(site, AddressFamily.InterNetwork);
        foreach (var address in ipAddress)
            Console.WriteLine(address);

        var destinations = new Dictionary<IPAddress, long>();

        var threads = new List<Thread>();
        foreach (var address in ipAddress)
        {
            var t = new Thread(() =>
            {
                var p = new Ping();
                var pingReply = p.Send(address);
                destinations.Add(address, pingReply.RoundtripTime);
                Console.WriteLine($"IP:{address}, ping {pingReply.RoundtripTime}");
            });
            threads.Add(t);
            t.Start();
        }

        // тут имитация какой-то деятельности
        foreach (var thread in threads) thread.Join();
        
        // long minPing = destinations.Min(x => x.Value);
        var minPing = destinations.MinBy(kvp => kvp.Value);
        Console.WriteLine($"Минимальное значение до: {minPing.ToString()}");
    }

    private static void Main(string[] args)
    {
        // Ex01();
        Ex02();
    }
}