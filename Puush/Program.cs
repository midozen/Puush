using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Puush.Infrastructure.Security.Middleware;
using Puush.Infrastructure.Services;
using Puush.Persistence;
using Puush.Persistence.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IUsageService, UsageService>();
builder.Services.AddScoped<ICdnService, CdnService>();
builder.Services.AddScoped<IUploadService, UploadService>();

builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddRouting(o => o.LowercaseUrls = true);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseMiddleware<PuushAuthMiddleware>();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapControllers();

await app.RunAsync();
