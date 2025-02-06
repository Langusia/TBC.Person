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
await using (var ctx = app.Services.GetService<ApplicationDbContext>())
{
    ctx.Database.MigrateAsync().Wait();
    await PersonDbDataSeeder.Seed(ctx);
}

await using (var localCtx = app.Services.GetService<LocalizationDbContext>())
{
    localCtx.Database.MigrateAsync().Wait();
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