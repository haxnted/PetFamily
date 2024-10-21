using PetFamily.Accounts.Presentation;
using PetFamily.Species.Infrastructure;
using PetFamily.Species.Presentation;
using PetFamily.VolunteerManagement.Infrastructure;
using PetFamily.VolunteerManagement.Presentation;
using Serilog;
using Web;
using Web.Extensions;
using Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfigureLogging();

builder.Services
    .AddSpeciesModule(builder.Configuration)
    .AddVolunteerModule(builder.Configuration)
    .AddAccountsModule(builder.Configuration)
    .AddWeb()
    .AddHttpLogging(u =>
    {
        u.CombineLogs = true;
    });



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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();
