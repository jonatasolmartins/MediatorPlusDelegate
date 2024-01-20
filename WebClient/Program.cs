using MediatorLibrary;
using Microsoft.OpenApi.Models;
using WebClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped(typeof(IValidator<>), typeof(PaymentValidator<>));
builder.Services.AddMediator(typeof(Program));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Register the endpoints
var root = app.MapGroup("paymentApi");
root.MapEndpoints();

app.Run();

