using AIHouseKeeperBackend.AuthorisationDomain.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AIHouseKeeperBackend.Controllers;

[ApiController]
[Authorise]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Hello()
    {
        return Ok("hello");
    }
}