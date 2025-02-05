using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace TBC.Persons.Infrastructure.Localizer;

public class DbStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly LocalizationDbContext _context;
    private readonly IMemoryCache _cache;

    public DbStringLocalizerFactory(LocalizationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public IStringLocalizer Create(Type resourceSource) => new DbStringLocalizer(_context, _cache);
    public IStringLocalizer Create(string baseName, string location) => new DbStringLocalizer(_context, _cache);
}