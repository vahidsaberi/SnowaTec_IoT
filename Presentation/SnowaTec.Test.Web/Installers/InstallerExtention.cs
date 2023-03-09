using SnowaTec.Test.Web.Interfaces.Base;

namespace SnowaTec.Test.Web.Installers
{
    public static class InstallerExtention
    {
        public static void InstallServiceInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(Program).Assembly.ExportedTypes.Where(x =>
                     typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();
            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}
