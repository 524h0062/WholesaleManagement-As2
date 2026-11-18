using System;
using System.Data;
using System.Data.SqlClient;
using AdminBackOffice.Models;

namespace AdminBackOffice.Data
{
    public static class ItemRepository
    {
        public static DataTable Search(string keyword)
        {
            return Database.Query("SELECT ItemID, ItemName, Size, Price, StockQuantity, RowVersion FROM dbo.Item WHERE ItemName LIKE @keyword OR Size LIKE @keyword ORDER BY ItemID",
                Database.Param("@keyword", "%" + (keyword ?? "").Trim() + "%"));
        }

        public static void Save(ItemRecord item)
        {
            if (string.IsNullOrWhiteSpace(item.ItemName) || string.IsNullOrWhiteSpace(item.Size))
                throw new ArgumentException("Item name and size are required.");
            if (item.Price < 0 || item.StockQuantity < 0)
                throw new ArgumentException("Price and stock cannot be negative.");

            if (item.ItemID == 0)
            {
                Database.Execute("INSERT dbo.Item(ItemName,Size,Price,StockQuantity) VALUES (@name,@size,@price,@stock)",
                    Database.Param("@name", item.ItemName.Trim()), Database.Param("@size", item.Size.Trim()),
                    Database.Param("@price", item.Price), Database.Param("@stock", item.StockQuantity));
                return;
            }

            var rows = Database.Execute("UPDATE dbo.Item SET ItemName=@name,Size=@size,Price=@price,StockQuantity=@stock WHERE ItemID=@id AND RowVersion=@version",
                Database.Param("@name", item.ItemName.Trim()), Database.Param("@size", item.Size.Trim()),
                Database.Param("@price", item.Price), Database.Param("@stock", item.StockQuantity),
                Database.Param("@id", item.ItemID), Database.Param("@version", item.RowVersion));
            if (rows != 1) throw new DBConcurrencyException("Item was updated by another user. Refresh and try again.");
        }

        public static void Delete(int itemId, byte[] rowVersion)
        {
            var rows = Database.Execute("DELETE dbo.Item WHERE ItemID=@id AND RowVersion=@version",
                Database.Param("@id", itemId), Database.Param("@version", rowVersion));
            if (rows == 0) throw new DBConcurrencyException("Item changed or was removed. Refresh the list.");
        }
    }
}
