﻿// TODO: 01:08
/*
Async
    Ключевое слово async используется для обозначения метода как асинхронного.
    Оно указывается перед типом возвращаемого значения метода.
    Метод, помеченный как async, может содержать одно или несколько выражений await.
    Возвращаемым типом асинхронного метода обычно является Task или Task<T>.
    Для асинхронных методов, которым не нужно возвращать значение, используется просто Task.
    Если метод должен возвращать значение, используется Task<T>, где T — тип возвращаемого значения.

Await
    Ключевое слово await используется для ожидания завершения асинхронной задачи.
    Когда выполнение кода достигает оператора await, текущий поток возвращается вызывающему коду,
    позволяя приложению продолжать работу, пока асинхронная задача выполняется.
    После завершения задачи выполнение кода продолжается с того места, где было использовано await.
*/

namespace _02_AsynсAwait;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        // Ex01();
        // Ex02();
        // Ex04();
        // Ex05();
        await Ex06();
    }

    private static int Do1()
    {
        Console.WriteLine("Do1 Start");
        Thread.Sleep(1_000);
        Console.WriteLine("Do1 End");
        return 42;
    }

    // static void Ex01()
    private static async Task Ex01()
    {
        var t = Task.Run(Do1);
        Console.WriteLine("t Started");
        var res = await t;
        // t.Wait();
        Console.WriteLine("t Ended");
        Console.WriteLine("xx");
        // без async не получим 42, вывод будет вот такой:
        // t Started
        // Do1 Start
        // Do1 End
        // t Ended
        // xx   
        Console.WriteLine("t result = " + res);
    }

    private static async ValueTask<int> Do2()
    {
        return 42;
    }

    /*
ValueTask полезна в сценариях, где результат асинхронной операции может быть доступен синхронно,
    и вы хотите избежать лишних выделений памяти, связанных с созданием Task.
    Это может быть особенно ценно в высоконагруженных системах и внутри библиотек, где эффективность критически важна.

Важные моменты при работе с ValueTask
    ValueTask может быть использована только один раз и не поддерживает несколько ожиданий (await).
    Если вам нужно ожидать ValueTask более одного раза, вы должны вызвать .AsTask() для преобразования её в Task.
    Всегда следите за тем, чтобы не начать новую асинхронную операцию до завершения предыдущей ValueTask.
    ValueTask и ValueTask<TResult> могут быть использованы с ключевыми словами async и await так же, как и Task и Task<TResult>.
    */
    private static async Task Ex02()
    {
        var t = Do2();
        var x = await t;
        Console.WriteLine(x);
    }

    private static async Task Ex03()
    {
        // Из ValueTask можно сделать Task
        var v = Do2();
        var task = v.AsTask();
    }

    /*
    Класс Parallel — это часть библиотеки параллельных задач (Task Parallel Library, TPL) в .NET,
    который предоставляет поддержку параллельных итераций, циклов и операций.
    Этот класс содержит статические методы, которые позволяют выполнять параллельные версии for и foreach циклов (Parallel.For и Parallel.ForEach),
    а также метод Parallel.Invoke для параллельного выполнения нескольких действий.

    Parallel.For и Parallel.ForEach
    Parallel.For и Parallel.ForEach используются для параллельного выполнения циклов.
    В отличие от традиционных циклов for и foreach, которые выполняют итерации последовательно,
    Parallel.For и Parallel.ForEach могут выполнять итерации параллельно на нескольких потоках.
    Это может привести к значительному ускорению вычислений на многопроцессорных и многоядерных системах.
    */

    // Parallel.Invoke(
    //     () => DoSomeWork("Task 1"),
    //     () => DoSomeWork("Task 2"),
    //     () => DoSomeWork("Task 3")
    // );

    // При использовании класса Parallel можно настроить различные параметры параллельного выполнения с помощью класса ParallelOptions.
    // Это может включать установку максимального количества потоков с помощью свойства MaxDegreeOfParallelism или передачу токена отмены через свойство CancellationToken.
    private static async Task Ex04()
    {
        Parallel.For(
            0,
            100,
            x =>
            {
                Thread.Sleep(10);
                Console.Write($"Thread: {Thread.CurrentThread.GetHashCode()} - {x}; ");
            }
        );
    }

    private static async Task Ex05()
    {
        var l = Enumerable.Range(0, 100).ToList();
        Parallel.ForEach(l, x => { Console.Write($"{x}; "); });
    }

    static async IAsyncEnumerable<int> AsyncNumbers(List<int> numbers)
    {
        foreach (var n in numbers)
        {
            await Task.Delay(100);
            yield return n;
        }
    }

    static async Task GetNumbers()
    {
        List<int> ints = new List<int> { 1, 2, 3, 4, 5 };
        await foreach (int n in AsyncNumbers(ints))
        {
            Console.WriteLine(n);
        }
    }

    static async Task Ex06()
    {
        var t = GetNumbers();
        Console.WriteLine("Main");
        await t;
    }
    
    /*
    Как Работает PLINQ
        Автоматическое распределение задач:
        PLINQ автоматически распределяет элементы коллекции по разным потокам, оптимизируя использование доступных процессорных ресурсов.
        
        Прозрачность использования:
        PLINQ позволяет преобразовывать обычные LINQ-запросы в параллельные простой заменой методов расширения на их параллельные аналоги (например, использованием метода .AsParallel()).
        Обработка исключений:
        PLINQ включает механизмы для эффективной обработки исключений, возникающих во время параллельного выполнения.
        Управление степенью параллелизма:
        PLINQ позволяет настраивать количество потоков, используемых для обработки запроса.
        
        Области Применения
        Обработка больших наборов данных:
        PLINQ идеально подходит для сценариев, где необходимо выполнить операции фильтрации, агрегации, сортировки или другие вычисления на больших наборах данных.
        Высокопроизводительные вычисления:
        Приложения, требующие высокой производительности, могут использовать PLINQ для распараллеливания задач и ускорения обработки.
        Многопоточные приложения:
        PLINQ облегчает написание многопоточного кода, автоматически распределяя работу по потокам и обеспечивая балансировку нагрузки.
*/
    
}


