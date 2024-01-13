using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class RequireAdminAuthentication : ActionFilterAttribute
{
    private readonly Role _requiredRole;

    public RequireAdminAuthentication(Role requiredRole = Role.Admin)
    {
        _requiredRole = requiredRole;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var sessionData = context.HttpContext.GetSessionData();
        if (sessionData == null || sessionData.Role != _requiredRole)
        {
            context.Result = new UnauthorizedObjectResult(new { Message = "Unauthorized access" });
        }
    }
}