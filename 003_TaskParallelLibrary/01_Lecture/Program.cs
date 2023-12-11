namespace _01_Lecture;

internal static class Program
{
    private static readonly object lockObj = new();

    private static readonly CancellationTokenSource cts = new();
    private static readonly CancellationToken ct = cts.Token;


    private static int GetValue1()
    {
        Thread.Sleep(1_000);
        return 1;
    }

    private static int GetValue2()
    {
        Thread.Sleep(2_000);
        return 2;
    }

    private static void Main(string[] args)
    {
        // Ex01();
        // Ex02();
        // TReturnVal1();
        // Ex03();
        // Ex04();
        // Ex05();
        // Ex06();
        Ex07();
    }

    private static void Print()
    {
        lock (lockObj)
        {
            for (var i = 0; i < 10; i++) Console.Write($"{Thread.CurrentThread.GetHashCode()}:{i}; ");
        }
    }

    private static void Ex01()
    {
        for (var i = 0; i < 10; i++)
            // Task тоже использует механизм потоков
            // вариант создания класса и запуска задачи
            // var t = new Task(Print);
            // t.Start();
            // проще
            Task.Run(Print);

        // так как до этой точки дойдем раньше чтем потоки закроются.
        Console.ReadKey(); // Но это не самый удачный способ дождаться выполнения задачи
    }

    private static void Ex02()
    {
        var tasks = new List<Task>();
        for (var i = 0; i < 10; i++)
            tasks.Add(Task.Run(Print));

        Task.WaitAll(tasks.ToArray());
    }

    private static void TReturnVal1()
    {
        var v1 = Task.Run(GetValue1);
        var v2 = Task.Run(GetValue2);

        v1.Wait();
        v2.Wait();

        Console.WriteLine(v1.Result + v2.Result);
    }

    private static void WriteParallel1()
    {
        Console.WriteLine("Hello");
    }

    private static void Ex03()
    {
        var action = WriteParallel1;
        var t = new Task(action);
        t.Start();
        t.Wait();
    }

    // static private CancellationTokenSource cts = new CancellationTokenSource();
    // static private CancellationToken tt = cts.Token;
    // CancellationTokenSource и CancellationToken являются частью механизма отмены задач в .NET Framework,
    // который используется для управления процессом отмены асинхронных операций.
    private static void WriteParallel2()
    {
        var c = 1;
        // while (true) // так будет работать бесконечно. если не сделать ct.ThrowIF... в цикле while
        while (!ct.IsCancellationRequested)
        {
            // еще один способ прекращения задачи для while (true)
            // ct.ThrowIfCancellationRequested();

            Console.WriteLine("Привет " + c++);
            Thread.Sleep(200);
        }

        ct.ThrowIfCancellationRequested();
    }

