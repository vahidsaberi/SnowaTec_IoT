using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SnowaTec.Test.Domain.Settings;
using SnowaTec.Test.Infrastructure.Mapping;
using SnowaTec.Test.Persistence;
using SnowaTec.Test.Persistence.Portal;
using SnowaTec.Test.Persistence.Recovery;
using SnowaTec.Test.Persistence.Test;
using SnowaTec.Test.RealTime;
using SnowaTec.Test.Service.Contract.Possibility;
using SnowaTec.Test.Service.Contract.Recovery;
using SnowaTec.Test.Service.Contract.Security;
using SnowaTec.Test.Service.Implementation.Possibility;
using SnowaTec.Test.Service.Implementation.Recovery;
using SnowaTec.Test.Service.Implementation.Security;

namespace SnowaTec.Test.Infrastructure.Extension
{
    public static class ConfigureServiceContainer
    {
        public static void AddDbContext(this IServiceCollection serviceCollection, IConfiguration configuration, IConfigurationRoot configRoot)
        {
            serviceCollection.AddDbContext<PortalDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("PortalConnection") ?? configRoot["ConnectionStrings:PortalConnection"]
                , b => b.MigrationsAssembly(typeof(PortalDbContext).Assembly.FullName))
            );

            serviceCollection.AddDbContext<RecoveryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("RecoveryConnection") ?? configRoot["ConnectionStrings:RecoveryConnection"]
                , b => b.MigrationsAssembly(typeof(RecoveryDbContext).Assembly.FullName))
            );

            serviceCollection.AddDbContext<TestDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TestConnection") ?? configRoot["ConnectionStrings:TestConnection"]
                    , b =>
                    {
                        b.MigrationsAssembly(typeof(TestDbContext).Assembly.FullName);
                        b.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                    }
                )
            );
        }

        public static void AddAutoMapper(this IServiceCollection serviceCollection)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new PortalMappingProfile());
                mc.AddProfile(new RecoveryMappingProfile());
                mc.AddProfile(new TestMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            serviceCollection.AddSingleton(mapper);
        }

        public static void AddScopedServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPortalDbContext>(provider => provider.GetService<PortalDbContext>());
            serviceCollection.AddScoped<IRecoveryDbContext>(provider => provider.GetService<RecoveryDbContext>());
            serviceCollection.AddScoped<ITestDbContext>(provider => provider.GetService<TestDbContext>());
        }

        public static void AddTransientServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient(typeof(IDbInitializer<>), typeof(DbInitializer<>));

            serviceCollection.AddTransient<IDateTimeService, DateTimeService>();
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<IRoleService, RoleService>();
            serviceCollection.AddTransient<IBackupService, BackupService>();
        }

        public static void AddSwaggerOpenAPI(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(setupAction =>
            {

                setupAction.SwaggerDoc(
                    "WebAPISpecification",
                    new OpenApiInfo()
                    {
                        Title = "Web API",
                        Version = "Web",
                        Description = "Through this API you can access web api details",
                        Contact = new OpenApiContact()
                        {
                            Email = "cyrus.pirates@gmail.com",
                            Name = "Cyrus",
                            Url = new Uri("https://github.com/vahidsaberi")
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });

                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = $"Input your Bearer token in this format - Bearer token to access this API",
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        }, new List<string>()
                    },
                });
            });
        }

        public static void AddMailSetting(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        }

        public static void AddSmsSetting(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<SmsSettings>(configuration.GetSection("SmsSettings"));
        }

        public static void AddController(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddControllers().AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        public static void AddVersion(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }

        public static void AddHealthCheck(this IServiceCollection serviceCollection, AppSettings appSettings, IConfiguration configuration)
        {
            serviceCollection.AddHealthChecks()
                .AddDbContextCheck<TestDbContext>(name: "Test DB Context", failureStatus: HealthStatus.Degraded)
                .AddUrlGroup(new Uri(appSettings.ApplicationDetail.ContactWebsite), name: "Test website", failureStatus: HealthStatus.Degraded)
                .AddSqlServer(configuration.GetConnectionString("TestConnection"));

            serviceCollection.AddHealthChecksUI(setupSettings: setup =>
            {
                setup.AddHealthCheckEndpoint("Basic Health Check", $"/healthz");
            }).AddInMemoryStorage();
        }

        public static void AddMQTT(this IServiceCollection serviceCollection)
        {
            var _mqttClient = new MqttNetClient();
            serviceCollection.AddSingleton(_mqttClient);
        }
    }
}
