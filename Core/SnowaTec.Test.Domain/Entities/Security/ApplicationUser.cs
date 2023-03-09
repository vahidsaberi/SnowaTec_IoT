using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SnowaTec.Test.Domain.Enum;

namespace SnowaTec.Test.Domain.Entities.Security
{
    public class ApplicationUser : IdentityUser<long>
    {
        public ApplicationUser()
        {
            RefreshTokens = new List<RefreshToken>();
        }

        [MaxLength(50)]
        public string Prefix { get; set; } = "";

        [MaxLength(100)]
        public string FullName { get; set; } = "";

        [NotMapped]
        public string GetFullName
        {
            get { return $"{Prefix} {FullName}"; }
        }

        [MaxLength(50)]
        public string NickName { get; set; } = "";

        public DateTime LastDateSendSMS { get; set; }

        public int CountSendSMS { get; set; }

        public string VerifyCode { get; set; } = "";

        public EducationType Education { get; set; }

        public string EmergencyPhone { get; set; }

        public string Address { get; set; }

        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }

        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
