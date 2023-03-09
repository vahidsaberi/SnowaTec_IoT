using SnowaTec.Test.Domain.Entities.Possibility;

namespace SnowaTec.Test.Persistence.Portal.Seeds
{
    public static class DefaultPortalInfos
    {
        public static List<PortalInfo> PortalInfoList()
        {
            return new List<PortalInfo>()
            {
                new PortalInfo
                {
                    Id = 1,
                    Title = "Snowa",
                    EnglishTitle = "Snowa Portal",
                    SiteUrl = "www.snowa.co",
                    Slogan = "Smarter Solution",
                    DeveloperTitle = "شرکت اسنوا تک",
                    DeveloperEnglishTitle = "Snowa Company",
                    DeveloperUrl = "www.snowa.co",
                }
            };
        }
    }
}
