namespace WholesaleAgentPortal.Models;

public sealed class Item
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = "";
    public string Size { get; set; } = "";
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}

public sealed class Agent
{
    public int AgentId { get; set; }
    public string AgentName { get; set; } = "";
    public string Address { get; set; } = "";
    public string Phone { get; set; } = "";
    public ICollection<WholesaleOrder> Orders { get; set; } = new List<WholesaleOrder>();
    public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
}

public sealed class WholesaleOrder
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public int AgentId { get; set; }
    public string OrderStatus { get; set; } = "Pending";
    public Agent Agent { get; set; } = null!;
    public ICollection<OrderDetail> Details { get; set; } = new List<OrderDetail>();
}

public sealed class OrderDetail
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitAmount { get; set; }
    public WholesaleOrder Order { get; set; } = null!;
    public Item Item { get; set; } = null!;
}

public sealed class AppUser
{
    public int UserId { get; set; }
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public bool IsLocked { get; set; }
    public string Role { get; set; } = "Agent";
    public int? AgentId { get; set; }
    public int FailedLoginCount { get; set; }
    public DateTime? LastLoginAtUtc { get; set; }
    public Agent? Agent { get; set; }
}
