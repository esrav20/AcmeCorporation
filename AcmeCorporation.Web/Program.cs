using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AcmeCorporation.Web.Data;
using AcmeCorporation.Core.Models;
using AcmeCorporation.Core.Interfaces;
using AcmeCorporation.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// === Services ===
builder.Services.AddControllersWithViews();

// EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity (brugere separat fra forretningsdata)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// DI: Registrér services (IoC bonus point)
// builder.Services.AddScoped<ISerialNumberService, SerialNumberService>();
// builder.Services.AddScoped<IDrawService, DrawService>();

var app = builder.Build();

// === Pipeline ===
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Draw}/{action=Index}/{id?}");

// === Seed serial numbers ===
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); // Kører evt. pending migrations

    if (!context.SerialNumbers.Any())
    {
        var numbers = AcmeCorporation.Core.Services.SerialNumberGenerator.Generate(100);
        foreach (var num in numbers)
        {
            context.SerialNumbers.Add(new SerialNumber { Number = num });
        }
        await context.SaveChangesAsync();
    }
}

app.Run();