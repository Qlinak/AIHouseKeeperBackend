using AIHouseKeeperBackend.AIDomain.ViewModels;
using AIHouseKeeperBackend.AuthorisationDomain.Services;
using AIHouseKeeperBackend.AuthorisationDomain.ViewModels;
using AIHouseKeeperBackend.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIHouseKeeperBackend.Controllers;


[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class AuthorisationController : ControllerBase
{
    private readonly IUserIdentityService _userIdentityService;

    public AuthorisationController(IUserIdentityService userIdentityService)
    {
        _userIdentityService = userIdentityService;
    }

    [HttpPost]
    [Route("SignUp")]
    public async Task<IActionResult> SignUp(UserSignUpRequestViewModel request)
    {
        try
        {
            await _userIdentityService.SignUpUser(request);
        }
        catch (Exception e)
        {
            return BadRequest(ErrorHelper.FormatErrorMessage(nameof(SignUp), e.ToString()));
        }

        return Ok();
    }

    [HttpPost]
    [Route("SignIn")]
    public async Task<ActionResult<GetTokenResponseViewModel>> SignIn(GetTokenRequestViewModel request)
    {
        var res = await _userIdentityService.AuthenticateUser(request);

        if (res == null)
        {
            return BadRequest(ErrorHelper.FormatErrorMessage(nameof(SignIn), "Username or password incorrect"));
        }

        return Ok(res);
    }

    [HttpPost("ValidateUsername")]
    public async Task<ActionResult<ValidateUsernameResponse>> ValidateUsername([FromBody] ValidateUsernameRequest request)
    {
        var user = await _userIdentityService.GetUserByUsername(request.Username);
        var isUnique = user == null;
        var res = new ValidateUsernameResponse
        {
            IsUnique = isUnique
        };

        return Ok(res);
    }

    [HttpPost("ValidateUserEmail")]
    public async Task<ActionResult<ValidateEmailResponse>> ValidateUserEmail([FromBody] ValidateEmailRequest request)
    {
        var user = await _userIdentityService.GetUserByEmail(request.Email);
        var isUnique = user == null;
        var res = new ValidateEmailResponse
        {
            IsUnique = isUnique
        };

        return Ok(res);
    }
}