using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SnowaTec.Test.Presentation.Front.Interfaces;

namespace SnowaTec.Test.Presentation.Front.Installers
{
    public static class InstallerExtention
    {
        public static void InstallServiceInAssembly(this IServiceCollection services, IConfiguration configuration, IWebAssemblyHostEnvironment env)
        {
            var installers = typeof(Program).Assembly.ExportedTypes.Where(x =>
                     typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();
            installers.ForEach(installer => installer.InstallServices(services, configuration, env));
        }
    }
}