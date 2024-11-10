using Dima.Core.Handlers;
using Dima.Web;
using Dima.Web.Handlers;
using Dima.Web.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CookieHandler>();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
builder.Services.AddScoped(static x => (ICookieAuthenticationStateProvider)x.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddMudServices();

builder.Services.AddHttpClient(Configuration.HTTPCLIENT_NAME, static options =>
{
    options.BaseAddress = new Uri(uriString: Configuration.BackendUrl);
}).AddHttpMessageHandler<CookieHandler>();

builder.Services.AddTransient<IAccountHandler, AccountHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();

await builder.Build().RunAsync();
