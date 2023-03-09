using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace SnowaTec.Test.Presentation.Front.Interfaces
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration, IWebAssemblyHostEnvironment env);
    }
}
