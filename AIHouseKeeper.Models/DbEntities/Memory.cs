using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIHouseKeeper.Models.DbEntities;

public class Memory
{
    [Key]
    public long Id { get; set; }

    public List<string> InformationList { get; set; } = new();

    [ForeignKey("User")]
    public long UserId { get; set; }
}