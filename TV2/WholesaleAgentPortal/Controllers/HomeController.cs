using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WholesaleAgentPortal.Controllers;

public sealed class HomeController : Controller
{
    [AllowAnonymous]
    public IActionResult Error() => View();
}
