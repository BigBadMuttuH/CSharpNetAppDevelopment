using System.Data.SQLite;

namespace DataBases;

public class ADONetConnection
{
    public static void Ex01()
    {
        var databaseName = @"F:\CSharp\GB\CSharpNetAppDevelopment\005_DataBases\01_Lecture\DataBases\db\db_01";
        var connectionString = $"Data Source={databaseName};Version=3";

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            var query = $"SELECT users.id, users.name, messages.message" +
                        $" FROM users" +
                        $" JOIN messages ON users.id = messages.user_id";
            using (var command = new SQLiteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var userID = reader.GetInt32(0);
                        var userName = reader.GetString(1);
                        var message = reader.GetString(2);

                        Console.WriteLine($"User ID:{userID}, User Name:{userName}, message:{message}");
                    }
                }
            }
        }
    }
}