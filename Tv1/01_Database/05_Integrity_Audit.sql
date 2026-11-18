USE WholesaleManagementDB;
GO
SELECT N'Invalid item values' AS CheckName, COUNT(*) AS Violations
FROM dbo.Item WHERE Price < 0 OR StockQuantity < 0 OR LEN(LTRIM(RTRIM(ItemName)))=0
UNION ALL SELECT N'Invalid order statuses', COUNT(*) FROM dbo.[Order] WHERE OrderStatus NOT IN (N'Pending',N'Shipped')
UNION ALL SELECT N'Invalid detail values', COUNT(*) FROM dbo.OrderDetail WHERE Quantity<=0 OR UnitAmount<0
UNION ALL SELECT N'Orphan order agents', COUNT(*) FROM dbo.[Order] o LEFT JOIN dbo.Agent a ON a.AgentID=o.AgentID WHERE a.AgentID IS NULL
UNION ALL SELECT N'Orphan order details', COUNT(*) FROM dbo.OrderDetail d LEFT JOIN dbo.[Order] o ON o.OrderID=d.OrderID LEFT JOIN dbo.Item i ON i.ItemID=d.ItemID WHERE o.OrderID IS NULL OR i.ItemID IS NULL
UNION ALL SELECT N'Duplicate order item pairs', COUNT(*) FROM (SELECT OrderID,ItemID FROM dbo.OrderDetail GROUP BY OrderID,ItemID HAVING COUNT(*)>1) q
UNION ALL SELECT N'Invalid role/agent mapping', COUNT(*) FROM dbo.Users WHERE ([Role]=N'Admin' AND AgentID IS NOT NULL) OR ([Role]=N'Agent' AND AgentID IS NULL)
UNION ALL SELECT N'Non-hashed demo passwords', COUNT(*) FROM dbo.Users WHERE [Password] NOT LIKE N'PBKDF2$%'
UNION ALL SELECT N'Duplicate non-null AgentID', COUNT(*) FROM (SELECT AgentID FROM dbo.Users WHERE AgentID IS NOT NULL GROUP BY AgentID HAVING COUNT(*)>1) q;
GO
SELECT TOP (10) i.ItemID,i.ItemName,SUM(d.Quantity) AS UnitsSold,
CAST(SUM(d.Quantity*d.UnitAmount) AS DECIMAL(18,2)) AS RevenueVnd
FROM dbo.Item i JOIN dbo.OrderDetail d ON d.ItemID=i.ItemID
JOIN dbo.[Order] o ON o.OrderID=d.OrderID WHERE o.OrderStatus=N'Shipped'
GROUP BY i.ItemID,i.ItemName ORDER BY UnitsSold DESC;
GO
SELECT a.AgentID,a.AgentName,CAST(SUM(d.Quantity*d.UnitAmount) AS DECIMAL(18,2)) AS RevenueVnd
FROM dbo.Agent a JOIN dbo.[Order] o ON o.AgentID=a.AgentID
JOIN dbo.OrderDetail d ON d.OrderID=o.OrderID
WHERE o.OrderStatus=N'Shipped'
GROUP BY a.AgentID,a.AgentName ORDER BY RevenueVnd DESC;
GO
