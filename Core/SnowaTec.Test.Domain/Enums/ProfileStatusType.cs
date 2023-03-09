using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace SnowaTec.Test.Domain.Enum
{
    public enum ProfileStatusType
    {
        Active = 1, //فعال
        Waiting = 2, //درحال انتظار
        Stagnant = 3 //راکد
    }
    public static class ProfileStatusTypeFunction
    {
        public static List<SelectListItem> ToSelectedList(ProfileStatusType selectedValue)
        {
            return System.Enum.GetValues(typeof(ProfileStatusType)).Cast<ProfileStatusType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString(), Selected = x == selectedValue ? true : false }).ToList();
        }
        public static List<SelectListItem> ToSelectedList()
        {
            return System.Enum.GetValues(typeof(ProfileStatusType)).Cast<ProfileStatusType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString() }).ToList();
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
            result.AddRange(System.Enum.GetValues(typeof(ProfileStatusType)).Cast<ProfileStatusType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString() }).ToList());
            return result;
        }
        public static string GetValue(ProfileStatusType x)
        {
            switch (x)
            {
                case ProfileStatusType.Active:
                    return "فعال";
                case ProfileStatusType.Waiting:
                    return "در لیست انتظار";
                case ProfileStatusType.Stagnant:
                    return "راکد";
            }

            return "Unknown";
        }
        public static ProfileStatusType Get(int value)
        {
            switch (value)
            {
                case 1:
                    return ProfileStatusType.Active;
                case 2:
                    return ProfileStatusType.Waiting;
                case 3:
                    return ProfileStatusType.Stagnant;
            }

            return ProfileStatusType.Stagnant;
        }
    }
}
