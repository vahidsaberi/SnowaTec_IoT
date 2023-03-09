using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Enum;

namespace SnowaTec.Test.Persistence.Identity.Seeds
{
    public static class DefaultUser
    {
        public static List<ApplicationUser> IdentityBasicUserList()
        {
            return new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = Constants.SuperAdminUser,
                    UserName = "superadmin",
                    Email = "cyrus.pirates@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "09372300727",
                    PhoneNumberConfirmed = true,
                    Prefix = "آقای",
                    FullName = "وحید صابری",
                    NickName = "Cyrus",
                    // Password@123
                    PasswordHash = "AHVmN19CcpcPWkBoEzRbbJrhS18HC1kH0KTzLXm4S41gHzKLvbCf2di7Nz+tZhUVIw==",
                    NormalizedEmail= "CYRUS.PIRATES@GMAIL.COM",
                    NormalizedUserName="SUPERADMIN",
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser
                {
                    Id = Constants.BasicUser,
                    UserName = "basicuser",
                    Email = "basicuser@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "09038907283",
                    PhoneNumberConfirmed = true,
                    Prefix = "آقای",
                    FullName = "آدرین فرهمند",
                    NickName = "Bogeyman",
                    // Password@123
                    PasswordHash = "AHVmN19CcpcPWkBoEzRbbJrhS18HC1kH0KTzLXm4S41gHzKLvbCf2di7Nz+tZhUVIw==",
                    NormalizedEmail= "BASICUSER@GMAIL.COM",
                    NormalizedUserName="BASICUSER",
                    SecurityStamp = Guid.NewGuid().ToString()
                },
            };
        }
    }
}
