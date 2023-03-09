using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SnowaTec.Test.Domain.Enum
{
    public enum FieldType
    {
        CheckBox = 1,
        SelectBox = 2,
        Date = 3,
        String = 4,
        LongString = 5,
        NationalCode = 6,
        Phone = 7,
        Int = 8,
        Decimal = 9,
        RadioButton = 10,
    }

    public static class FieldTypeFunction
    {
        public static List<SelectListItem> ToSelectedList(FieldType selectedValue)
        {
            return System.Enum.GetValues(typeof(FieldType)).Cast<FieldType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString(), Selected = x == selectedValue ? true : false }).ToList();
        }
        public static List<SelectListItem> ToSelectedList()
        {
            return System.Enum.GetValues(typeof(FieldType)).Cast<FieldType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString() }).ToList();
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
            result.AddRange(System.Enum.GetValues(typeof(FieldType)).Cast<FieldType>().Select(x => new SelectListItem { Text = GetValue(x), Value = ((int)x).ToString() }).ToList());
            return result;
        }
        public static string GetValue(FieldType x)
        {
            switch (x)
            {
                case FieldType.CheckBox:
                    return " کادر انتخابی";
                case FieldType.SelectBox:
                    return "لیست انتخابی";
                case FieldType.Date:
                    return "تاریخ";
                case FieldType.String:
                    return "متن";
                case FieldType.LongString:
                    return "متن  چند سطری";
                case FieldType.NationalCode:
                    return "کد ملی";
                case FieldType.Phone:
                    return "تلفن";
                case FieldType.Int:
                    return "عدد صحیح";
                case FieldType.Decimal:
                    return "عدد اعشار";
                case FieldType.RadioButton:
                    return "دکمه رادیویی";
            }

            return "Unknown";
        }
        public static FieldType Get(int value)
        {
            switch (value)
            {
                case 1: return FieldType.CheckBox;
                case 2: return FieldType.SelectBox;
                case 3: return FieldType.Date;
                case 4: return FieldType.String;
                case 5: return FieldType.LongString;
                case 6: return FieldType.NationalCode;
                case 7: return FieldType.Phone;
                case 8: return FieldType.Int;
                case 9: return FieldType.Decimal;
                case 10: return FieldType.RadioButton;
            }

            return FieldType.String;
        }

        public static Type GetTypeOf(FieldType x)
        {
            switch (x)
            {
                case FieldType.CheckBox:
                    return typeof(List<string>);
                case FieldType.SelectBox:
                    return typeof(string);
                case FieldType.Date:
                    return typeof(DateTime);
                case FieldType.String:
                    return typeof(string);
                case FieldType.LongString:
                    return typeof(string);
                case FieldType.NationalCode:
                    return typeof(string);
                case FieldType.Phone:
                    return typeof(string);
                case FieldType.Int:
                    return typeof(int);
                case FieldType.Decimal:
                    return typeof(decimal);
                case FieldType.RadioButton:
                    return typeof(string);
            }

            return typeof(string);
        }
    }
}
