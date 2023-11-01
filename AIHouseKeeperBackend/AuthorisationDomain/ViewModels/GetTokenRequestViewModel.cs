using System.ComponentModel.DataAnnotations;

namespace AIHouseKeeperBackend.AuthorisationDomain.ViewModels;

public class GetTokenRequestViewModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}