using System.Text.Json;

namespace Server;

public class ChatMessage(string senderNickname, string messageText)
{
    public DateTime Timestamp { get; private set; } = DateTime.Now;
    public string SenderNickname { get; private set; } = senderNickname;
    public string MessageText { get; private set; } = messageText;

    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public static ChatMessage? FromJson(string json)
    {
        return JsonSerializer.Deserialize<ChatMessage>(json);
    }

    public string Print()
    {
        return $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] {SenderNickname}: {MessageText}";
    }
}