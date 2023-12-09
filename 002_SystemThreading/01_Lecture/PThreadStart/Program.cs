using System.Globalization;

namespace PThreadStart;

internal static class Program
{
    private static bool isActive = true;
    
    private static void Main(string[] args)
    {
        // Ex01();
        // Ex02();
        Ex03();
    }

    private static void Ex01()
    {
        new Thread(
            () =>
            {
                Console.WriteLine(Thread.CurrentThread);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Console.WriteLine($"Current culture = {Thread.CurrentThread.CurrentCulture}");
                Console.WriteLine(DateTime.Now.ToString());

                Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

                Console.WriteLine($"Current culture = {Thread.CurrentThread.CurrentCulture}");
                Console.WriteLine(DateTime.Now.ToString());
            }).Start();
    }

    private static void Ex02()
    {
        var t = new Thread(
            () =>
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.Write(".");
                    Thread.Sleep(100);
                }
            });
        t.IsBackground = true; // выполнение потока в фоне
        t.Start();
        Thread.Sleep(500);
    }

    private static void Ex03()
    {
        var t1 = new Thread(ThreadProc);
        t1.Priority = ThreadPriority.Lowest;
        t1.Name = "t1";
        
        var t2 = new Thread(ThreadProc);
        t2.Priority = ThreadPriority.Highest;
        t2.Name = "t2";
        
        t1.Start();
        t2.Start();
        
        Thread.Sleep(10_000);
        isActive = false;
        
        // Thread t2 with priority Highest made 16166460292 iterations
        // Thread t1 with priority Lowest made 4759746341 iterations
    }

    static void ThreadProc()
    {
        long c = 0;
        while (isActive)
        {
            c++;
        }

        var t = Thread.CurrentThread;
        Console.WriteLine($"Thread {t.Name} with priority {t.Priority} made {c} iterations");
    }
}