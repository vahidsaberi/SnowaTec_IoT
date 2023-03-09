using SnowaTec.Test.Presentation.Front;
using SnowaTec.Test.Presentation.Front.Installers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

WebAssemblyHostConfiguration configuration = builder.Configuration; // allows both to access and to set up the config

builder.Services.InstallServiceInAssembly(configuration, builder.HostEnvironment);

await builder.Build().RunAsync();
