using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WholesaleAgentPortal.Data;
using WholesaleAgentPortal.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("WholesaleDB");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException(
        "Missing WholesaleDB connection string. Configure appsettings.json or user-secrets before running the Agent Portal.");

builder.Services.AddDbContext<WholesaleDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<AgentAuthenticationService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<AgentCookieEvents>();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.Name = "Wholesale.AgentPortal";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.EventsType = typeof(AgentCookieEvents);
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Catalog}/{action=Index}/{id?}");
app.Run();
