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

    Task<User?> GetUserByEmail(string email);

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
        if (await GetUserByUsername(request.Username) != null)
        {
            throw new InvalidOperationException($"user with Username {request.Username} already exists");
        }

        if (await GetUserByEmail(request.Email) != null)
        {
            throw new InvalidOperationException($"user with email {request.Email} already exists");
        }

        var user = new User
        {
            Username = request.Username,
            PasswordHash = HashHelper.HashPassword(request.Password, _config.SecretKey),
            Email = request.Email
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
        return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<User?> GetUserFromRequestModelAsync(GetTokenRequestViewModel vm)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(
            x => 
                x.Username == vm.Username
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