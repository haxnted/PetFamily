using PetFamily.Web.Extensions;
using PetFamily.Web.Middlewares;
using PetFamily.Species.Infrastructure;
using PetFamily.Species.Presentation;
using PetFamily.VolunteerManagement.Infrastructure;
using PetFamily.VolunteerManagement.Presentation;
using PetFamily.Web;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfigureLogging();

builder.Services
    .AddWeb()
    .AddSpeciesModule(builder.Configuration)
    .AddVolunteerModule(builder.Configuration)
    .AddHttpLogging(u => { u.CombineLogs = true; });

var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.ApplyMigrations<SpeciesWriteDbContext>();
    await app.ApplyMigrations<VolunteersWriteDbContext>();
}

app.UseHttpLogging();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();