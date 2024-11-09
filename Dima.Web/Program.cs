using Dima.Web;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CookieHandler>();

builder.Services.AddMudServices();

builder.Services.AddHttpClient(ConfigurationHelpers.HTTPCLIENT_NAME, static options =>
{
    options.BaseAddress = new Uri(uriString: ConfigurationHelpers.BackendUrl);
}).AddHttpMessageHandler<CookieHandler>();

await builder.Build().RunAsync();
