using Microsoft.EntityFrameworkCore;
using DataAccess;
using Domain.Services.Repositories;
using Shared.Shared.Interfaces;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddFastEndpoints()
    .AddSwaggerDocument();

builder.Services.AddDbContext<ShopContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("UsersDb");
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app
    .UseFastEndpoints()
    .UseSwaggerGen();

app.Run();
