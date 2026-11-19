using Microsoft.EntityFrameworkCore;
using WholesaleAgentPortal.Models;

namespace WholesaleAgentPortal.Migrations;

// Frozen model for the initial migration. Do not change when editing the live DbContext.
internal static class InitialSchemaModel
{
    internal static void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item", tb =>
            {
                tb.HasCheckConstraint("CK_Item_Price", "[Price] >= 0");
                tb.HasCheckConstraint("CK_Item_Stock", "[StockQuantity] >= 0");
                tb.HasCheckConstraint("CK_Item_Name", "LEN(LTRIM(RTRIM([ItemName]))) > 0");
            });
            entity.HasKey(x => x.ItemId).HasName("PK_Item");
            entity.Property(x => x.ItemId).HasColumnName("ItemID");
            entity.Property(x => x.ItemName).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Size).HasMaxLength(40).IsRequired();
            entity.Property(x => x.Price).HasColumnType("decimal(18,2)");
            entity.Property(x => x.RowVersion).IsRowVersion();
        });
        modelBuilder.Entity<Agent>(entity =>
        {
            entity.ToTable("Agent", tb => tb.HasCheckConstraint("CK_Agent_Name", "LEN(LTRIM(RTRIM([AgentName]))) > 0"));
            entity.HasKey(x => x.AgentId).HasName("PK_Agent");
            entity.Property(x => x.AgentId).HasColumnName("AgentID");
            entity.Property(x => x.AgentName).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Address).HasMaxLength(250).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(20).IsRequired();
        });
        modelBuilder.Entity<WholesaleOrder>(entity =>
        {
            entity.ToTable("Order", tb => tb.HasCheckConstraint("CK_Order_Status", "[OrderStatus] IN (N'Pending', N'Shipped')"));
            entity.HasKey(x => x.OrderId).HasName("PK_Order");
            entity.Property(x => x.OrderId).HasColumnName("OrderID");
            entity.Property(x => x.AgentId).HasColumnName("AgentID");
            entity.Property(x => x.OrderDate).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(x => x.OrderStatus).HasMaxLength(20).HasDefaultValue("Pending").IsRequired();
            entity.HasOne(x => x.Agent).WithMany(x => x.Orders).HasForeignKey(x => x.AgentId)
                .OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_Order_Agent");
            entity.HasIndex(x => new { x.AgentId, x.OrderDate }).HasDatabaseName("IX_Order_Agent_Date")
                .IncludeProperties(x => x.OrderStatus);
            entity.HasIndex(x => new { x.OrderStatus, x.OrderDate }).HasDatabaseName("IX_Order_Status_Date");
        });
        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail", tb =>
            {
                tb.HasCheckConstraint("CK_Detail_Qty", "[Quantity] > 0");
                tb.HasCheckConstraint("CK_Detail_UnitAmount", "[UnitAmount] >= 0");
            });
            entity.HasKey(x => x.Id).HasName("PK_OrderDetail");
            entity.Property(x => x.Id).HasColumnName("ID");
            entity.Property(x => x.OrderId).HasColumnName("OrderID");
            entity.Property(x => x.ItemId).HasColumnName("ItemID");
            entity.Property(x => x.UnitAmount).HasColumnType("decimal(18,2)");
            entity.HasOne(x => x.Order).WithMany(x => x.Details).HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_Detail_Order");
            entity.HasOne(x => x.Item).WithMany(x => x.OrderDetails).HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_Detail_Item");
            entity.HasAlternateKey(x => new { x.OrderId, x.ItemId }).HasName("UQ_Detail_Order_Item");
            entity.HasIndex(x => x.ItemId).HasDatabaseName("IX_Detail_Item");
        });
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("Users", tb =>
            {
                tb.HasCheckConstraint("CK_Users_Role", "[Role] IN (N'Admin', N'Agent')");
                tb.HasCheckConstraint("CK_Users_RoleAgent", "([Role] = N'Admin' AND [AgentID] IS NULL) OR ([Role] = N'Agent' AND [AgentID] IS NOT NULL)");
                tb.HasCheckConstraint("CK_Users_FailCount", "[FailedLoginCount] >= 0");
            });
            entity.HasKey(x => x.UserId).HasName("PK_Users");
            entity.Property(x => x.UserId).HasColumnName("UserID");
            entity.Property(x => x.UserName).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(160).IsRequired();
            entity.Property(x => x.Password).HasColumnName("Password").HasMaxLength(255).IsRequired();
            entity.Property(x => x.IsLocked).HasColumnName("Lock").HasDefaultValue(false);
            entity.Property(x => x.Role).HasMaxLength(12).IsRequired();
            entity.Property(x => x.AgentId).HasColumnName("AgentID");
            entity.Property(x => x.FailedLoginCount).HasDefaultValue(0);
            entity.Property(x => x.LastLoginAtUtc).HasColumnType("datetime2(0)");
            entity.HasOne(x => x.Agent).WithMany(x => x.Users).HasForeignKey(x => x.AgentId)
                .OnDelete(DeleteBehavior.NoAction).HasConstraintName("FK_Users_Agent");
            entity.HasIndex(x => x.UserName).IsUnique().HasDatabaseName("UQ_Users_UserName");
            entity.HasIndex(x => x.Email).IsUnique().HasDatabaseName("UQ_Users_Email");
            entity.HasIndex(x => x.AgentId).IsUnique().HasFilter("[AgentID] IS NOT NULL")
                .HasDatabaseName("UX_Users_AgentID_Active");
        });
    }
}
