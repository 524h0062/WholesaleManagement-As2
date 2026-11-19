using System;
using System.Data;

namespace AdminBackOffice.Data
{
    public static class DashboardRepository
    {
        public static DataTable BestSelling(DateTime start, DateTime end)
        {
            return Database.Query(@"SELECT TOP (10) i.ItemID,i.ItemName,
                SUM(d.Quantity) AS UnitsSold,
                CAST(SUM(d.Quantity*d.UnitAmount) AS DECIMAL(18,2)) AS RevenueVnd
                FROM dbo.Item i JOIN dbo.OrderDetail d ON d.ItemID=i.ItemID
                JOIN dbo.[Order] o ON o.OrderID=d.OrderID
                WHERE o.OrderStatus=N'Shipped' AND o.OrderDate>=@start AND o.OrderDate<@end
                GROUP BY i.ItemID,i.ItemName ORDER BY UnitsSold DESC,i.ItemName",
                Database.Param("@start", start), Database.Param("@end", end));
        }

        public static DataTable LowStock(int threshold)
        {
            return Database.Query("SELECT ItemID,ItemName,Size,StockQuantity FROM dbo.Item WHERE StockQuantity<=@limit ORDER BY StockQuantity,ItemName",
                Database.Param("@limit", threshold));
        }

        public static DataTable RevenueByAgent(DateTime start, DateTime end, int agentId)
        {
            return Database.Query(@"SELECT a.AgentID,a.AgentName,COUNT(DISTINCT o.OrderID) AS ShippedOrders,
                CAST(SUM(d.Quantity*d.UnitAmount) AS DECIMAL(18,2)) AS RevenueVnd
                FROM dbo.Agent a JOIN dbo.[Order] o ON o.AgentID=a.AgentID
                JOIN dbo.OrderDetail d ON d.OrderID=o.OrderID
                WHERE o.OrderStatus=N'Shipped' AND o.OrderDate>=@start AND o.OrderDate<@end
                  AND (@agent=0 OR a.AgentID=@agent)
                GROUP BY a.AgentID,a.AgentName ORDER BY RevenueVnd DESC,a.AgentName",
                Database.Param("@start", start), Database.Param("@end", end), Database.Param("@agent", agentId));
        }
    }
}
