using AIHouseKeeper.Models.DbEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AIHouseKeeperBackend.AuthorisationDomain.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthoriseAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        var user = (User?)context.HttpContext.Items["User"];
        if (user == null)
        {
            context.Result = 
                new JsonResult(new { message = "Unauthorized" }) 
                { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}