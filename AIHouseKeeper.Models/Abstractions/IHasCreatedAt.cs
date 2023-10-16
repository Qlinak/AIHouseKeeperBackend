namespace AIHouseKeeper.Models.Abstractions;

public interface IHasCreatedAt
{
    public DateTimeOffset CreatedAt { get; set; }
}