using Microsoft.AspNetCore.Http;

namespace SnowaTec.Test.Domain.DTO.Possibility
{
    public class PortalInfoDto : BaseEntity
    {
        public string Title { get; set; }

        public string EnglishTitle { get; set; }

        public string SiteUrl { get; set; }

        public string Slogan { get; set; }

        public string DeveloperTitle { get; set; }

        public string DeveloperEnglishTitle { get; set; }

        public string DeveloperUrl { get; set; }

        public bool RetentionOfPreviousData { get; set; }

        public byte[]? Logo { get; set; }
        public byte[]? FavIcon { get; set; }
        public byte[]? SmallLogo { get; set; }
        public byte[]? DeveloperLogo { get; set; }

        public IFormFile? LogoFile { get; set; }
        public IFormFile? FavIconFile { get; set; }
        public IFormFile? SmallLogoFile { get; set; }
        public IFormFile? DeveloperLogoFile { get; set; }
    }
}
