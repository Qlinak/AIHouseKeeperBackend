using System.ComponentModel.DataAnnotations;
using AIHouseKeeper.Models.Abstractions;

namespace AIHouseKeeper.Models.DbEntities;

public class User : IHasCreatedAt
{
    [Key]
    public long Id { get; set; }
    
    public string UserName { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }
    
    public Memory Memory { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}