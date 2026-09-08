using Microsoft.EntityFrameworkCore;
using ComputerComponents.Application;
using ComputerComponents.Infrastructure;
using ComputerComponents.Infrastructure.Data;
using ComputerComponents.Infrastructure.Repositories;
using ComputerComponents.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using ComputerComponents.Domain.Entities;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy for Flutter Mobile & Web app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Clean Architecture: Register Application & Infrastructure Services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

var app = builder.Build();

app.UseCors("AllowAll");

// Ensure Database & Tables Creation
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    // Ensure Users table exists if DB was created in earlier step
    try
    {
        context.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
            BEGIN
                CREATE TABLE Users (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    FullName NVARCHAR(MAX) NOT NULL,
                    Email NVARCHAR(MAX) NOT NULL,
                    Password NVARCHAR(MAX) NOT NULL,
                    Role NVARCHAR(MAX) NOT NULL,
                    Status NVARCHAR(MAX) NOT NULL,
                    CreatedAt DATETIME2 NOT NULL
                );
            END
        ");
    }
    catch { }

    // Seed Categories if empty
    if (!context.Categories.Any())
    {
        var cat1 = new Category { CategoryName = "معالجات (CPUs)", Description = "معالجات Intel و AMD بأحدث الأجيال" };
        var cat2 = new Category { CategoryName = "كروت شاشة (GPUs)", Description = "بطاقات رسومية Nvidia RTX و AMD Radeon" };
        var cat3 = new Category { CategoryName = "ذاكرة عشوائية (RAM)", Description = "ذواكر DDR4 و DDR5 فائقة السرعة" };
        var cat4 = new Category { CategoryName = "أجهزة تخزين (SSD/HDD)", Description = "أقراص NVMe SSD و أجهزة التخزين السريعة" };

        context.Categories.AddRange(cat1, cat2, cat3, cat4);
        context.SaveChanges();

        // Seed Components
        context.Components.AddRange(
            new Component { ComponentName = "Intel Core i9-14900K", Model = "BX8071514900K", Price = 589.99m, ImageUrl = "https://images.unsplash.com/photo-1591799264318-7e6ef8ddb7ea?w=500", CategoryId = cat1.Id },
            new Component { ComponentName = "AMD Ryzen 9 7950X", Model = "100-100000514WOF", Price = 549.00m, ImageUrl = "https://images.unsplash.com/photo-1555680202-c86f0e12f086?w=500", CategoryId = cat1.Id },
            new Component { ComponentName = "Nvidia RTX 4090 OC", Model = "ROG-STRIX-RTX4090", Price = 1799.99m, ImageUrl = "https://images.unsplash.com/photo-1587202372775-e229f172b9d7?w=500", CategoryId = cat2.Id },
            new Component { ComponentName = "Corsair Vengeance RGB 32GB DDR5", Model = "CMH32GX5M2B6000C30", Price = 124.99m, ImageUrl = "https://images.unsplash.com/photo-1562976540-1502c2145186?w=500", CategoryId = cat3.Id },
            new Component { ComponentName = "Samsung 990 PRO 2TB NVMe SSD", Model = "MZ-V9P2T0BW", Price = 169.99m, ImageUrl = "https://images.unsplash.com/photo-1597872200969-2b65d56bd16b?w=500", CategoryId = cat4.Id }
        );
        context.SaveChanges();
    }

    // Seed Admin User if empty
    if (!context.Users.Any())
    {
        context.Users.Add(new User
        {
            FullName = "المدير العام",
            Email = "admin@system.com",
            Password = "admin",
            Role = "مدير نظام",
            Status = "نشط",
            CreatedAt = DateTime.Now
        });
        context.Users.Add(new User
        {
            FullName = "أحمد علي",
            Email = "ahmed@system.com",
            Password = "123",
            Role = "مستعمل",
            Status = "نشط",
            CreatedAt = DateTime.Now
        });
        context.SaveChanges();
    }
}

// Configure HTTP request pipeline.
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

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
