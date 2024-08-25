using PetFamily.API;
using PetFamily.API.Extensions;
using PetFamily.API.Middlewares;
using PetFamily.Application;
using PetFamily.Infrastructure;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Seq(
        builder.Configuration.GetConnectionString("Seq") ??
        throw new ArgumentNullException("Argument cannot be null")
    )
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .CreateLogger();

builder.Services
    .AddApi()
    .AddAInfrastructure()
    .AddApplication();

builder.Services.AddHttpLogging(u => { u.CombineLogs = true; });
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