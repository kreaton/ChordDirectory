using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ChordDirectory;
using ChordDirectory.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// One audio engine (and one set of instrument/volume settings) for the whole app.
builder.Services.AddScoped<AudioService>();

await builder.Build().RunAsync();
