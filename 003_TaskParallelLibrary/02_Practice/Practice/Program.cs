using System.Net;
using System.Net.Sockets;

namespace Practice;

internal static class Program
{
    private static readonly int[] Array1 = { 2, 2, 2, 2, 2 };
    private static readonly int[] Array2 = { 1, 1, 1, 1, 1 };

    public static void Main(string[] args)
    {
        Console.WriteLine($"Сумма: {Ex01().Result}");
    }

    // Напишите приложение для одновременного выполнения двух задач в потоках созданных с помощью Task.
    // Нужно подсчитать сумму элементов каждого из массивов а потом сложить эти суммы полученные после
    // выполнения каждого из потоков и вывести результат на экран.
    private static async Task<int> Ex01()
    {
        var t1 = Task.Run(() => Array1.Sum());
        var t2 = Task.Run(() => Array2.Sum());
        var sum1 = await t1;
        var sum2 = await t2;

        return sum1 + sum2;
    }
}