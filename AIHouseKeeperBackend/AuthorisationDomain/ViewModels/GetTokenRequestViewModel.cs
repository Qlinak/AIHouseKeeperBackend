using System.ComponentModel.DataAnnotations;

namespace AIHouseKeeperBackend.AuthorisationDomain.ViewModels;

public class GetTokenRequestViewModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}