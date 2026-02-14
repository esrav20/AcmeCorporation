using AcmeCorporation.Core.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AcmeCorporation.Web.Data;
using AcmeCorporation.Core.Interfaces;
using AcmeCorporation.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// === Services ===
builder.Services.AddControllersWithViews();

// EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity 
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

// Dependency INjection
builder.Services.AddScoped<ISerialNumberService, SerialNumberService>();
builder.Services.AddScoped<IDrawService, DrawService>();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Auth for users
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Draw}/{action=Index}/{id?}");

// Seeding serial number
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); // Adds pending migrations, if any

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