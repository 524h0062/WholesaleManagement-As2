using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WholesaleAgentPortal.Data;
using WholesaleAgentPortal.Services;
using WholesaleAgentPortal.ViewModels;

namespace WholesaleAgentPortal.Controllers;

[Authorize(Roles = "Agent")]
public sealed class OrdersController(WholesaleDbContext db, OrderService service) : Controller
{
    private int CurrentAgentId => int.TryParse(User.FindFirstValue("AgentId"), out var id) ? id : 0;

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var agentId = CurrentAgentId;
        var orders = await db.Orders.AsNoTracking()
            .Where(x => x.AgentId == agentId)
            .OrderByDescending(x => x.OrderDate).ThenByDescending(x => x.OrderId)
            .Select(x => new OrderSummaryViewModel
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                OrderStatus = x.OrderStatus,
                LineCount = x.Details.Count,
                Total = x.Details.Sum(d => d.Quantity * d.UnitAmount)
            }).ToListAsync(ct);
        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? itemId, CancellationToken ct)
    {
        var model = new CreateOrderViewModel { Lines = new() { new OrderLineInput { ItemId = itemId ?? 0, Quantity = 1 } } };
        await LoadCatalog(model, ct);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrderViewModel model, CancellationToken ct)
    {
        model.Lines ??= new();
        if (model.Lines.Count == 0 || model.Lines.Count > 20)
            ModelState.AddModelError("", "Choose between 1 and 20 order rows.");
        if (!ModelState.IsValid)
        {
            await LoadCatalog(model, ct);
            return View(model);
        }
        var request = model.Lines.Select(x => new OrderLineRequest(x.ItemId, x.Quantity)).ToList();
        var result = await service.PlaceOrderAsync(CurrentAgentId, request, ct);
        if (result.Success)
        {
            TempData["Success"] = $"Order #{result.OrderId} was placed successfully.";
            return RedirectToAction(nameof(Details), new { id = result.OrderId });
        }
        ModelState.AddModelError("", result.Message);
        await LoadCatalog(model, ct);
        return View(model);
    }

    public async Task<IActionResult> Details(int id, CancellationToken ct)
    {
        var order = await GetReceipt(id, ct);
        if (order == null) return NotFound();
        return View(order);
    }

    public async Task<IActionResult> Invoice(int id, CancellationToken ct)
    {
        var order = await GetReceipt(id, ct);
        if (order == null) return NotFound();
        return View(order);
    }

    private async Task<OrderReceiptViewModel?> GetReceipt(int id, CancellationToken ct)
    {
        var agentId = CurrentAgentId;
        return await db.Orders.AsNoTracking()
            .Where(x => x.OrderId == id && x.AgentId == agentId)
            .Select(x => new OrderReceiptViewModel
            {
                OrderId = x.OrderId,
                OrderDate = x.OrderDate,
                OrderStatus = x.OrderStatus,
                AgentName = x.Agent.AgentName,
                Address = x.Agent.Address,
                Phone = x.Agent.Phone,
                Lines = x.Details.OrderBy(d => d.Id).Select(d => new OrderReceiptLine
                {
                    ItemName = d.Item.ItemName,
                    Size = d.Item.Size,
                    Quantity = d.Quantity,
                    UnitAmount = d.UnitAmount
                }).ToList()
            }).SingleOrDefaultAsync(ct);
    }

    private async Task LoadCatalog(CreateOrderViewModel model, CancellationToken ct)
    {
        model.Catalog = await db.Items.AsNoTracking().OrderBy(x => x.ItemName)
            .Select(x => new ProductOption
            {
                ItemId = x.ItemId, ItemName = x.ItemName, Size = x.Size,
                Price = x.Price, StockQuantity = x.StockQuantity
            }).ToListAsync(ct);
    }
}
