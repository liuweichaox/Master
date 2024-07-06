using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Master.Infrastructure.Localization;

public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly LocalizationOptions _options;
    public JsonStringLocalizerFactory(IWebHostEnvironment hostingEnvironment, IOptions<LocalizationOptions> localizationOptions)
    {
        if (localizationOptions == null)
        {
            throw new ArgumentNullException(nameof(localizationOptions));
        }
        this._options = localizationOptions.Value;
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        return new JsonStringLocalizer(_options, resourceSource.Name, CultureInfo.CurrentUICulture);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new JsonStringLocalizer(_options, baseName, new CultureInfo(location));
    }
}