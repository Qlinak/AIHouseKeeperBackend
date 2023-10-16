using AIHouseKeeper.Models.DbEntities;
using AIHouseKeeperBackend.AuthorisationDomain.ViewModels;
using AIHouseKeeperBackend.Configs;
using AIHouseKeeperBackend.Database;
using AIHouseKeeperBackend.DependencyInjections;
using AIHouseKeeperBackend.Helpers;
using Microsoft.EntityFrameworkCore;

namespace AIHouseKeeperBackend.AuthorisationDomain.Services;

public interface IUserIdentityService
{
    Task SignUpUser(UserSignUpRequestViewModel request);
    
    Task<GetTokenResponseViewModel?> AuthenticateUser(GetTokenRequestViewModel request);

    Task<User?> GetUserByUsername(string username);

    Task<User?> GetUserFromRequestModelAsync(GetTokenRequestViewModel vm);

    Task<User?> GetUserFromUserIdAsync(long userId);
}

public class UserIdentityService : IUserIdentityService, IScopedService
{
    private readonly AppDbContext _appDbContext;
    private readonly IAuthorisationConfig _config;
    private readonly IJwtService _jwtService;

    public UserIdentityService(
        AppDbContext appDbContext,
        IAuthorisationConfig config,
        IJwtService jwtService)
    {
        _appDbContext = appDbContext;
        _config = config;
        _jwtService = jwtService;
    }

    public async Task SignUpUser(UserSignUpRequestViewModel request)
    {
        if (await GetUserByUsername(request.UserName) != null)
        {
            throw new InvalidOperationException($"user with username {request.UserName} already exists");
        }

        var user = new User
        {
            UserName = request.UserName,
            PasswordHash = HashHelper.HashPassword(request.Password, _config.SecretKey),
        };

        await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<GetTokenResponseViewModel?> AuthenticateUser(GetTokenRequestViewModel request)
    {
        var user = await GetUserFromRequestModelAsync(request);
        if (user == null)
        {
            return null;
        }

        var token = _jwtService.GenerateJwtToken(user);
        return new GetTokenResponseViewModel(user, token);
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<User?> GetUserFromRequestModelAsync(GetTokenRequestViewModel vm)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(
            x => 
                x.UserName == vm.UserName
                && x.PasswordHash == HashHelper.HashPassword(vm.Password, _config.SecretKey));
    }

    public async Task<User?> GetUserFromUserIdAsync(long userId)
    {
        var user = await _appDbContext.Users
            .Where(x => x.Id == userId)
            .FirstOrDefaultAsync();

        return user;
    }
}