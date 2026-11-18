using System.ComponentModel.DataAnnotations;

namespace WholesaleAgentPortal.ViewModels;

public sealed class ProductOption
{
    public int ItemId { get; set; }
    public string ItemName { get; set; } = "";
    public string Size { get; set; } = "";
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}

public sealed class OrderLineInput
{
    [Range(1, int.MaxValue)]
    public int ItemId { get; set; }
    [Range(1, 5000)]
    public int Quantity { get; set; } = 1;
}

public sealed class CreateOrderViewModel
{
    public List<OrderLineInput> Lines { get; set; } = new();
    public IReadOnlyList<ProductOption> Catalog { get; set; } = Array.Empty<ProductOption>();
}

public sealed class OrderSummaryViewModel
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; } = "";
    public int LineCount { get; set; }
    public decimal Total { get; set; }
}

public sealed class OrderReceiptViewModel
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public string OrderStatus { get; set; } = "";
    public string AgentName { get; set; } = "";
    public string Address { get; set; } = "";
    public string Phone { get; set; } = "";
    public IReadOnlyList<OrderReceiptLine> Lines { get; set; } = Array.Empty<OrderReceiptLine>();
    public decimal Total => Lines.Sum(x => x.Quantity * x.UnitAmount);
}

public sealed class OrderReceiptLine
{
    public string ItemName { get; set; } = "";
    public string Size { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitAmount { get; set; }
}
