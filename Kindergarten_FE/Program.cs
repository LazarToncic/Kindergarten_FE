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

builder.Services.AddScoped<TokenRefreshHandler>();

builder.Services.AddScoped(sp =>
{
    var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);

    return new HttpClient(sp.GetRequiredService<TokenRefreshHandler>())
    {
        BaseAddress = baseAddress
    };
});

await builder.Build().RunAsync();