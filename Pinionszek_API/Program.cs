using Microsoft.EntityFrameworkCore;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Services.DatabaseServices.BudgetService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBudgetApiService, BudgetApiService>();
builder.Services.AddDbContext<ProdDbContext>(opt => opt.UseSqlServer("name=ConnectionStrings:Default"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