/*
ParallelLoopState
    ParallelLoopState предоставляет возможности для взаимодействия с текущим состоянием выполнения параллельного цикла.
    Этот тип используется внутри тела цикла Parallel.For или Parallel.ForEach для следующих целей:

Break:
    Метод Break может быть вызван для указания, что цикл должен прекратить выполнение после текущей итерации.
    Он не останавливает выполнение итераций, которые уже начались, но предотвращает запуск новых итераций с индексом, большим, чем текущий.
Stop:
    Метод Stop используется для немедленного прекращения выполнения цикла. Это приводит к тому, что цикл перестает запускать новые итерации, но уже начатые итерации могут завершиться.
IsStopped:
    Свойство IsStopped возвращает true, если был вызван Stop.
LowestBreakIteration:
    Свойство LowestBreakIteration возвращает наименьший индекс итерации, для которой был вызван Break, или null, если Break не вызывался.
    
ParallelLoopResult
    После завершения выполнения методов Parallel.For или Parallel.ForEach возвращается экземпляр ParallelLoopResult.
    Этот тип содержит информацию о выполнении цикла:
IsCompleted:
    Свойство IsCompleted возвращает true, если цикл завершился нормально, то есть все итерации были запущены и выполнены до конца.
LowestBreakIteration:
    Свойство LowestBreakIteration имеет тот же смысл, что и в ParallelLoopState, но доступно после завершения цикла и содержит индекс самой ранней итерации, для которой был вызван Break.    
*/

// Интерфейс IAsyncDisposable был добавлен в C# 8.0 и предназначен для асинхронного освобождения неуправляемых и других ресурсов,
// которые требуют асинхронного закрытия или очистки.
// Это особенно актуально для ресурсов, связанных с вводом-выводом (I/O),
// таких как файловые потоки, сетевые соединения или подключения к базам данных,
// где операции закрытия могут быть длительными и не должны блокировать основной поток выполнения.
internal class DisposableAsync : IDisposable, IAsyncDisposable
{
    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
