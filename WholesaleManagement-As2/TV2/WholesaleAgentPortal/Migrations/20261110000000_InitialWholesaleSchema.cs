using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using WholesaleAgentPortal.Data;

#nullable disable

namespace WholesaleAgentPortal.Migrations;

[DbContext(typeof(WholesaleDbContext))]
[Migration("20261110000000_InitialWholesaleSchema")]
public partial class InitialWholesaleSchema : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Agent",
            columns: table => new
            {
                AgentID = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                AgentName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                Address = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Agent", x => x.AgentID);
                table.CheckConstraint("CK_Agent_Name", "LEN(LTRIM(RTRIM([AgentName]))) > 0");
            });

        migrationBuilder.CreateTable(
            name: "Item",
            columns: table => new
            {
                ItemID = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                ItemName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                Size = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                StockQuantity = table.Column<int>(type: "int", nullable: false),
                RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Item", x => x.ItemID);
                table.CheckConstraint("CK_Item_Price", "[Price] >= 0");
                table.CheckConstraint("CK_Item_Stock", "[StockQuantity] >= 0");
                table.CheckConstraint("CK_Item_Name", "LEN(LTRIM(RTRIM([ItemName]))) > 0");
            });

        migrationBuilder.CreateTable(
            name: "Order",
            columns: table => new
            {
                OrderID = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                OrderDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                AgentID = table.Column<int>(type: "int", nullable: false),
                OrderStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Order", x => x.OrderID);
                table.CheckConstraint("CK_Order_Status", "[OrderStatus] IN (N'Pending', N'Shipped')");
                table.ForeignKey("FK_Order_Agent", x => x.AgentID, "Agent", "AgentID", onDelete: ReferentialAction.NoAction);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                UserID = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                UserName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                Email = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Lock = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                Role = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                AgentID = table.Column<int>(type: "int", nullable: true),
                FailedLoginCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                LastLoginAtUtc = table.Column<DateTime>(type: "datetime2(0)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.UserID);
                table.CheckConstraint("CK_Users_Role", "[Role] IN (N'Admin', N'Agent')");
                table.CheckConstraint("CK_Users_RoleAgent", "([Role] = N'Admin' AND [AgentID] IS NULL) OR ([Role] = N'Agent' AND [AgentID] IS NOT NULL)");
                table.CheckConstraint("CK_Users_FailCount", "[FailedLoginCount] >= 0");
                table.ForeignKey("FK_Users_Agent", x => x.AgentID, "Agent", "AgentID", onDelete: ReferentialAction.NoAction);
            });

        migrationBuilder.CreateTable(
            name: "OrderDetail",
            columns: table => new
            {
                ID = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                OrderID = table.Column<int>(type: "int", nullable: false),
                ItemID = table.Column<int>(type: "int", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false),
                UnitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OrderDetail", x => x.ID);
                table.CheckConstraint("CK_Detail_Qty", "[Quantity] > 0");
                table.CheckConstraint("CK_Detail_UnitAmount", "[UnitAmount] >= 0");
                table.ForeignKey("FK_Detail_Order", x => x.OrderID, "Order", "OrderID", onDelete: ReferentialAction.NoAction);
                table.ForeignKey("FK_Detail_Item", x => x.ItemID, "Item", "ItemID", onDelete: ReferentialAction.NoAction);
                table.UniqueConstraint("UQ_Detail_Order_Item", x => new { x.OrderID, x.ItemID });
            });

        migrationBuilder.CreateIndex("IX_Order_Agent_Date", "Order", new[] { "AgentID", "OrderDate" })
            .Annotation("SqlServer:Include", new[] { "OrderStatus" });
        migrationBuilder.CreateIndex("IX_Order_Status_Date", "Order", new[] { "OrderStatus", "OrderDate" });
        migrationBuilder.CreateIndex("IX_Detail_Item", "OrderDetail", "ItemID");
        migrationBuilder.CreateIndex("UQ_Users_UserName", "Users", "UserName", unique: true);
        migrationBuilder.CreateIndex("UQ_Users_Email", "Users", "Email", unique: true);
        migrationBuilder.CreateIndex("UX_Users_AgentID_Active", "Users", "AgentID", unique: true, filter: "[AgentID] IS NOT NULL");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("OrderDetail");
        migrationBuilder.DropTable("Users");
        migrationBuilder.DropTable("Order");
        migrationBuilder.DropTable("Item");
        migrationBuilder.DropTable("Agent");
    }
}
