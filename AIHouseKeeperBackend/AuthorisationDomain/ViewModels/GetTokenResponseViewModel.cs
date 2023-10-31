using AIHouseKeeper.Models.DbEntities;

namespace AIHouseKeeperBackend.AuthorisationDomain.ViewModels;

public class GetTokenResponseViewModel
{
    public long Id { get; set; }
    
    public string UserName { get; set; }

    public string TokenType { get; set; }

    public string Token { get; set; }

    public GetTokenResponseViewModel(User user, string token)
    {
        Id = user.Id;
        UserName = user.Username;
        TokenType = "Bearer";
        Token = token;
    }
}