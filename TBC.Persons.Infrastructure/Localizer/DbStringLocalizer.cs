using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace TBC.Persons.Infrastructure.Localizer;

public class DbStringLocalizer : IStringLocalizer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMemoryCache _cache;

    public DbStringLocalizer(IServiceProvider serviceProvider, IMemoryCache cache)
    {
        _serviceProvider = serviceProvider;
        _cache = cache;
    }

    public LocalizedString this[string name]
    {
        get
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LocalizationDbContext>();

            var culture = CultureInfo.CurrentUICulture.Name;
            var cacheKey = $"{culture}_{name}";

            if (_cache.TryGetValue(cacheKey, out string cachedValue))
            {
                return new LocalizedString(name, cachedValue);
            }

            var value = context.LocalizedStrings
                .FirstOrDefault(x => x.ResourceKey == name && x.Culture == culture)?.ResourceValue;

            if (value is null)
            {
                value = $"{culture}_{name}";
                context.LocalizedStrings.Add(new TBC.Persons.Domain.Entities.LocalizedString()
                {
                    Culture = culture,
                    ResourceKey = name,
                    ResourceValue = value
                });
                context.SaveChanges();
            }

            _cache.Set(cacheKey, value, TimeSpan.FromMinutes(30));

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
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<LocalizationDbContext>();

            cachedStrings = context.LocalizedStrings
                .Where(x => x.Culture == culture)
                .Select(x => new LocalizedString(x.ResourceKey, x.ResourceValue))
                .ToList();

            _cache.Set(cacheKey, cachedStrings, TimeSpan.FromMinutes(30));
        }

        return cachedStrings;
    }
}