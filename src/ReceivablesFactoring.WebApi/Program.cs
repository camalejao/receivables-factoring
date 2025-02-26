using ReceivablesFactoring.Application;
using ReceivablesFactoring.Infrastructure;
using ReceivablesFactoring.WebApi.Extensions;
using ReceivablesFactoring.WebApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();
