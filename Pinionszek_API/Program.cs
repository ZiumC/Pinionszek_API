using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Pinionszek_API.DbContexts;
using Pinionszek_API.Services.DatabaseServices.BudgetService;
using Pinionszek_API.Services.DatabaseServices.PaymentService;
using Pinionszek_API.Services.DatabaseServices.UserService;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBudgetApiService, BudgetApiService>();
builder.Services.AddScoped<IUserApiService, UserApiService>();
builder.Services.AddScoped<IPaymentApiService, PaymentApiService>();
builder.Services.AddDbContext<ProdDbContext>(opt => opt.UseSqlServer("name=ConnectionStrings:Default"));

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("Budgets", new OpenApiInfo
    {
        Title = "Pinionszek API",
        Version = "v2.0",
        Description = "All endpoints for BudgetsController"
    });

    c.SwaggerDoc("Payments", new OpenApiInfo
    {
        Title = "Pinionszek API",
        Version = "v2.0",
        Description = "All endpoints for PaymentsController"
    });

    c.SwaggerDoc("User", new OpenApiInfo
    {
        Title = "Pinionszek API",
        Version = "v1.2",
        Description = "All endpoints for UserController"
    });

    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/Budgets/swagger.json", "BudgetsController");
        c.SwaggerEndpoint($"/swagger/Payments/swagger.json", "PaymentsController");
        c.SwaggerEndpoint($"/swagger/User/swagger.json", "UserController");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
