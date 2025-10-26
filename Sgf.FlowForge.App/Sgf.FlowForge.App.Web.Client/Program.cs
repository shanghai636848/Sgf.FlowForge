using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sgf.FlowForge.App.Shared.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// ≈‰÷√ HttpClient
builder.Services.AddScoped<HttpClient>(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient
    {
        BaseAddress = new Uri(navigationManager.BaseUri)
    };
});

builder.Services.AddScoped<LocalizationService>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<ToastService>();

await builder.Build().RunAsync();
