using System;
using System.Data;
using AdminBackOffice.Data;

namespace AdminBackOffice.Data
{
    public static class OrderRepository
    {
        public static DataTable Search(string status, string orderId)
        {
            int id;
            var hasId = int.TryParse((orderId ?? "").Trim(), out id);
            if (!string.IsNullOrWhiteSpace(orderId) && !hasId) return new DataTable();
            return Database.Query(@"SELECT o.OrderID,o.OrderDate,a.AgentName,o.AgentID,o.OrderStatus,
                CAST(SUM(d.Quantity*d.UnitAmount) AS DECIMAL(18,2)) AS TotalVnd
                FROM dbo.[Order] o JOIN dbo.Agent a ON a.AgentID=o.AgentID
                JOIN dbo.OrderDetail d ON d.OrderID=o.OrderID
                WHERE (@status='All' OR o.OrderStatus=@status) AND (@id=0 OR o.OrderID=@id)
                GROUP BY o.OrderID,o.OrderDate,a.AgentName,o.AgentID,o.OrderStatus
                ORDER BY o.OrderDate DESC,o.OrderID DESC",
                Database.Param("@status", status), Database.Param("@id", hasId ? id : 0));
        }

        public static DataTable GetDetails(int orderId)
        {
            return Database.Query(@"SELECT d.ID,i.ItemName,i.Size,d.Quantity,d.UnitAmount,
                CAST(d.Quantity*d.UnitAmount AS DECIMAL(18,2)) AS LineTotal
                FROM dbo.OrderDetail d JOIN dbo.Item i ON i.ItemID=d.ItemID
                WHERE d.OrderID=@id ORDER BY d.ID", Database.Param("@id", orderId));
        }

        public static DataTable GetHeader(int orderId)
        {
            return Database.Query(@"SELECT o.OrderID,o.OrderDate,o.OrderStatus,a.AgentName,a.Address,a.Phone
                FROM dbo.[Order] o JOIN dbo.Agent a ON a.AgentID=o.AgentID
                WHERE o.OrderID=@id", Database.Param("@id", orderId));
        }

        public static bool MarkShipped(int orderId)
        {
            return Database.Execute("UPDATE dbo.[Order] SET OrderStatus=N'Shipped' WHERE OrderID=@id AND OrderStatus=N'Pending'",
                Database.Param("@id", orderId)) == 1;
        }
    }
}
