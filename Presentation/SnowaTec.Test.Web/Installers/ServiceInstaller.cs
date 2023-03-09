using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OrodGoverment.Web.Contract;
using SnowaTec.Test.Domain.Enum;
using SnowaTec.Test.Infrastructure.Mapping;
using SnowaTec.Test.RealTime;
using SnowaTec.Test.Service.Contract.Possibility;
using SnowaTec.Test.Service.Implementation.Possibility;
using SnowaTec.Test.Web.Interfaces.Base;
using SnowaTec.Test.Web.Models;
using SnowaTec.Test.Web.Services.Base;

namespace SnowaTec.Test.Web.Installers
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PortalMappingProfile());
                cfg.AddProfile(new RecoveryMappingProfile());
                cfg.AddProfile(new TestMappingProfile());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddHttpContextAccessor();

            services.Configure<APIServer>(options => configuration.Bind(nameof(APIServer), options));

            #region Enable Session
            services.AddSession(configs =>
            {
                configs.IdleTimeout = TimeSpan.FromMinutes(60);
                configs.Cookie.HttpOnly = true;  // Mitigate the risk of client side script accessing the protected cookie 
                configs.Cookie.IsEssential = true;
                configs.Cookie.SameSite = SameSiteMode.Lax;
            });
            #endregion

            #region Cookie Schema [Authorize]
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.AccessDeniedPath = "/AccessDenied"; //Redirect to AccessDenied when Unauthorize User access

                options.Cookie.Name = "Smart-Gateway-System";
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

                options.LoginPath = "/Authentication/Login";   //Force Unauthorize User to Login Page
                options.LogoutPath = "/Authentication/Logout";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });
            #endregion

            #region Policy and Claim
            services.AddAuthorization(options =>
            {
                options.AddPolicy(StaticData.Policy.SUPERADMIN_ONLY, policy => policy.RequireAuthenticatedUser().RequireRole(Roles.SuperAdmin.ToString()));
                options.AddPolicy(StaticData.Policy.ADMIN_ONLY, policy => policy.RequireAuthenticatedUser().RequireRole(Roles.Admin.ToString()));
                options.AddPolicy(StaticData.Policy.MODERATOR_ONLY, policy => policy.RequireAuthenticatedUser().RequireRole(Roles.Moderator.ToString()));
                options.AddPolicy(StaticData.Policy.BASIC_ONLY, policy => policy.RequireAuthenticatedUser().RequireRole(Roles.Basic.ToString()));
                options.AddPolicy(StaticData.Policy.AUTHENTICATED_ONLY, policy => policy.RequireAuthenticatedUser());
            });
            #endregion

            #region HTTP Client
            services.AddHttpClient("api", (serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(configuration["APIServer:BaseAddress"]);
                client.DefaultRequestHeaders.Add("origin", StaticData.CONTENT_JSON);
            });
            #endregion

            #region Razor Pages Settings
            services.AddRazorPages()
                    .AddRazorRuntimeCompilation()
                    .AddRazorPagesOptions(options =>
                    {
                        options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());

                        //Set the Folder to specific policy
                        options.Conventions.AuthorizeAreaFolder("SuperAdmin", "/", StaticData.Policy.SUPERADMIN_ONLY);
                        options.Conventions.AuthorizeAreaFolder("Admin", "/", StaticData.Policy.ADMIN_ONLY);
                        options.Conventions.AuthorizeAreaFolder("Moderator", "/", StaticData.Policy.MODERATOR_ONLY);
                        options.Conventions.AuthorizeAreaFolder("Basic", "/", StaticData.Policy.BASIC_ONLY);
                        options.Conventions.AuthorizeFolder("/Authentication/Profile", StaticData.Policy.AUTHENTICATED_ONLY);
                    })
                    .AddSessionStateTempDataProvider();  //Enable Session state based TempData storage
            #endregion

            #region Register interface service
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddScoped<ICallApiService, CallApiService>();
            services.AddScoped<IPermission, Permission>();

            var _server = configuration["MQTTServer:Server"];
            var _port = int.Parse(configuration["MQTTServer:Port"]);
            var _username = configuration["MQTTServer:Username"];
            var _password = configuration["MQTTServer:Password"];

            var _mqttClient = new MqttNetClient(_server, _port, _username, _password);               
            services.AddSingleton(_mqttClient);
            #endregion
        }
    }
}
