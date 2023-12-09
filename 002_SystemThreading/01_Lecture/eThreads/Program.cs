namespace eThreads;

static class Program
{
    static void PrintThread()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.GetHashCode()} - one");
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.GetHashCode()} - two");
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.GetHashCode()} - three");
        }
    }
    
    static void Main(string[] args)
    {
        for (int i = 0; i < 10; i++)
        {
            Thread t = new Thread(PrintThread);
            t.Start();
        }
    }
}

