USE WholesaleManagementDB;
GO
SELECT N'Item' AS TableName, COUNT(*) AS TotalRows FROM dbo.Item
UNION ALL SELECT N'Agent', COUNT(*) FROM dbo.Agent
UNION ALL SELECT N'Order', COUNT(*) FROM dbo.[Order]
UNION ALL SELECT N'OrderDetail', COUNT(*) FROM dbo.OrderDetail
UNION ALL SELECT N'Users', COUNT(*) FROM dbo.Users;
GO
SELECT TOP(15) i.ItemID,i.ItemName,i.Price,i.StockQuantity FROM dbo.Item i ORDER BY i.ItemID;
SELECT TOP(15) a.AgentID,a.AgentName,a.Phone FROM dbo.Agent a ORDER BY a.AgentID;
SELECT TOP(20) o.OrderID,o.OrderDate,a.AgentName,o.OrderStatus,
       SUM(d.Quantity*d.UnitAmount) AS OrderTotalVnd
FROM dbo.[Order] o
JOIN dbo.Agent a ON a.AgentID=o.AgentID
JOIN dbo.OrderDetail d ON d.OrderID=o.OrderID
GROUP BY o.OrderID,o.OrderDate,a.AgentName,o.OrderStatus ORDER BY o.OrderID;
GO
SELECT o.OrderID,o.AgentID,o.OrderStatus FROM dbo.[Order] o
LEFT JOIN dbo.Agent a ON a.AgentID=o.AgentID WHERE a.AgentID IS NULL;
SELECT d.ID,d.OrderID,d.ItemID FROM dbo.OrderDetail d
LEFT JOIN dbo.[Order] o ON d.OrderID=o.OrderID
LEFT JOIN dbo.Item i ON d.ItemID=i.ItemID WHERE o.OrderID IS NULL OR i.ItemID IS NULL;
SELECT UserID,UserName,Role,AgentID,[Lock],FailedLoginCount FROM dbo.Users ORDER BY UserID;
SELECT ItemID,ItemName,StockQuantity FROM dbo.Item WHERE StockQuantity<10 ORDER BY StockQuantity;
GO
