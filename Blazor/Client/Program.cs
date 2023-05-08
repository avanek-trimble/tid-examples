using Blazor.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddHttpClient("BackendAPI", c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>(); // adds TID JWT to Authorization header

// use the BackendAPI configured HttpClient any time one is injected into a Razor page/component.
builder.Services.AddScoped(s => s.GetRequiredService<IHttpClientFactory>().CreateClient("BackendAPI"));


builder.Services.AddOidcAuthentication(o =>
{
    // remove the default scopes provided by the NuGet package
    o.ProviderOptions.DefaultScopes.Clear();

    // read the settings from the appsettings.json file
    builder.Configuration.Bind("TrimbleIdentity4", o.ProviderOptions);
});

await builder.Build().RunAsync();
