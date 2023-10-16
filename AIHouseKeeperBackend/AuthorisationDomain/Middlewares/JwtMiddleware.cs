using AIHouseKeeperBackend.AuthorisationDomain.Services;
using Microsoft.AspNetCore.Http;

namespace AIHouseKeeperBackend.AuthorisationDomain.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserIdentityService userIdentityService, IJwtService jwtService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtService.ValidateJwtToken(token);
        if (userId != null)
        {
            // attach user to context on successful jwt validation
            context.Items["User"] = await userIdentityService.GetUserFromUserIdAsync(userId.Value);
        }

        await _next(context);
    }
}