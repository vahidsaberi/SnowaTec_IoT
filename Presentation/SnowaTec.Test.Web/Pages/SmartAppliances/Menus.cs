using SnowaTec.Test.Domain.Entities.Security;
using SnowaTec.Test.Domain.Enum;
using SnowaTec.Test.Web.Interfaces.Base;

namespace SnowaTec.Test.Web.Pages.SmartAppliances
{
    public class Menus : IMenu
    {
        public List<SystemPart> GetAll()
        {
            var smartAppliances = new SystemPart { EnglishTitle = "SmartAppliances", PersianTitle = "لوازم هوشمند", MenuType = MenuType.Group, IconName = "", AdditionalPermissions = true };

            var setting = new SystemPart { EnglishTitle = "Setting", PersianTitle = "تنظیمات", MenuType = MenuType.Menu, IconName = "settings", Parent = smartAppliances };
            var action = new SystemPart { EnglishTitle = "Action", PersianTitle = "دستگاه ها", MenuType = MenuType.Menu, IconName = "server", Parent = smartAppliances };

            var section = new SystemPart { EnglishTitle = "Section", PersianTitle = "مدیریت فضاها", MenuType = MenuType.Menu, IconName = "flag", Parent = setting };
            var sectionCreate = new SystemPart { EnglishTitle = (PermissionType.Create).ToString(), PersianTitle = PermissionType.Create.ToString(), MenuType = MenuType.License, Parent = section };
            var sectionUpdate = new SystemPart { EnglishTitle = (PermissionType.Update).ToString(), PersianTitle = PermissionType.Update.ToString(), MenuType = MenuType.License, Parent = section };
            var sectionDelete = new SystemPart { EnglishTitle = (PermissionType.Delete).ToString(), PersianTitle = PermissionType.Delete.ToString(), MenuType = MenuType.License, Parent = section };
            var sectionImport = new SystemPart { EnglishTitle = (PermissionType.Import).ToString(), PersianTitle = PermissionType.Import.ToString(), MenuType = MenuType.License, Parent = section };
            var sectionExport = new SystemPart { EnglishTitle = (PermissionType.Export).ToString(), PersianTitle = PermissionType.Export.ToString(), MenuType = MenuType.License, Parent = section };
            var sectionRecovery = new SystemPart { EnglishTitle = (PermissionType.Recovery).ToString(), PersianTitle = PermissionType.Recovery.ToString(), MenuType = MenuType.License, Parent = section };

            var device = new SystemPart { EnglishTitle = "Device", PersianTitle = "مدیریت دستگاه ها", MenuType = MenuType.Menu, IconName = "pocket", Parent = action };
            var deviceCreate = new SystemPart { EnglishTitle = (PermissionType.Create).ToString(), PersianTitle = PermissionType.Create.ToString(), MenuType = MenuType.License, Parent = device };
            var deviceUpdate = new SystemPart { EnglishTitle = (PermissionType.Update).ToString(), PersianTitle = PermissionType.Update.ToString(), MenuType = MenuType.License, Parent = device };
            var deviceDelete = new SystemPart { EnglishTitle = (PermissionType.Delete).ToString(), PersianTitle = PermissionType.Delete.ToString(), MenuType = MenuType.License, Parent = device };
            var deviceImport = new SystemPart { EnglishTitle = (PermissionType.Import).ToString(), PersianTitle = PermissionType.Import.ToString(), MenuType = MenuType.License, Parent = device };
            var deviceExport = new SystemPart { EnglishTitle = (PermissionType.Export).ToString(), PersianTitle = PermissionType.Export.ToString(), MenuType = MenuType.License, Parent = device };
            var deviceRecovery = new SystemPart { EnglishTitle = (PermissionType.Recovery).ToString(), PersianTitle = PermissionType.Recovery.ToString(), MenuType = MenuType.License, Parent = device };

            return new List<SystemPart>()
            {
                smartAppliances,

                setting,
                action,

                section,
                sectionCreate,
                sectionUpdate,
                sectionDelete,
                sectionImport,
                sectionExport,
                sectionRecovery,

                device,
                deviceCreate,
                deviceUpdate,
                deviceDelete,
                deviceImport,
                deviceExport,
                deviceRecovery
            };
        }
    }
}
