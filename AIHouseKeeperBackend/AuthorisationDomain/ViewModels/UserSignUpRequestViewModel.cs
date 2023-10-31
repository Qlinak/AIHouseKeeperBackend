using System.ComponentModel.DataAnnotations;

namespace AIHouseKeeperBackend.AuthorisationDomain.ViewModels;

public class UserSignUpRequestViewModel
{
    [Required]
    public string Username { get; set; }
    
    [Required] 
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}