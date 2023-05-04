using Blazor.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOidcAuthentication(o =>
{
    // remove the default scopes provided by the NuGet package
    o.ProviderOptions.DefaultScopes.Clear();

    // read the settings from the appsettings.json file
    builder.Configuration.Bind("TrimbleIdentity4", o.ProviderOptions);
});

await builder.Build().RunAsync();
