using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using Blazored.LocalStorage.StorageOptions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TreeBuilder.Services;

namespace TreeBuilder
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<StorageService>();
            builder.Services.AddScoped<RenderService>();
            builder.Services.AddTelerikBlazor();
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                IgnoreNullValues = true,
                IncludeFields = true
            };
            builder.Services.AddBlazoredLocalStorage(config =>
            {
                config.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                config.JsonSerializerOptions.IncludeFields = true;
                
            });
            //builder.Logging.SetMinimumLevel(LogLevel.Trace);
            await builder.Build().RunAsync();
        }
    }
}
