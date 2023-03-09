using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SnowaTec.Test.Domain.Enum
{
    public enum MenuType
    {
        Group = 1,
        Menu = 2,
        License = 3
    }

    public static class MenuTypeFunction
    {
        public static List<SelectListItem> ToSelectedList(MenuType selectedValue)
        {
            return System.Enum.GetValues(typeof(MenuType)).Cast<MenuType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString(), Selected = x == selectedValue ? true : false }).ToList();
        }
        public static List<SelectListItem> ToSelectedList()
        {
            return System.Enum.GetValues(typeof(MenuType)).Cast<MenuType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString() }).ToList();
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
            result.AddRange(System.Enum.GetValues(typeof(MenuType)).Cast<MenuType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString() }).ToList());
            return result;
        }
        public static string GetValue(MenuType x)
        {
            switch (x)
            {
                case MenuType.Group:
                    return "Group";
                case MenuType.Menu:
                    return "Menu";
                case MenuType.License:
                    return "License";
            }

            return "Unknown";
        }
        public static MenuType Get(int value)
        {
            switch (value)
            {
                case 1:
                    return MenuType.Group;
                case 2:
                    return MenuType.Menu;
                case 3:
                    return MenuType.License;
            }

            return MenuType.Group;
        }
    }
}
