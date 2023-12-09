namespace ARV;

class Program
{
    static bool started = true;
    private static AutoResetEvent autoResetEvent = new AutoResetEvent(false);

    static void ThreadProc1()
    {
        while (started)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.Write(i + " ");
                Thread.Sleep(200);
            }

            Console.WriteLine();
            autoResetEvent.Set();
            autoResetEvent.WaitOne();
        }
    }

    static void ThreadProc2()
    {
        while (started)
        {
            autoResetEvent.WaitOne();
            Console.WriteLine("2 seconds"); // см. Sleep 200
            autoResetEvent.Set();
        }
    }
    static void Ex01()
    {
        new Thread(ThreadProc1).Start();
        new Thread(ThreadProc2).Start();
        Thread.Sleep(10_000);
        started = false;
    }

    static void Main(string[] args)
    {
        Ex01();
    }
}
