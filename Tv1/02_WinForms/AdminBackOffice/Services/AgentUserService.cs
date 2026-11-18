using System;
using System.Net.Mail;
using AdminBackOffice.Data;

namespace AdminBackOffice.Services
{
    public static class AgentUserService
    {
        public static void CreateAgentLogin(int agentId, string userName, string email, string password)
        {
            if (agentId <= 0) throw new ArgumentException("Choose an agent first.");
            if (string.IsNullOrWhiteSpace(userName) || userName.Trim().Length < 4 || userName.Trim().Length > 80)
                throw new ArgumentException("Username must contain 4 to 80 characters.");
            if (string.IsNullOrWhiteSpace(email) || email.Trim().Length > 160)
                throw new ArgumentException("Enter a valid email address.");
            try
            {
                var parsed = new MailAddress(email.Trim());
                if (!string.Equals(parsed.Address, email.Trim(), StringComparison.OrdinalIgnoreCase))
                    throw new ArgumentException("Enter a valid email address.");
            }
            catch (FormatException) { throw new ArgumentException("Enter a valid email address."); }
            if (string.IsNullOrEmpty(password) || password.Length < 10)
                throw new ArgumentException("Temporary password must be at least 10 characters long.");
            if (Database.Scalar("SELECT UserID FROM dbo.Users WHERE AgentID=@agentId", Database.Param("@agentId", agentId)) != null)
                throw new InvalidOperationException("This agent already has a Web login account.");
            Database.Execute("INSERT dbo.Users(UserName,Email,[Password],[Role],AgentID,[Lock],FailedLoginCount) VALUES(@user,@email,@hash,N'Agent',@agentId,0,0)",
                Database.Param("@user", userName.Trim()), Database.Param("@email", email.Trim()),
                Database.Param("@hash", PasswordHasher.Hash(password)), Database.Param("@agentId", agentId));
        }
    }
}
