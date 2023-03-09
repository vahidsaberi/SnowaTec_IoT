using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SnowaTec.Test.Domain.Common;
using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Settings;
using SnowaTec.Test.Persistence.Identity;
using SnowaTec.Test.Service.Contract.Possibility;
using SnowaTec.Test.Service.Contract.Security;
using SnowaTec.Test.Service.Implementation.Possibility;
using SnowaTec.Test.Service.Implementation.Security;
using System.Reflection;
using System.Text;

namespace SnowaTec.Test.Service
{
    public static class DependencyInjection
    {
        public static void AddServiceLayer(this IServiceCollection services)
        {
            // or you can use assembly in Extension method in Infra layer with below command
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            services.AddTransient<IEmailService, MailService>();
            services.AddTransient<ISmsService, SmsService>();
        }

        public static void AddIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseInMemoryDatabase("IdentityDb")
                );
            }
            else
            {
                services.AddDbContext<IdentityContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
                });
            }
            //services.AddIdentity<ApplicationUser, ApplicationRole>(option => option.SignIn.RequireConfirmedPhoneNumber = true).AddEntityFrameworkStores<IdentityContext>()
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.AllowedForNewUsers = false;
            });

            #region Services
            services.AddTransient<IUserService, UserService>();
            #endregion

            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        //ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = 500;
                            c.Response.ContentType = "text/plain";
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("You are not Authorized"));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = 403;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("You are not authorized to access this resource"));
                            return context.Response.WriteAsync(result);
                        },
                    };
                    //o.Events = new JwtBearerEvents
                    //{
                    //    OnAuthenticationFailed = ctx =>
                    //    {
                    //        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //        ctx.Response.ContentType = "application/json";
                    //        var result = JsonConvert.SerializeObject(new Response<string>($"From OnAuthenticationFailed:\n{ctx.Exception}"));
                    //        return ctx.Response.WriteAsync(result);
                    //    },

                    //    OnChallenge = ctx =>
                    //    {
                    //        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //        ctx.Response.ContentType = "application/json";
                    //        var result = JsonConvert.SerializeObject(new Response<string>($"From OnChallenge:\n"));
                    //        return ctx.Response.WriteAsync(result);
                    //    },

                    //    OnMessageReceived = ctx =>
                    //    {
                    //        ctx.Request.Headers.TryGetValue("Authorization", out var BearerToken);
                    //        if (BearerToken.Count == 0)
                    //            BearerToken = "no Bearer token sent\n";

                    //        ctx.Response.ContentType = "application/json";
                    //        var result = JsonConvert.SerializeObject(new Response<string>($"From OnMessageReceived:\nAuthorization Header sent: {BearerToken}\n"));
                    //        return ctx.Response.WriteAsync(result);
                    //    },

                    //    OnTokenValidated = ctx =>
                    //    {
                    //        ctx.Response.ContentType = "application/json";
                    //        var result = JsonConvert.SerializeObject(new Response<string>($"token: {ctx.SecurityToken.ToString()}"));
                    //        return ctx.Response.WriteAsync(result);
                    //    }
                    //};
                });
        }
    }
}
