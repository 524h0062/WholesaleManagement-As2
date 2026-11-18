using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WholesaleAgentPortal.Data;
using WholesaleAgentPortal.Models;

namespace WholesaleAgentPortal.Services;

public sealed record PlaceOrderResult(bool Success, int OrderId, string Message);

public sealed class OrderService(WholesaleDbContext db)
{
    public async Task<PlaceOrderResult> PlaceOrderAsync(int agentId, IReadOnlyList<OrderLineRequest> submitted, CancellationToken ct)
    {
        if (agentId <= 0) return Invalid("Agent account is not valid.");
        if (submitted == null || submitted.Count == 0 || submitted.Count > 20)
            return Invalid("Please select between 1 and 20 products.");

        await using var transaction = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
        try
        {
            // Read prices and inventory inside the transaction so a concurrent admin
            // edit cannot change the purchase price between validation and commit.
            var ids = submitted.Select(x => x.ItemId).Distinct().ToArray();
            var stock = await db.Items.AsNoTracking().Where(x => ids.Contains(x.ItemId))
                .Select(x => new StockInformation(x.ItemId, x.ItemName, x.StockQuantity, x.Price))
                .ToListAsync(ct);
            var validation = OrderRules.Validate(submitted, stock);
            if (!validation.IsValid)
            {
                await transaction.RollbackAsync(ct);
                return Invalid(validation.Error);
            }

            var order = new WholesaleOrder { AgentId = agentId, OrderStatus = "Pending", OrderDate = DateTime.UtcNow };
            db.Orders.Add(order);
            await db.SaveChangesAsync(ct);

            foreach (var line in validation.Lines)
            {
                var rows = await db.Database.ExecuteSqlInterpolatedAsync(
                    $"UPDATE dbo.Item SET StockQuantity=StockQuantity-{line.Quantity} WHERE ItemID={line.ItemId} AND StockQuantity>={line.Quantity}", ct);
                if (rows != 1)
                {
                    await transaction.RollbackAsync(ct);
                    db.ChangeTracker.Clear();
                    return Invalid($"Stock changed while you were placing the order. Please review {line.ItemName} and try again.");
                }
                db.OrderDetails.Add(new OrderDetail
                {
                    OrderId = order.OrderId,
                    ItemId = line.ItemId,
                    Quantity = line.Quantity,
                    UnitAmount = line.UnitPrice
                });
            }
            await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return new PlaceOrderResult(true, order.OrderId, "Order placed successfully.");
        }
        catch (DbUpdateException)
        {
            await transaction.RollbackAsync(ct);
            db.ChangeTracker.Clear();
            return Invalid("Unable to save the order. Please review the data and try again.");
        }
        catch (SqlException)
        {
            await transaction.RollbackAsync(ct);
            db.ChangeTracker.Clear();
            return Invalid("The database could not complete this order. No stock was deducted.");
        }
    }

    private static PlaceOrderResult Invalid(string message) => new(false, 0, message);
}
