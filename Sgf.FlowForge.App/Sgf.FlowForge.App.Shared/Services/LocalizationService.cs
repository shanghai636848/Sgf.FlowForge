// Services/LocalizationService.cs
using System.Globalization;
using System.Text.Json;

namespace Sgf.FlowForge.App.Shared.Services;

public class LocalizationService
{
    private readonly HttpClient _httpClient;
    private Dictionary<string, string> _localizedStrings = new();
    private string _currentCulture = "zh-CN";

    public event Action? LanguageChanged;

    public LocalizationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task InitializeAsync(string culture = "zh-CN")
    {
        await SetLanguageAsync(culture);
    }

    public async Task SetLanguageAsync(string culture)
    {
        var culturePath = culture switch
        {
            "zh-CN" => "i18n/zh-CN.json",
            "en-US" => "i18n/en-US.json",
            "ja-JP" => "i18n/ja-JP.json",
            _ => "i18n/zh-CN.json"
        };

        try
        {
            var json = await _httpClient.GetStringAsync(culturePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            _localizedStrings = FlattenJson(data);
            _currentCulture = culture;

            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);

            // 保存到 localStorage
            await SaveLanguagePreferenceAsync(culture);

            LanguageChanged?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load language file: {ex.Message}");
        }
    }

    public string GetString(string key, params object[] args)
    {
        if (_localizedStrings.TryGetValue(key, out var value))
        {
            if (args.Length > 0)
            {
                return string.Format(value, args);
            }
            return value;
        }

        return key; // 返回 key 本身作为后备
    }

    public string this[string key] => GetString(key);

    public string CurrentCulture => _currentCulture;

    public List<LanguageInfo> GetAvailableLanguages() => new()
    {
        new LanguageInfo { Code = "zh-CN", Name = "简体中文", NativeName = "简体中文" },
        new LanguageInfo { Code = "en-US", Name = "English", NativeName = "English" },
        new LanguageInfo { Code = "ja-JP", Name = "Japanese", NativeName = "日本語" }
    };

    private Dictionary<string, string> FlattenJson(
        Dictionary<string, object> data,
        string prefix = "")
    {
        var result = new Dictionary<string, string>();

        foreach (var kvp in data)
        {
            var key = string.IsNullOrEmpty(prefix) ? kvp.Key : $"{prefix}.{kvp.Key}";

            if (kvp.Value is JsonElement element)
            {
                if (element.ValueKind == JsonValueKind.Object)
                {
                    var nested = JsonSerializer.Deserialize<Dictionary<string, object>>(
                        element.GetRawText());
                    if (nested != null)
                    {
                        var flattened = FlattenJson(nested, key);
                        foreach (var item in flattened)
                        {
                            result[item.Key] = item.Value;
                        }
                    }
                }
                else if (element.ValueKind == JsonValueKind.String)
                {
                    result[key] = element.GetString() ?? string.Empty;
                }
            }
        }

        return result;
    }

    private async Task SaveLanguagePreferenceAsync(string culture)
    {
        // 使用 localStorage 保存语言偏好
        // 这里需要 JS Interop
    }
}

public class LanguageInfo
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NativeName { get; set; } = string.Empty;
}