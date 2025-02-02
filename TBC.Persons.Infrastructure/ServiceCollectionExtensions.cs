using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using TBC.Persons.Domain.Interfaces;
using TBC.Persons.Infrastructure.Database;
using TBC.Persons.Infrastructure.Localizer;

namespace TBC.Persons.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //Infrastructure
        services.TryAddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        services.TryAddScoped<IPersonsRepository, PersonRepository>();
        services.AddDbContext<ApplicationDbContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("ConnectionStrings:DefaultConnection")));

        //Localization
        services.AddDbContext<LocalizationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionStrings:DefaultConnection")));

        services.AddSingleton<IMemoryCache, MemoryCache>();
        services.AddSingleton<IStringLocalizerFactory, DbStringLocalizerFactory>();
        services.AddLocalization();
    }
}