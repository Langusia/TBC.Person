using Microsoft.EntityFrameworkCore;
using Serilog;
using TBC.Persons.API.Middlewares;
using TBC.Persons.Application;
using TBC.Persons.Infrastructure;
using TBC.Persons.Infrastructure.Db.Contexts;
using TBC.Persons.Infrastructure.Localizer;
using TBC.Persons.Infrastructure.Seeder;

//////////host-builder//////////////////
var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog(
        (ctx, lc) =>
        {
            lc.WriteTo.Debug();
            lc.ReadFrom.Configuration(ctx.Configuration);
        })
    .ConfigureAppConfiguration((hostContext, builder) => { builder.AddEnvironmentVariables(); });
//////////services///////////////
builder.Services.Configure<List<SupportedLanguage>>(builder.Configuration.GetSection("SupportedLanguages"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

///////app////////////
var app = builder.Build();
/////////seeder////////////////
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    ctx.Database.MigrateAsync().Wait();
    await PersonDbDataSeeder.Seed(ctx);
}

using (var scope = app.Services.CreateScope())
{
    var localCtx = scope.ServiceProvider.GetRequiredService<LocalizationDbContext>();
    localCtx.Database.Migrate();
}

//////////pipeline/////////////
app.UseMiddleware<LocalizationMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapControllers();

app.Run();