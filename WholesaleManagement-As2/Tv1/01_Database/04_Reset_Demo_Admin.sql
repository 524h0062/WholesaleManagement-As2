USE WholesaleManagementDB;
GO
UPDATE dbo.Users
SET [Lock]=0, FailedLoginCount=0
WHERE UserName=N'admin01' AND [Role]=N'Admin';
SELECT UserName,[Role],[Lock],FailedLoginCount FROM dbo.Users WHERE UserName=N'admin01';
GO
