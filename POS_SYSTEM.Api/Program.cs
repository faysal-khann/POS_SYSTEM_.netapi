using Microsoft.Extensions.FileProviders;
using POS_SYSTEM.Application.Interfaces;
using POS_SYSTEM.Application.Services;
using POS_SYSTEM.Domain.Interfaces;
using POS_SYSTEM.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using POS_SYSTEM.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware pipeline — order matters
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();       // serves /openapi/v1.json
    app.UseSwagger();       // serves /swagger/v1/swagger.json
    app.UseSwaggerUI();     // serves the interactive page at /swagger
}

app.UseHttpsRedirection();



app.UseAuthorization();

app.MapControllers();

app.Run();