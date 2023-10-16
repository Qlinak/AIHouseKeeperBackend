namespace AIHouseKeeperBackend.AIDomain.ViewModels;

public class Respond
{
    public List<Choice> Choices { get; set; }
}

public class Choice
{
    public long Index { get; set; }

    public string FinishReason { get; set; }

    public MessageContent Message { get; set; }
}