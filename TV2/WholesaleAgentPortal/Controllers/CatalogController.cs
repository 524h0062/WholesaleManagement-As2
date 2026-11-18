using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WholesaleAgentPortal.Data;
using WholesaleAgentPortal.ViewModels;

namespace WholesaleAgentPortal.Controllers;

[Authorize(Roles = "Agent")]
public sealed class CatalogController(WholesaleDbContext db) : Controller
{
    public async Task<IActionResult> Index(string? search, CancellationToken ct)
    {
        var items = db.Items.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();
            items = items.Where(x => x.ItemName.Contains(term) || x.Size.Contains(term));
        }
        var products = await items.OrderBy(x => x.ItemName)
            .Select(x => new ProductOption
            {
                ItemId = x.ItemId,
                ItemName = x.ItemName,
                Size = x.Size,
                Price = x.Price,
                StockQuantity = x.StockQuantity
            }).ToListAsync(ct);
        ViewBag.Search = search?.Trim();
        return View(products);
    }
}
