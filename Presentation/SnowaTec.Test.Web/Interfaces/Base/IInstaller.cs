namespace SnowaTec.Test.Web.Interfaces.Base
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
