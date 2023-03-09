using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Enum;
using System.Collections.Generic;

namespace SnowaTec.Test.Persistence.Portal.Seeds
{
    public static class DefaultSystemParts
    {
        public static List<SystemPart> SystemPartList()
        {
            var id = 1;

            var superAdmin = new SystemPart { Id = id++, EnglishTitle = Roles.SuperAdmin.ToString(), PersianTitle = "مدیر پورتال", MenuType = MenuType.Group, IconName = "", Parent = null };

            var admin = new SystemPart { Id = id++, EnglishTitle = Roles.Admin.ToString(), PersianTitle = "مدیر", MenuType = MenuType.Group, IconName = "", Parent = null };

            //var moderator = new SystemPart { Id = id++, EnglishTitle = Roles.Moderator.ToString(), PersianTitle = "مجری", MenuType = MenuType.Group, IconName = "", Parent = null };

            //var basic = new SystemPart { Id = id++, EnglishTitle = Roles.Basic.ToString(), PersianTitle = "پایه", MenuType = MenuType.Group, IconName = "", Parent = null };

            var systemPart = new SystemPart { Id = id++, EnglishTitle = "SystemPart", PersianTitle = "بخش های سیستم", MenuType = MenuType.Menu, IconName = "grid", ParentId = superAdmin.Id };

            var portalInfo = new SystemPart { Id = id++, EnglishTitle = "PortalInfo", PersianTitle = "اطلاعات پورتال", MenuType = MenuType.Menu, IconName = "eye", ParentId = superAdmin.Id };

            var availability = new SystemPart { Id = id++, EnglishTitle = "Availability", PersianTitle = "مدیریت سطوح دسترسی", MenuType = MenuType.Menu, IconName = "book", ParentId = admin.Id };
            var availabilityCreate = new SystemPart { Id = id++, EnglishTitle = (PermissionType.Create).ToString(), PersianTitle = PermissionType.Create.ToString(), MenuType = MenuType.License, ParentId = availability.Id };
            var availabilityUpdate = new SystemPart { Id = id++, EnglishTitle = (PermissionType.Update).ToString(), PersianTitle = PermissionType.Update.ToString(), MenuType = MenuType.License, ParentId = availability.Id };
            var availabilityDelete = new SystemPart { Id = id++, EnglishTitle = (PermissionType.Delete).ToString(), PersianTitle = PermissionType.Delete.ToString(), MenuType = MenuType.License, ParentId = availability.Id };

            var user = new SystemPart { Id = id++, EnglishTitle = "User", PersianTitle = "مدیریت کاربران", MenuType = MenuType.Menu, IconName = "users", ParentId = admin.Id };
            var userCreate = new SystemPart { Id = id++, EnglishTitle = (PermissionType.Create).ToString(), PersianTitle = PermissionType.Create.ToString(), MenuType = MenuType.License, ParentId = user.Id };
            var userUpdate = new SystemPart { Id = id++, EnglishTitle = (PermissionType.Update).ToString(), PersianTitle = PermissionType.Update.ToString(), MenuType = MenuType.License, ParentId = user.Id };
            var userDelete = new SystemPart { Id = id++, EnglishTitle = (PermissionType.Delete).ToString(), PersianTitle = PermissionType.Delete.ToString(), MenuType = MenuType.License, ParentId = user.Id };

            var field = new SystemPart { Id = id++, EnglishTitle = "Field", PersianTitle = "مدیریت فیلدها", MenuType = MenuType.Menu, IconName = "figma", ParentId = admin.Id };
            
            var tag = new SystemPart { Id = id++, EnglishTitle = "Tag", PersianTitle = "مدیریت تگ ها", MenuType = MenuType.Menu, IconName = "tag", ParentId = admin.Id };
            
            var plugins = new SystemPart { Id = id++, EnglishTitle = "Installer", PersianTitle = "مدیریت پلاگین ها", MenuType = MenuType.Menu, IconName = "shopping-cart", ParentId = superAdmin.Id };

            return new List<SystemPart>()
            {
                superAdmin,
                admin,
                //moderator,
                //basic,
                systemPart,
                portalInfo,
                availability,
                availabilityCreate,
                availabilityUpdate,
                availabilityDelete,
                user,
                userCreate,
                userUpdate,
                userDelete,
                field,
                tag,
                plugins
            };
        }
    }
}
