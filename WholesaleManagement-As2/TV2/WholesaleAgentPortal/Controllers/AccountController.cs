using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WholesaleAgentPortal.Services;
using WholesaleAgentPortal.ViewModels;

namespace WholesaleAgentPortal.Controllers;

public sealed class AccountController(AgentAuthenticationService auth) : Controller
{
    [HttpGet, AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Index", "Catalog");
        return View(new AccountLoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
    public async Task<IActionResult> Login(AccountLoginViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(model);
        var result = await auth.SignInAsync(model.UserName, model.Password, ct);
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            return View(model);
        }
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, result.UserId.ToString()),
            new(ClaimTypes.Name, result.UserName),
            new(ClaimTypes.Role, "Agent"),
            new("AgentId", result.AgentId.ToString())
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
            new AuthenticationProperties { IsPersistent = false, AllowRefresh = true });

        return !string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl)
            ? LocalRedirect(model.ReturnUrl)
            : RedirectToAction("Index", "Catalog");
    }

    [Authorize(Roles = "Agent"), HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    [AllowAnonymous]
    public IActionResult AccessDenied() => View();
}
