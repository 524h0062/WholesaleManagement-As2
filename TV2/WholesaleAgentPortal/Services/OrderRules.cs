namespace WholesaleAgentPortal.Services;

public sealed record OrderLineRequest(int ItemId, int Quantity);
public sealed record StockInformation(int ItemId, string ItemName, int Available, decimal UnitPrice);
public sealed record PreparedLine(int ItemId, int Quantity, string ItemName, decimal UnitPrice, int Available)
{
    public decimal LineTotal => Quantity * UnitPrice;
}
public sealed record OrderValidation(bool IsValid, string Error, IReadOnlyList<PreparedLine> Lines)
{
    public decimal Total => Lines.Sum(x => x.LineTotal);
}

public static class OrderRules
{
    public static OrderValidation Validate(IEnumerable<OrderLineRequest>? submitted, IEnumerable<StockInformation> inventory)
    {
        var input = submitted?.ToArray() ?? Array.Empty<OrderLineRequest>();
        if (input.Length == 0 || input.Length > 20)
            return Bad("An order must contain between 1 and 20 rows.");
        if (input.Any(x => x.ItemId < 1 || x.Quantity < 1 || x.Quantity > 5000))
            return Bad("Choose an item and enter a quantity between 1 and 5,000.");

        var catalog = inventory.ToDictionary(x => x.ItemId);
        var lines = new List<PreparedLine>();
        foreach (var group in input.GroupBy(x => x.ItemId).OrderBy(x => x.Key))
        {
            var requested = group.Sum(x => (long)x.Quantity);
            if (requested > 5000) return Bad("An item cannot exceed 5,000 units per order.");
            if (!catalog.TryGetValue(group.Key, out var item))
                return Bad("An item in the order is no longer available.");
            if (requested > item.Available)
                return Bad($"Not enough stock for {item.ItemName}. Available: {item.Available}.");
            lines.Add(new PreparedLine(item.ItemId, (int)requested, item.ItemName, item.UnitPrice, item.Available));
        }
        return new OrderValidation(true, "", lines);
    }

    private static OrderValidation Bad(string text) => new(false, text, Array.Empty<PreparedLine>());
}
