using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Serilog;
using SnowaTec.Test.Domain.Settings;
using SnowaTec.Test.Infrastructure.Extension;
using SnowaTec.Test.Persistence;
using SnowaTec.Test.Persistence.Identity;
using SnowaTec.Test.Persistence.Portal;
using SnowaTec.Test.Persistence.Recovery;
using SnowaTec.Test.Persistence.Test;
using SnowaTec.Test.Service;
using System.IO;

namespace SnowaTec.Test.API
{
    public class Startup
    {
        private readonly IConfigurationRoot configRoot;
        private AppSettings AppSettings { get; set; }

        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            Configuration = configuration;

            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            configRoot = builder.Build();

            AppSettings = new AppSettings();
            Configuration.Bind(AppSettings);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddController();

            services.AddIdentityService(Configuration);

            services.AddDbContext(Configuration, configRoot);

            services.AddAutoMapper();

            services.AddScopedServices();

            services.AddTransientServices();

            services.AddSwaggerOpenAPI();

            services.AddMailSetting(Configuration);

            services.AddSmsSetting(Configuration);

            services.AddServiceLayer();

            services.AddVersion();

            //services.AddHealthCheck(AppSettings, Configuration);

            services.AddFeatureManagement();

            services.AddControllersWithViews();

            services.AddRazorPages();

            services.AddMQTT();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            log.AddSerilog();

            app.ConfigureCustomExceptionMiddleware();

            app.UseCors(options =>
                 options.WithOrigins("https://localhost:5003", "http://localhost:5004")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
            );

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.ConfigureSwagger();

            //app.UseHealthChecks("/healthz", new HealthCheckOptions
            //{
            //    Predicate = _ => true,
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            //    ResultStatusCodes =
            //    {
            //        [HealthStatus.Healthy] = StatusCodes.Status200OK,
            //        [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
            //        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
            //    },
            //}).UseHealthChecksUI(setup =>
            //{
            //    setup.ApiPath = "/healthcheck";
            //    setup.UIPath = "/healthcheck-ui";
            //    //setup.AddCustomStylesheet("Customization/custom.css");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();

            #region Seed Data
            var scope = app.ApplicationServices.CreateScope();

            var identitydbInitializer = scope.ServiceProvider.GetService<IDbInitializer<IdentityContext>>();
            identitydbInitializer.Initialize();

            var portaldbInitializer = scope.ServiceProvider.GetService<IDbInitializer<PortalDbContext>>();
            portaldbInitializer.Initialize();

            var recoverydbInitializer = scope.ServiceProvider.GetService<IDbInitializer<RecoveryDbContext>>();
            recoverydbInitializer.Initialize();

            var testdbInitializer = scope.ServiceProvider.GetService<IDbInitializer<TestDbContext>>();
            testdbInitializer.Initialize();
            #endregion
        }
    }
}
