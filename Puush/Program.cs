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

builder.Services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseMiddleware<PuushAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
