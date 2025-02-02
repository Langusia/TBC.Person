using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System.Globalization;
using TBC.Persons.Infrastructure.Localizer;

public class DbStringLocalizer : IStringLocalizer
{
    private readonly LocalizationDbContext _context;
    private readonly IMemoryCache _cache;

    public DbStringLocalizer(LocalizationDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var cacheKey = $"{culture}_{name}";

            if (_cache.TryGetValue(cacheKey, out string cachedValue))
            {
                return new LocalizedString(name, cachedValue);
            }

            var value = _context.LocalizedStrings
                .FirstOrDefault(x => x.ResourceKey == name && x.Culture == culture)?.ResourceValue ?? name;

            _cache.Set(cacheKey, value, TimeSpan.FromMinutes(30)); // Cached for 30 minutes

            return new LocalizedString(name, value);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var value = this[name].Value;
            return new LocalizedString(name, string.Format(value, arguments));
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var culture = CultureInfo.CurrentUICulture.Name;
        var cacheKey = $"AllStrings_{culture}";

        if (!_cache.TryGetValue(cacheKey, out List<LocalizedString> cachedStrings))
        {
            cachedStrings = _context.LocalizedStrings
                .Where(x => x.Culture == culture)
                .Select(x => new LocalizedString(x.ResourceKey, x.ResourceValue))
                .ToList();

            _cache.Set(cacheKey, cachedStrings, TimeSpan.FromMinutes(30));
        }

        return cachedStrings;
    }
}
