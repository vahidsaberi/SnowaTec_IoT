using System;

namespace SnowaTec.Test.Domain.Entities.Security
{
    public class RefreshToken
    {
        public long ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public long Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

        public string PushToken { get; set; }
        public string OSType { get; set; }
        public string OSVersion { get; set; }
        public string Explorer { get; set; }
        public string DeviceModel { get; set; }
        public string AppVersion { get; set; }
    }
}
