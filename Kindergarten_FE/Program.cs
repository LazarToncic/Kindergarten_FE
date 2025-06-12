using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Kindergarten_FE;
using Kindergarten_FE.Common.Interfaces;
using Kindergarten_FE.Handlers;
using Kindergarten_FE.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenStorageService, TokenStorageService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IAuthStateService, AuthStateService>();

builder.Services.AddScoped<TokenRefreshHandler>();

var baseAddress = new Uri("https://localhost:44309/");

builder.Services.AddScoped(sp =>
{
    var refreshHandler = sp.GetRequiredService<TokenRefreshHandler>();
    refreshHandler.InnerHandler = new HttpClientHandler(); // ← OVO JE KLJUČNO!

    return new HttpClient(refreshHandler)
    {
        BaseAddress = baseAddress
    };
});

await builder.Build().RunAsync();