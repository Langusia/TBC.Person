using System.Globalization;
using Microsoft.Extensions.Options;

namespace TBC.Persons.API.Middlewares
{
    public class LocalizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<SupportedLanguage> _supportedLanguages;

        public LocalizationMiddleware(RequestDelegate next, IOptions<List<SupportedLanguage>> options)
        {
            _next = next;
            _supportedLanguages = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var langs = context.Request.Headers["Accept-Language"].ToString().Split(',');

            var culture = _supportedLanguages.First(x => x.IsDefault).Culture;

            foreach (var lang in langs)
            {
                if (_supportedLanguages.Any(x => x.Culture == lang))
                {
                    culture = lang;
                    break;
                }
            }

            var cultureInfo = new CultureInfo(culture);

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            await _next(context);
        }
    }

    public class SupportedLanguage
    {
        public string Culture { get; set; }

        public bool IsDefault { get; set; }
    }
}