using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace SnowaTec.Test.Domain.Enum
{
    public enum ActionType
    {
        Create = 1,
        Read = 2,
        Update = 3,
        Delete = 4,
        Recovery = 5
    }

    public static class ActionTypeFunction
    {
        public static List<SelectListItem> ToSelectedList(ActionType selectedValue)
        {
            return System.Enum.GetValues(typeof(ActionType)).Cast<ActionType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString(), Selected = x == selectedValue ? true : false }).ToList();
        }
        public static List<SelectListItem> ToSelectedList()
        {
            return System.Enum.GetValues(typeof(ActionType)).Cast<ActionType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString() }).ToList();
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
            result.AddRange(System.Enum.GetValues(typeof(ActionType)).Cast<ActionType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString() }).ToList());
            return result;
        }
        public static string GetValue(ActionType x)
        {
            switch (x)
            {
                case ActionType.Create:
                    return "ایجاد";
                case ActionType.Read:
                    return "خواندن";
                case ActionType.Update:
                    return "بروزرسانی";
                case ActionType.Delete:
                    return "حذف";
                case ActionType.Recovery:
                    return "بازیابی";
            }

            return "Unknown";
        }
        public static ActionType Get(int value)
        {
            switch (value)
            {
                case 1:
                    return ActionType.Create;
                case 2:
                    return ActionType.Read;
                case 3:
                    return ActionType.Update;
                case 4:
                    return ActionType.Delete;
                case 5:
                    return ActionType.Recovery;
            }

            return ActionType.Read;
        }
    }
}
