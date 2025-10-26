// Services/ThemeService.cs
using Microsoft.JSInterop;

namespace Sgf.FlowForge.App.Shared.Services;

public class ThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private string _currentTheme = "light";

    public event Action<string>? ThemeChanged;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        _currentTheme = await GetThemeAsync();
    }

    public async Task SetThemeAsync(string theme)
    {
        await _jsRuntime.InvokeVoidAsync("ThemeInterop.setTheme", theme);
        _currentTheme = theme;
        ThemeChanged?.Invoke(theme);
    }

    public async Task<string> GetThemeAsync()
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string>("ThemeInterop.getTheme");
        }
        catch
        {
            return "light";
        }
    }

    public async Task ToggleThemeAsync()
    {
        await _jsRuntime.InvokeVoidAsync("ThemeInterop.toggleTheme");
        _currentTheme = await GetThemeAsync();
        ThemeChanged?.Invoke(_currentTheme);
    }

    public string CurrentTheme => _currentTheme;
}