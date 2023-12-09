namespace TMutex;

internal class Program
{
    private static Mutex mtx = null;

    private static void ThreadProc()
    {
        try
        {
            mtx.WaitOne();
            for (var i = 0; i < 10; i++) Console.WriteLine($"{Thread.CurrentThread.Name}:{i}");
        }
        // Mutex всегда нужно освобождать
        finally
        {
            mtx.ReleaseMutex();
        }
    }

    private static void Main(string[] args)
    {
        // Ex01();
        Ex02();
    }

    private static void Ex01()
    {
        mtx = new Mutex(true);

        for (var i = 0; i < 10; i++)
        {
            var t = new Thread(ThreadProc);
            t.Name = "Thread " + i;
            t.Start();
        }

        Thread.Sleep(100);
        Console.WriteLine("Освобождаем");
        mtx.ReleaseMutex();
    }

    static void Ex02()
    {
        using (var mt = new Mutex(true, "Global\\MyMutex", out bool createdNew))
        {
            if (!createdNew)
            {
                Console.WriteLine("Повторный запуск, закрываем приложение");
                return;
            }
            mt.WaitOne();
            for (int i = 0; i < 100; i++)
            {
                Console.Write($"{i}");
                Thread.Sleep(100);
            }
            mt.ReleaseMutex();
            mt.ReleaseMutex();
        }
    }
}