using AcmeCorporation.Core.Data;
using AcmeCorporation.Core.Models;
using Microsoft.AspNetCore.Identity;
using SqlServerDbContextOptionsExtensions = Microsoft.EntityFrameworkCore.SqlServerDbContextOptionsExtensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DbContext
EntityFrameworkServiceCollectionExtensions.AddDbContext<DbContext>(builder.Services, options =>
    SqlServerDbContextOptionsExtensions.UseSqlServer(options, ConfigurationExtensions.GetConnectionString(builder.Configuration, "DefaultConnection")));

IdentityEntityFrameworkBuilderExtensions
    .AddEntityFrameworkStores<DbContext>(builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.User.RequireUniqueEmail = true;

    }))
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
