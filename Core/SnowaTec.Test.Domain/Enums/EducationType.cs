using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SnowaTec.Test.Domain.Enum
{
    public enum EducationType
    {
        Illiterat = 1,
        Elementary = 2,
        Diploma = 3,
        Associate = 4,
        Bachelors = 5,
        Masters = 6,
        PHD = 7
    }

    public static class EducationTypeFunction
    {
        public static List<SelectListItem> ToSelectedList(EducationType selectedValue)
        {
            return System.Enum.GetValues(typeof(EducationType)).Cast<EducationType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString(), Selected = x == selectedValue ? true : false }).ToList();
        }
        public static List<SelectListItem> ToSelectedList()
        {
            return System.Enum.GetValues(typeof(EducationType)).Cast<EducationType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString() }).ToList();
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
            result.AddRange(System.Enum.GetValues(typeof(EducationType)).Cast<EducationType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString() }).ToList());
            return result;
        }
        public static string GetValue(EducationType x)
        {
            switch (x)
            {
                case EducationType.Illiterat:
                    return "بی سواد";
                case EducationType.Elementary:
                    return "ابتدایی";
                case EducationType.Diploma:
                    return "دیپلم";
                case EducationType.Associate:
                    return "فوق دیپلم";
                case EducationType.Bachelors:
                    return "لیسانس";
                case EducationType.Masters:
                    return "فوق لیسانس";
                case EducationType.PHD:
                    return "دکتری";
            }

            return "Unknown";
        }
        public static EducationType Get(int value)
        {
            switch (value)
            {
                case 1:
                    return EducationType.Illiterat;
                case 2:
                    return EducationType.Elementary;
                case 3:
                    return EducationType.Diploma;
                case 4:
                    return EducationType.Associate;
                case 5:
                    return EducationType.Bachelors;
                case 6:
                    return EducationType.Masters;
                case 7:
                    return EducationType.PHD;
            }

            return EducationType.Illiterat;
        }
    }
}
