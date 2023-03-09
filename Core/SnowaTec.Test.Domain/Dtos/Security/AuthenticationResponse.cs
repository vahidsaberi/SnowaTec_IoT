using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SnowaTec.Test.Domain.Enum;

namespace SnowaTec.Test.Domain.DTO.Security
{
    public class AuthenticationResponse
    {
        public AuthenticationResponse()
        {
            Roles = new List<RoleDto>();
        }

        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Prefix { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public EducationType EducationType { get; set; }
        [NotMapped]
        public string EducationTypeValue { get { return EducationTypeFunction.GetValue(EducationType); } }
        public string EmergencyPhone { get; set; }
        public string Address { get; set; }
        public bool IsVerified { get; set; }
        public string JWToken { get; set; }
        //[JsonIgnore]
        public string RefreshToken { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
}
