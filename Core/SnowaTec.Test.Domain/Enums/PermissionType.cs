using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace SnowaTec.Test.Domain.Enum
{
    public enum PermissionType
    {
        Create = 1,
        Read = 2,
        Update = 3,
        Delete = 4,
        Report = 5,
        Import = 6,
        Export = 7,
        Recovery = 8
    }

    public static class PermissionTypeFunction
    {
        public static string GetValue(PermissionType x)
        {
            switch (x)
            {
                case PermissionType.Create:
                    return "Create";
                case PermissionType.Read:
                    return "Read";
                case PermissionType.Update:
                    return "Update";
                case PermissionType.Delete:
                    return "Delete";
                case PermissionType.Report:
                    return "Report";
                case PermissionType.Import:
                    return "Import";
                case PermissionType.Export:
                    return "Export";
                case PermissionType.Recovery:
                    return "Recovery";
            }

            return "Unknown";
        }

        public static List<SelectListItem> ToSelectedList(PermissionType selectedValue)
        {
            return System.Enum.GetValues(typeof(PermissionType)).Cast<PermissionType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString(), Selected = x == selectedValue ? true : false }).ToList();
        }

        public static List<SelectListItem> ToSelectedList()
        {
            return System.Enum.GetValues(typeof(PermissionType)).Cast<PermissionType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString() }).ToList();
        }

        public static List<SelectListItem> ToSelectedListForSearch(bool addAllItem = true)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            if (addAllItem)
            {
                result.Add(new SelectListItem()
                {
                    Selected = true,
                    Text = "All",
                    Value = ""
                });
            }
            result.AddRange(System.Enum.GetValues(typeof(PermissionType)).Cast<PermissionType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString() }).ToList());
            return result;
        }

        public static PermissionType GetEnum(int value)
        {
            switch (value)
            {
                case 1:
                    return PermissionType.Create;
                case 2:
                    return PermissionType.Read;
                case 3:
                    return PermissionType.Update;
                case 4:
                    return PermissionType.Delete;
                case 5:
                    return PermissionType.Report;
                case 6:
                    return PermissionType.Import;
                case 7:
                    return PermissionType.Export;
                case 8:
                    return PermissionType.Recovery;
            }

            return 0;
        }
    }
}
