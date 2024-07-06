using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Localization;

namespace Master.Infrastructure.Localization;

public class JsonStringLocalizer : IStringLocalizer
{
    private readonly ConcurrentDictionary<string, string> _all;
    private readonly LocalizationOptions _options;
    private readonly string _baseResourceName;
    public LocalizedString this[string name] => Get(name);
    public LocalizedString this[string name, params object[] arguments] => Get(name, arguments);
    public JsonStringLocalizer(LocalizationOptions options, string baseResourceName, CultureInfo culture)
    {
        _options = options;
        _baseResourceName = baseResourceName + "." + culture.Name;
        _all = GetAll();
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return _all.Select(t => new LocalizedString(t.Key, t.Value, true)).ToArray();
    }

    private LocalizedString Get(string name, params object[] arguments)
    {
        if (_all.TryGetValue(name, value: out var value))
        {
            return new LocalizedString(name, string.Format(value, arguments));
        }
        return new LocalizedString(name, name, true);
    }

    private ConcurrentDictionary<string, string> GetAll()
    {
        var file = Path.Combine(AppContext.BaseDirectory, _baseResourceName + ".json");
        if (!string.IsNullOrEmpty(_options.ResourcesPath))
            file = Path.Combine(AppContext.BaseDirectory, _options.ResourcesPath, _baseResourceName + ".json");

        Debug.WriteLineIf(!File.Exists(file), "Path not found! " + file);

        if (!File.Exists(file))
            return new ConcurrentDictionary<string, string>();

        try
        {
            var txt = File.ReadAllText(file);

            return JsonSerializer.Deserialize<ConcurrentDictionary<string, string>>(txt);
        }
        catch (Exception)
        {
        }

        return new ConcurrentDictionary<string, string>();
    }
}