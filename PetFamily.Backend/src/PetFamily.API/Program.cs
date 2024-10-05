using PetFamily.API;
using PetFamily.API.Extensions;
using PetFamily.API.Middlewares;
using PetFamily.Application;
using PetFamily.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfigureLogging();

builder.Services
    .AddApi()
    .AddAInfrastructure(builder.Configuration)
    .AddApplication()
    .AddHttpLogging(u => { u.CombineLogs = true; });

var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.ApplyMigrations();
}

app.UseHttpLogging();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();