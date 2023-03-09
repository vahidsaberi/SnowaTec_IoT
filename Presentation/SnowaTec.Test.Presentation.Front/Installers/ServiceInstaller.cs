using Blazored.Toast;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SnowaTec.Test.Presentation.Front.Contract;
using SnowaTec.Test.Presentation.Front.Interfaces;
using SnowaTec.Test.Presentation.Front.Models;
using SnowaTec.Test.Presentation.Front.Services;
using SnowaTec.Test.RealTime;

namespace SnowaTec.Test.Presentation.Front.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebAssemblyHostEnvironment env)
        {
            //services.AddHttpContextAccessor();

            services.Configure<APIServer>(options => configuration.Bind(nameof(APIServer), options));

            services.AddBlazoredToast();

            #region HTTP Client
            //services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(env.BaseAddress) });

            services.AddHttpClient("api", (serviceProvider, client) =>
            {
                client.BaseAddress = new Uri("https://localhost:5002/"); //new Uri(configuration["APIServer:BaseAddress"]);
                client.DefaultRequestHeaders.Add("origin", StaticData.CONTENT_JSON);
            });
            #endregion

            #region Register interface service
            services.AddScoped<ICallApiService, CallApiService>();

            services.AddSingleton<Settings>();

            var _mqttClient = new MqttNetClient();
            services.AddSingleton(_mqttClient);

            #endregion

            //services.UseCors(options =>
            //     options.WithOrigins("https://localhost:5003", "http://localhost:5004")
            //     .AllowAnyHeader()
            //     .AllowAnyMethod());
        }
    }
}
