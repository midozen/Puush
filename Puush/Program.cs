using Microsoft.EntityFrameworkCore;
using Puush.Infrastructure.Security.Middleware;
using Puush.Persistence;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySQL(connectionString));

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseMiddleware<PuushAuthMiddleware>();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
