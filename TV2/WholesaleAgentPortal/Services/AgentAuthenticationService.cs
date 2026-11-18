using Microsoft.EntityFrameworkCore;
using WholesaleAgentPortal.Data;

namespace WholesaleAgentPortal.Services;

public sealed record AgentLoginResult(bool Success, int UserId, int AgentId, string UserName, string Message);

public sealed class AgentAuthenticationService(WholesaleDbContext db)
{
    public async Task<AgentLoginResult> SignInAsync(string? username, string? password, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return Failed("Enter both your username and password.");

        var user = await db.Users.AsNoTracking().SingleOrDefaultAsync(
            x => x.UserName == username.Trim(), ct);
        if (user == null) return Failed("Invalid username or password.");
        if (user.IsLocked) return Failed("This account is locked. Contact an administrator.");
        if (user.Role != "Agent" || user.AgentId is null)
            return Failed("Only registered agents can access this portal.");

        if (!SharedPasswordHasher.Verify(password, user.Password))
        {
            await db.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE dbo.Users SET FailedLoginCount=FailedLoginCount+1, [Lock]=CASE WHEN FailedLoginCount+1>=5 THEN 1 ELSE [Lock] END WHERE UserID={user.UserId} AND [Lock]=0", ct);
            return Failed("Invalid username or password. The account locks after five failed attempts.");
        }

        var updated = await db.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE dbo.Users SET FailedLoginCount=0, LastLoginAtUtc=SYSUTCDATETIME() WHERE UserID={user.UserId} AND [Lock]=0", ct);
        if (updated != 1) return Failed("This account is no longer available.");
        return new AgentLoginResult(true, user.UserId, user.AgentId.Value, user.UserName, "Signed in.");
    }

    private static AgentLoginResult Failed(string message) => new(false, 0, 0, "", message);
}
