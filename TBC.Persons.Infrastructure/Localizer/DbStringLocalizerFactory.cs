using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace TBC.Persons.Infrastructure.Localizer;

public class DbStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMemoryCache _cache;

    public DbStringLocalizerFactory(IServiceProvider serviceProvider, IMemoryCache cache)
    {
        _serviceProvider = serviceProvider;
        _cache = cache;
    }

    public IStringLocalizer Create(Type resourceSource) =>
        new DbStringLocalizer(_serviceProvider, _cache);

    public IStringLocalizer Create(string baseName, string location) =>
        new DbStringLocalizer(_serviceProvider, _cache);
}