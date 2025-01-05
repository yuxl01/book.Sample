using App;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;
using System.Security.Principal;

var builder = WebApplication.CreateBuilder();
builder.Services
    .AddSingleton<IPageRenderer, PageRenderer>()
    .AddSingleton<IAccountService, AccountService>()
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddAuthorization();
var app = builder.Build();
app.UseAuthentication();
app.Map("/", WelcomeAsync);
app.MapGet("Account/Login", Login);
app.MapPost("Account/Login", SignInAsync);
app.Map("Account/Logout", SignOutAsync);
app.Map("Account/AccessDenied", DenyAccess);
app.Run();

async Task WelcomeAsync(HttpContext context, ClaimsPrincipal user, IPageRenderer renderer, IAuthorizationService authorizationService)
{
    if (user?.Identity?.IsAuthenticated ?? false)
    {
        var requirement = new RolesAuthorizationRequirement(new string[] { "admin" });
        var result = await authorizationService.AuthorizeAsync(user:user, resource: null, requirements: new IAuthorizationRequirement[] { requirement });
        if (result.Succeeded)
        {
            await renderer.RenderHomePage(user.Identity.Name!).ExecuteAsync(context);
        }
        else
        {
            await context.ForbidAsync();
        }
    }
    else
    {
      await  context.ChallengeAsync();
    }
}

IResult Login(IPageRenderer renderer) => renderer.RenderLoginPage();

Task SignInAsync(HttpContext context, HttpRequest request, IPageRenderer renderer, IAccountService accountService)
{
    var username = request.Form["username"];
    if (string.IsNullOrEmpty(username))
    { 
        return renderer.RenderLoginPage(null, null, "Please enter user name.").ExecuteAsync(context);
    }

    var password = request.Form["password"];
    if (string.IsNullOrEmpty(password))
    {
        return renderer.RenderLoginPage(username, null, "Please enter user password.").ExecuteAsync(context);
    }

    if (!accountService.Validate(username, password, out var roles))
    {
        return renderer.RenderLoginPage(username, null, "Invalid user name or password.").ExecuteAsync(context);
    }

    var identity = new GenericIdentity(name: username, type: CookieAuthenticationDefaults.AuthenticationScheme);
    foreach (var role in roles)
    {
        identity.AddClaim(new Claim(ClaimTypes.Role, role));
    }
    var user = new ClaimsPrincipal(identity);
    return context.SignInAsync(user);
}

async Task SignOutAsync(HttpContext context)
{
    await context.SignOutAsync();
    context.Response.Redirect("/");
}

IResult DenyAccess(ClaimsPrincipal user, IPageRenderer renderer) => renderer.RenderAccessDeniedPage(user?.Identity?.Name!);