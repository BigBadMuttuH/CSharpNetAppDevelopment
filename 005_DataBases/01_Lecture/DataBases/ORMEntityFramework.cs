using DataBases.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBases;

public class ORMEntityFramework
{
    private static string ConnectionString
    {
        get
        {
            var databaseName = @"F:\CSharp\GB\CSharpNetAppDevelopment\005_DataBases\01_Lecture\DataBases\db\db_01";
            var connectionString = $"Data Source={databaseName}";
            return connectionString;
        }
    }

    public static void Ex01()
    {
        var connectionString = ConnectionString;

        var optionsBuilder = new DbContextOptionsBuilder<OrmDbContext>()
            .UseSqlite(connectionString);

        using (var ctx = new OrmDbContext(optionsBuilder.Options))
        {
            var users = ctx.Users.ToList();
            foreach (var user in users)
            {
                Console.WriteLine($"Имя: {user.Name}");
                Console.WriteLine("\tСообщения:");
                var messages = ctx.Messages;
                foreach (var message in messages) Console.WriteLine($"\t{message.MessageContext}");
            }
        }
    }

    public static void Ex02()
    {
        var databaseName = @"F:\CSharp\GB\CSharpNetAppDevelopment\005_DataBases\01_Lecture\DataBases\db\db_01";
        var connectionString = $"Data Source={databaseName}";

        var optionsBuilder = new DbContextOptionsBuilder<OrmDbContext>()
            .UseSqlite(connectionString);

        // Создаем экземпляр OrmDbContext с использованием опций, указанных выше
        using (var context = new OrmDbContext(optionsBuilder.Options))
        {
            // Для ленивой загрузки необходимо убедиться, что LazyLoadingEnabled = true для контекста
            // Это уже установлено в конструкторе OrmDbContext

            // Получаем список всех пользователей с их сообщениями
            // Используем оператор Include, чтобы указать EF загрузить связанные сообщения
            var usersWithMessages = context.Users
                .Include(u => u.Messages)
                .ToList();

            // Перебираем полученные данные и выводим информацию
            foreach (var user in usersWithMessages)
            {
                Console.WriteLine($"User: {user.Name} (ID: {user.Id})");
                foreach (var message in user.Messages) Console.WriteLine($"\tMessage: {message.MessageContext}");
            }
        }
    }

    public static void Ex03()
    {
        var connectionString = ConnectionString;

        var optionsBuilder = new DbContextOptionsBuilder<OrmDbContext>()
            .UseSqlite(connectionString);

        using (var ctx = new OrmDbContext(optionsBuilder.Options))
        {
            var user = new User() { Name = "Николай" };
            user.Messages = new HashSet<Message>();
            user.Messages.Add(new Message {MessageContext = "Привет!"});
            user.Messages.Add(new Message {MessageContext = "Я Коля!"});
            
            ctx.Add(user);
            ctx.SaveChanges();
            int num = ctx.SaveChanges();
            Console.WriteLine("Было изменено" + num + "Строк");
        }
    }
    public static void Ex04()
    {
        var connectionString = ConnectionString;

        var optionsBuilder = new DbContextOptionsBuilder<OrmDbContext>()
            .UseSqlite(connectionString);

        using (var ctx = new OrmDbContext(optionsBuilder.Options))
        {
            var user = ctx.Users.FirstOrDefault(x => x.Name == "Коля");
            if (user != null)
            {
                user.Name = "Николай";

            }
            ctx.SaveChanges();
            int num = ctx.SaveChanges();
            Console.WriteLine("Было изменено " + num + " Строк");
        }
    }
}