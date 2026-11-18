using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WholesaleAgentPortal.Data;

namespace WholesaleAgentPortal.Services;

public sealed class AgentCookieEvents(WholesaleDbContext db) : CookieAuthenticationEvents
{
    public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        var idText = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        var agentText = context.Principal?.FindFirstValue("AgentId");
        if (!int.TryParse(idText, out var id) || !int.TryParse(agentText, out var agentId))
        {
            await Reject(context);
            return;
        }

        var account = await db.Users.AsNoTracking().Where(x => x.UserId == id)
            .Select(x => new { x.IsLocked, x.Role, x.AgentId }).SingleOrDefaultAsync(context.HttpContext.RequestAborted);
        if (account == null || account.IsLocked || account.Role != "Agent" || account.AgentId != agentId)
            await Reject(context);
    }

    private static async Task Reject(CookieValidatePrincipalContext context)
    {
        context.RejectPrincipal();
        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
