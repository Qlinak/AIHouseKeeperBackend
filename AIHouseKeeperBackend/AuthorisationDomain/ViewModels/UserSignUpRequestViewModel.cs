using System.ComponentModel.DataAnnotations;

namespace AIHouseKeeperBackend.AuthorisationDomain.ViewModels;

public class UserSignUpRequestViewModel
{
    [Required]
    public string UserName { get; set; }
    
    [Required] 
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}