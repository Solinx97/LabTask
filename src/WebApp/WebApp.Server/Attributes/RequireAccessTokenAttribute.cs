using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Server.Enums;
using WebApp.Server.Interfaces;

namespace WebApp.Server.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
internal class RequireAccessTokenAttribute(IHttpClientHelper httpClientHelper) : ActionFilterAttribute
{
    private readonly IHttpClientHelper _httpClientHelper = httpClientHelper;

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.Request.Cookies.TryGetValue(nameof(AuthenticationCookie.AccessToken), out var accessToken))
        {
            context.Result = new UnauthorizedResult();

            return;
        }

        _httpClientHelper.AddAuthorizationHeader("Bearer", accessToken);

        base.OnActionExecuting(context);
    }
}
