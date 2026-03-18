using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VShop.Web.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = "/")
    {
        if (User.Identity?.IsAuthenticated == true)
            return Redirect(returnUrl ?? "/");

        var properties = new AuthenticationProperties { RedirectUri = returnUrl };
        properties.Items["prompt"] = "login";

        return Challenge(properties, "oidc");
    }

    [Authorize]
    [HttpGet]
    public IActionResult Logout()
    {
        return SignOut(
            new AuthenticationProperties { RedirectUri = Url.Action("Index", "Home") },
            CookieAuthenticationDefaults.AuthenticationScheme,
            "oidc");
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