    private static void Ex04()
    {
        var t = new Task(WriteParallel2, ct);
        t.Start();
        Thread.Sleep(1_000);
        cts.Cancel();
        // Console.WriteLine("Нажмите Enter");
        // Console.ReadKey();

        // 
        try
        {
            t.Wait();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.InnerException);
        }
    }
    // опции выполнения задач
    /*
    None:
    Это значение по умолчанию. Задача будет запланирована и выполнена с использованием стандартных параметров.

        PreferFairness:
    Указывает планировщику задач попытаться запланировать задачи таким образом, чтобы они выполнялись в порядке своего появления.

        LongRunning:
    Указывает, что задача будет выполняться длительное время. Это может привести к созданию дополнительного потока для выполнения задачи, чтобы избежать занимания потоков из пула потоков.

        AttachedToParent:
    Задача будет присоединена к родительской задаче. Это означает, что родительская задача не будет считаться завершенной, пока не завершится эта присоединенная задача.

        DenyChildAttach:
    Запрещает задачам, созданным внутри текущей задачи, автоматически присоединяться к ней.

        HideScheduler:
    Предотвращает использование текущего планировщика задач для задач, создаваемых внутри этой задачи. Это гарантирует, что вложенные задачи будут использовать стандартный планировщик задач.

        RunContinuationsAsynchronously:
    Обеспечивает, что любые продолжения задачи выполняются асинхронно.

        LazyCancellation:
    Задача будет проверять токен отмены (если он был передан) не сразу после своего запуска, а в какой-то момент во время своего выполнения.
    */

    private static void Ex05()
    {
        var t = new Task(WriteParallel2, TaskCreationOptions.LongRunning);
        t.Start();
        Thread.Sleep(1_000);
        cts.Cancel();
    }

    // Object
    private static void WriteParallel3(object s)
    {
        for (var i = 0; i < 10; i++)
        {
            Console.WriteLine(s);
            Thread.Sleep(100);
        }
    }

    private static void Ex06()
    {
        var t1 = new Task(WriteParallel3, "Flip");
        var t2 = new Task(WriteParallel3, "Flop");

        t1.Start();
        t2.Start();
        Thread.Sleep(100);
        t1.Wait();
        t2.Wait();
    }
    // Ошибка в Task будет скрыта, если мы не возвращаем результат работы Task


    /*
    Основные Особенности Task.Factory
        Настройка Поведения Задач:
    Task.Factory позволяет задавать определенные параметры для создаваемых задач, такие как планировщик задач (TaskScheduler), опции создания задач (TaskCreationOptions) и другие.

        Методы Создания Задач:
    StartNew: Это основной метод для создания и запуска новой задачи. Он позволяет указать делегат (логику задачи), а также различные опции и параметры для задачи.
        ContinueWhenAll и ContinueWhenAny: Эти методы используются для создания задач, которые будут запущены после завершения одной или всех предыдущих задач.

        Параметризация Задач:
    Можно передавать параметры, которые влияют на выполнение и планирование задач, такие как CancellationToken, опции выполнения и планировщик задач.

        Планирование Задач:
    Task.Factory тесно связан с TaskScheduler, который определяет, как и когда задачи будут выполняться. Это может быть полезно для специализированных сценариев выполнения.
*/

    //Статусы задач
    private static void WriteParallel4()
    {
        Thread.Sleep(10);
        Console.WriteLine("Задача завершилась");
    }

    /*
    Created:
    Задача была создана, но еще не запланирована для выполнения. Этот статус часто встречается, когда задача создается с помощью конструктора Task, но до вызова Start.

    WaitingForActivation:
    Задача ожидает активации. Этот статус обычно наблюдается в задачах, созданных через асинхронные методы или методы TaskCompletionSource.

    WaitingToRun:
    Задача запланирована и ожидает выполнения.

    Running:
    Задача в настоящее время выполняется.

    WaitingForChildrenToComplete:
    Задача завершилась, но ожидает завершения своих дочерних задач. Этот статус актуален для задач, которые используют опцию TaskCreationOptions.AttachedToParent.

    RanToCompletion:
    Задача успешно завершила свою работу. Это означает, что весь код в задаче был выполнен без исключений.

    Canceled:
    Задача была отменена до ее завершения, обычно через использование CancellationToken.

    Faulted:
    В процессе выполнения задачи произошло неперехваченное исключение. Этот статус указывает на то, что задача завершилась с ошибкой.
    */
    private static void Ex07()
    {
        var t = Task.Run(WriteParallel4);
        while (t.Status != TaskStatus.RanToCompletion) Console.WriteLine(t.Status);

        Console.WriteLine(t.Status);
    }
    /*
    #Основные Методы Task

        Start:
    Запускает Task, который был создан с помощью конструктора Task.

        Wait:
    Блокирует вызывающий поток до завершения выполнения задачи.
    Имеются перегрузки, позволяющие указать таймаут или CancellationToken.

        ContinueWith:
    Создает продолжение, которое выполняется, когда задача завершается.
    Позволяет определить действие, которое должно быть выполнено после завершения исходной задачи.

        RunSynchronously:
    Пытается запустить Task синхронно в текущем потоке.

        Dispose:
    Освобождает все ресурсы, используемые задачей.

        FromResult:
    Создает успешно завершенную задачу с указанным результатом (для Task<TResult>).

        Delay:
    Создает задачу, которая завершается после указанного временного промежутка.


    #Основные Свойства Task

        Status:
    Возвращает текущий TaskStatus задачи(например, Running, Completed, Faulted).

        IsCompleted:
    Указывает, завершилась ли задача.

        IsCanceled:
    Указывает, была ли задача отменена.

        IsFaulted:
    Указывает, завершилась ли задача из-за необработанного исключения.

        Exception:
    Возвращает исключение, вызвавшее ошибку в задаче, если таковое имеется.

        Id:
    Возвращает уникальный идентификатор задачи.

        CurrentId (статическое свойство):
    Возвращает идентификатор текущей выполняемой задачи.

    #Статические Методы Task

        Run:
    Запускает новую задачу.
    
        WhenAll:
    Возвращает задачу, которая завершается после выполнения всех переданных задач.

        WhenAny:
    Возвращает задачу, которая завершается, когда завершается любая из переданных задач.

        CompletedTask:
    Возвращает уже завершенную задачу.
    */
}