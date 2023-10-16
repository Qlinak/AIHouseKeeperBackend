namespace AIHouseKeeperBackend.AIDomain.ViewModels;

public class Message
{
    public List<MessageContent> Messages { get; set; }
}

public class MessageContent
{
    public string Role { get; set; }

    public string Content { get; set; }
}