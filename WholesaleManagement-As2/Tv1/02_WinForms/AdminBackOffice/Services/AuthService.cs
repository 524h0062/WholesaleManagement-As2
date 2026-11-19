using System;
using System.Data;
using AdminBackOffice.Data;

namespace AdminBackOffice.Services
{
    public sealed class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
    }

    public static class AuthService
    {
        public static LoginResult SignIn(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return new LoginResult { Message = "Please enter your username and password." };

            var users = Database.Query(
                "SELECT UserID, UserName, [Password], [Role], [Lock], FailedLoginCount FROM dbo.Users WHERE UserName=@name",
                Database.Param("@name", username.Trim()));

            if (users.Rows.Count == 0)
                return new LoginResult { Message = "Invalid username or password." };

            var user = users.Rows[0];
            var userId = Convert.ToInt32(user["UserID"]);
            if (Convert.ToBoolean(user["Lock"]))
                return new LoginResult { Message = "This account is locked. Contact the administrator." };

            if (!string.Equals(user["Role"].ToString(), "Admin", StringComparison.Ordinal))
                return new LoginResult { Message = "Only Admin accounts can access the desktop application." };

            if (!PasswordHasher.Verify(password, user["Password"].ToString()))
            {
                Database.Execute("UPDATE dbo.Users SET FailedLoginCount=FailedLoginCount+1, [Lock]=CASE WHEN FailedLoginCount+1>=5 THEN 1 ELSE [Lock] END WHERE UserID=@id AND [Lock]=0", Database.Param("@id", userId));
                return new LoginResult { Message = "Invalid username or password. Accounts lock after five failed attempts." };
            }

            Database.Execute("UPDATE dbo.Users SET FailedLoginCount=0, LastLoginAtUtc=SYSUTCDATETIME() WHERE UserID=@id AND [Lock]=0", Database.Param("@id", userId));
            return new LoginResult { Success = true, UserName = user["UserName"].ToString(), Message = "Login successful." };
        }
    }
}
