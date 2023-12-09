// TODO 21:00

namespace TDataSlot;

internal static class Program
{
    [ThreadStatic] private static int a;

    private static readonly LocalDataStoreSlot localDataStoreSlot = Thread.AllocateDataSlot();

    private static bool isActive = true;

    private static void ThreadProc1(int x)
    {
        for (var i = 0; i < 10; i++)
        {
            var data = (int?)Thread.GetData(localDataStoreSlot) ?? 0;
            Thread.SetData(localDataStoreSlot, data + x);
        }

        Console.WriteLine("Total1=" + ((int?)Thread.GetData(localDataStoreSlot) ?? 0));
    }

    private static void ThreadProc2(int x)
    {
        for (var i = 0; i < 10; i++)
        {
            var data = x;
            a += x;
        }

        Console.WriteLine("Total2=" + a);
    }

    private static void ThreadProc3(int x)
    {
        var ds = Thread.GetNamedDataSlot("Counter");
        for (var i = 0; i < 10_000; i++)
        {
            var c = (int?)Thread.GetData(ds) ?? 0;
            Thread.SetData(ds, c + x);
        }

        Console.WriteLine("Total3=" + ((int?)Thread.GetData(ds) ?? 0));
    }

    private static void ThreadProc4()
    {
        while (isActive)
        {
            Console.WriteLine(Thread.CurrentThread.Name + ":" + Thread.GetCurrentProcessorId());
            Thread.Sleep(100);
        }
    }

    private static void ThreadProc5()
    {
        for (var i = 0; i < 10; i++) Thread.Sleep(100);

        Console.WriteLine("Я завершился!");
    }

    private static void ThreadProc6(object? stateInfo)
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("This is " + i);
            Thread.Sleep(100);
        }
    }

    private static void Main(string[] args)
    {
        // Ex01();
        // Ex02();
        // Ex03();
        // Ex04();
        // Ex05();
        Ex06();
    }

    private static void Ex01()
    {
        new Thread(() => { ThreadProc1(1); }).Start();
        new Thread(() => { ThreadProc1(10); }).Start();
    }

    private static void Ex02()
    {
        new Thread(() => { ThreadProc2(1); }).Start();
        new Thread(() => { ThreadProc2(10); }).Start();

        /*
        Total1=10
        Total1=100
        Total2=10
        Total2=110 -

        Но если будет вот так
        [ThreadStatic]
        private static int a = 0;
        то вывод будет такой

        Total2=10
        Total1=100
        Total1=10
        Total2=100 - все ок.
        */
    }

    private static void Ex03()
    {
        new Thread(() => { ThreadProc3(1); }).Start();
        new Thread(() => { ThreadProc3(10); }).Start();
    }

    private static void Ex04()
    {
        var t1 = new Thread(() => { ThreadProc4(); });
        var t2 = new Thread(() => { ThreadProc4(); });

        t1.Name = "t1";
        t2.Name = "t2";

        t1.Start();
        t2.Start();

        Thread.Sleep(3_000);

        isActive = false;
    }

    public static void Ex05()
    {
        var t1 = new Thread(() => { ThreadProc5(); });
        
        // t1.UnsafeStart(); // не используй, если не знаешь зачем это делаешь.
        
        t1.Start();
        
        // t1.Join();
        // без t1.Join будет так:
        // Main завершился
        // Я завершился!
        if (t1.Join(300))
            Console.WriteLine("Дождались");
        else
            Console.WriteLine("Не дождались");

        Console.WriteLine("Main завершился");
    }

    //пул потоков
    static void Ex06()
    {
        Console.WriteLine("CompletedWC = " + ThreadPool.CompletedWorkItemCount);
        Console.WriteLine("count = " + ThreadPool.ThreadCount);
        
        ThreadPool.QueueUserWorkItem(ThreadProc6, "t1");
        ThreadPool.QueueUserWorkItem(ThreadProc6, "t2");
        ThreadPool.QueueUserWorkItem(ThreadProc6, "t3");
        Console.WriteLine("count = " + ThreadPool.ThreadCount);
        Console.WriteLine("pending count = " + ThreadPool.PendingWorkItemCount);
        
        Thread.Sleep(2_000);
        
        Console.WriteLine("count = " + ThreadPool.ThreadCount);
        Console.WriteLine("CompletedWC = " + ThreadPool.CompletedWorkItemCount);
        Console.WriteLine("pending count = " + ThreadPool.PendingWorkItemCount);
    }
}