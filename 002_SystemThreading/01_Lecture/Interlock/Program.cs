namespace Interlock;

class Program
{
    static int x = 0;

    static void Increment()
    {
        for (int i = 0; i < 10_000; i++)
        {
            // x++; // так результат не всегда гарантирован. может быть и 10000, а может и 9000
            // чтобы это избежать можно воспользоваться Interlocked
            Interlocked.Increment(ref x);
        }

        Console.WriteLine("done");
    }

    static void Ex01()
    {
        for (int i = 0; i < 10; i++)
        {
            new Thread(Increment).Start();
        }
        Thread.Sleep(2_000);
        Console.WriteLine(x);
    }

    static void Main(string[] args)
    {
        Ex01();
    }
}

