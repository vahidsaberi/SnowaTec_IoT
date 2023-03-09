using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SnowaTec.Test.Domain.Helper
{
    public static class PersianDateHelper
    {
        #region DiffTime
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;

        public static string Calculate(DateTime dateTime)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - dateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);
            if (delta < 1 * MINUTE)
            {
                return ts.Seconds == 1 ? "لحظه ای قبل" : ts.Seconds + " ثانیه قبل";
            }
            if (delta < 2 * MINUTE)
            {
                return "یک دقیقه قبل";
            }
            if (delta < 45 * MINUTE)
            {
                return ts.Minutes + " دقیقه قبل";
            }
            if (delta < 90 * MINUTE)
            {
                return "یک ساعت قبل";
            }
            if (delta < 24 * HOUR)
            {
                return ts.Hours + " ساعت قبل";
            }
            if (delta < 48 * HOUR)
            {
                return "دیروز";
            }
            if (delta < 30 * DAY)
            {
                return ts.Days + " روز قبل";
            }
            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "یک ماه قبل" : months + " ماه قبل";
            }
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "یک سال قبل" : years + " سال قبل";
        }

        public static string CalculateReverce(DateTime dateTime)
        {
            var ts = new TimeSpan(dateTime.Ticks - DateTime.Now.Ticks);

            string responce = "";
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            if (years > 0)
            {
                responce += years + " سال ";
            }
            int delta = ts.Days - (years * 365);
            int monthes = Convert.ToInt32(Math.Floor((double)delta / 30));
            if (monthes > 0)
            {
                responce += monthes + " ماه ";
            }
            delta = delta - (monthes * 30);
            if (delta > 0)
            {
                responce += delta + " روز";
            }
            return responce;
        }
        #endregion DiffTime

        public static string PersianToEnglishNumber(this string persianStr)
        {
            List<Tuple<char, char>> LettersDictionary = new List<Tuple<char, char>>();
            //persian
            LettersDictionary.Add(Tuple.Create('۰', '0'));
            LettersDictionary.Add(Tuple.Create('۱', '1'));
            LettersDictionary.Add(Tuple.Create('۲', '2'));
            LettersDictionary.Add(Tuple.Create('۳', '3'));
            LettersDictionary.Add(Tuple.Create('۴', '4'));
            LettersDictionary.Add(Tuple.Create('۵', '5'));
            LettersDictionary.Add(Tuple.Create('۶', '6'));
            LettersDictionary.Add(Tuple.Create('۷', '7'));
            LettersDictionary.Add(Tuple.Create('۸', '8'));
            LettersDictionary.Add(Tuple.Create('۹', '9'));
            //arabic              
            LettersDictionary.Add(Tuple.Create('٠', '0'));
            LettersDictionary.Add(Tuple.Create('١', '1'));
            LettersDictionary.Add(Tuple.Create('٢', '2'));
            LettersDictionary.Add(Tuple.Create('٣', '3'));
            LettersDictionary.Add(Tuple.Create('٤', '4'));
            LettersDictionary.Add(Tuple.Create('٥', '5'));
            LettersDictionary.Add(Tuple.Create('٦', '6'));
            LettersDictionary.Add(Tuple.Create('٧', '7'));
            LettersDictionary.Add(Tuple.Create('٨', '8'));
            LettersDictionary.Add(Tuple.Create('٩', '9'));
            //other               
            LettersDictionary.Add(Tuple.Create('۱', '1'));
            LettersDictionary.Add(Tuple.Create('۲', '2'));
            LettersDictionary.Add(Tuple.Create('۳', '3'));
            LettersDictionary.Add(Tuple.Create('۴', '4'));
            LettersDictionary.Add(Tuple.Create('۵', '5'));
            LettersDictionary.Add(Tuple.Create('۶', '6'));
            LettersDictionary.Add(Tuple.Create('۷', '7'));
            LettersDictionary.Add(Tuple.Create('۸', '8'));
            LettersDictionary.Add(Tuple.Create('۹', '9'));
            foreach (var item in persianStr)
                if (LettersDictionary.Count(x => x.Item1 == item) > 0)
                    persianStr = persianStr.Replace(item, LettersDictionary.First(x => x.Item1 == item).Item2);
            return persianStr;
        }

        public static string ShowDateWithDayNumber(long dayNumber)
        {
            var date = DateTime.Now.AddDays(dayNumber);
            return CalculateReverce(date);
        }

        public static DateTime PersianToEnglishDateTime(string date)
        {
            date = PersianToEnglishNumber(date);
            var persianCalendar = new PersianCalendar();
            try
            {
                return new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(5, 2)),
                    int.Parse(date.Substring(8, 2)), int.Parse(date.Substring(11, 2)), int.Parse(date.Substring(14, 2)), 0, persianCalendar);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public static DateTime PersianToEnglishDateFilter(string date, bool isStart)
        {
            date = PersianToEnglishNumber(date);
            var persianCalendar = new PersianCalendar();
            try
            {
                if (isStart)
                {
                    return new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(5, 2)),
                        int.Parse(date.Substring(8, 2)), 0, 0, 0, persianCalendar);
                }
                else
                {
                    return new DateTime(int.Parse(date.Substring(0, 4)), int.Parse(date.Substring(5, 2)),
                        int.Parse(date.Substring(8, 2)), 23, 59, 59, persianCalendar);
                }
            }
            catch
            {
                return DateTime.Now;
            }
        }

        public static string EnglishToPersianDateTime(DateTime date)
        {
            var persianCalendar = new PersianCalendar();
            string year = persianCalendar.GetYear(date).ToString();
            string month = persianCalendar.GetMonth(date) > 9 ? persianCalendar.GetMonth(date).ToString() : "0" + persianCalendar.GetMonth(date).ToString();
            string day = persianCalendar.GetDayOfMonth(date) > 9 ? persianCalendar.GetDayOfMonth(date).ToString() : "0" + persianCalendar.GetDayOfMonth(date).ToString();
            return string.Format("{0}-{1}-{2} {3}:{4}", year, month, day, date.Hour < 9 ? "0" + date.Hour : date.Hour.ToString(), date.Minute < 9 ? "0" + date.Minute : date.Minute.ToString());
        }

        public static string ConvertToPersianDateTime(DateTime dateTime)
        {
            var persian = new PersianCalendar();

            return string.Format("{3}{6}{4:D2}{6}{5:D2}-{0}/{1}/{2}",


                                 persian.GetYear(dateTime),
                                 persian.GetMonth(dateTime),
                                 persian.GetDayOfMonth(dateTime),
                                 persian.GetHour(dateTime),
                                 persian.GetMinute(dateTime),
                                 persian.GetSecond(dateTime),

                                 DateTimeFormatInfo.CurrentInfo.TimeSeparator);

            //return string.Format("{0}/{1}/{2} {3}{6}{4:D2}{6}{5:D2}\n",
            //                        persian.GetMonth(dateTime),
            //                        persian.GetDayOfMonth(dateTime),
            //                        persian.GetYear(dateTime),
            //                        persian.GetHour(dateTime),
            //                        persian.GetMinute(dateTime),
            //                        persian.GetSecond(dateTime),
            //                        DateTimeFormatInfo.CurrentInfo.TimeSeparator);
        }

        public static string GetFullDateLetter(DateTime model)
        {
            DayOfWeek todayWeek = model.DayOfWeek;
            string today = "";

            if (todayWeek == DayOfWeek.Saturday)
            {
                today = "شنبه";
            }
            else if (todayWeek == DayOfWeek.Sunday)
            {
                today = "یکشنبه";
            }
            else if (todayWeek == DayOfWeek.Monday)
            {
                today = "دوشنبه";
            }
            else if (todayWeek == DayOfWeek.Tuesday)
            {
                today = "سه شنبه";
            }
            else if (todayWeek == DayOfWeek.Wednesday)
            {
                today = "چهار شنبه";
            }
            else if (todayWeek == DayOfWeek.Thursday)
            {
                today = "پنج شنبه";
            }
            else if (todayWeek == DayOfWeek.Friday)
            {
                today = "جمعه";
            }

            string month = ConvertToPersianDateTime(model).Split('-')[1].Split('/')[1]
                .Replace("10", "دی")
                .Replace("11", "بهمن")
                .Replace("12", "اسفند")
                .Replace("1", "فروردین")
                .Replace("2", "اردیبهشت")
                .Replace("3", "خرداد")
                .Replace("4", "تیر")
                .Replace("5", "مرداد")
                .Replace("6", "شهریور")
                .Replace("7", "مهر")
                .Replace("8", "آبان")
                .Replace("9", "آذر");

            string date = ConvertToPersianDateTime(model).Split('-')[1];
            return today + " " + date.Split('/')[2] + " " + month + " " + date.Split('/')[0];
        }

        public static string EnglishToPersianDate(DateTime date)
        {
            var persianCalendar = new PersianCalendar();
            string year = persianCalendar.GetYear(date).ToString();
            string month = persianCalendar.GetMonth(date) > 9 ? persianCalendar.GetMonth(date).ToString() : "0" + persianCalendar.GetMonth(date).ToString();
            string day = persianCalendar.GetDayOfMonth(date) > 9 ? persianCalendar.GetDayOfMonth(date).ToString() : "0" + persianCalendar.GetDayOfMonth(date).ToString();
            return string.Format("{0}-{1}-{2}", year, month, day);
        }

        public static string PersianNumberToLetter(string number)
        {
            int length = number.Length, Level = 0;
            string Text = "";
            List<int> TriDigits = new List<int>();
            bool flag = false;
            if (length != 0)
            {
                try
                {
                    while (length != 0)
                    {
                        if (length > 3)
                        {
                            TriDigits.Add(System.Convert.ToInt32(number.Substring(length - 3, 3)));
                            length -= 3;
                        }
                        else
                        {
                            TriDigits.Add(System.Convert.ToInt32(number.Substring(0, length)));
                            length = 0;
                        }
                    }
                }
                catch
                {
                    return "لطفا فقط عدد وارد کنید !";
                }
                foreach (int num in TriDigits)
                {
                    if (num != 0)
                    {
                        if (NumberToLetterConvert.Leveler(Level, flag) == "ERROR")
                            break;
                        else
                        {
                            Text = NumberToLetterConvert.Leveler(Level, flag) + Text;
                            Text = NumberToLetterConvert.Thousand(num) + Text;
                            flag = true;
                        }
                    }
                    Level++;
                }
                if (Text == "")
                    return "صفر";
                else
                    return Text;
            }
            else
                return "لطفا عدد مورد نظر را وارد کنید !";
        }
    }

    public static class NumberToLetterConvert
    {
        /// <summary>
        /// برای تبدیل یکان عدد به حرف استفاده می شود
        /// </summary>
        /// <param name="Literal">حاوی شکل حروفی اعداد تبدیل شده در مرحله قبل</param>
        /// <param name="digit_yekan">یکان عدد</param>
        /// <param name="digit_dahgan">دهگان عدد</param>
        /// <param name="haveup">آیا مرحله قبل وجود دارد</param>
        /// <returns>شکل حرفی عدد String</returns>
        public static string Ones(string Literal, int digit_yekan, int digit_dahgan, bool haveup)
        {

            if (digit_dahgan != 1)
            {
                if (digit_yekan == 0 && !haveup)
                    return Literal = "صفر";
                else if (digit_yekan == 0 && haveup)
                    return Literal;
                if (haveup)
                {
                    Literal += " و ";
                }
                switch (digit_yekan)
                {
                    case 1:
                        Literal += "یک";
                        break;
                    case 2:
                        Literal += "دو";
                        break;
                    case 3:
                        Literal += "سه";
                        break;
                    case 4:
                        Literal += "چهار";
                        break;
                    case 5:
                        Literal += "پنج";
                        break;
                    case 6:
                        Literal += "شش";
                        break;
                    case 7:
                        Literal += "هفت";
                        break;
                    case 8:
                        Literal += "هشت";
                        break;
                    default:
                        Literal += "نه";
                        break;
                }
            }
            return Literal;
        }

        /// <summary>
        /// برای تبدیل دهگان عدد به حرف استفاده می شود
        /// </summary>
        /// <param name="Literal">حاوی شکل حروفی اعداد تبدیل شده در مرحله قبل</param>
        /// <param name="digit_yekan">یکان عدد</param>
        /// <param name="digit_dahgan">دهگان عدد</param>
        /// <param name="haveup">آیا مرحله قبل وجود دارد</param>
        /// <returns>شکل حرفی عدد String</returns>
        public static string Tens(string Literal, int digit_yekan, int digit_dahgan, bool haveup)
        {
            if (haveup && digit_dahgan != 0)
                Literal += " و ";
            if (digit_dahgan == 1)
            {
                switch (digit_yekan)
                {
                    case 0:
                        Literal += "ده";
                        break;
                    case 1:
                        Literal += "یازده";
                        break;
                    case 2:
                        Literal += "دوازده";
                        break;
                    case 3:
                        Literal += "سیزده";
                        break;
                    case 4:
                        Literal += "چهارده";
                        break;
                    case 5:
                        Literal += "پانزده";
                        break;
                    case 6:
                        Literal += "شانزده";
                        break;
                    case 7:
                        Literal += "هفده";
                        break;
                    case 8:
                        Literal += "هجده";
                        break;
                    case 9:
                        Literal += "نوزده";
                        break;
                }
            }
            else
            {
                switch (digit_dahgan)
                {
                    case 2:
                        Literal += "بیست";
                        break;
                    case 3:
                        Literal += "سی";
                        break;
                    case 4:
                        Literal += "چهل";
                        break;
                    case 5:
                        Literal += "پنجاه";
                        break;
                    case 6:
                        Literal += "شصت";
                        break;
                    case 7:
                        Literal += "هفتاد";
                        break;
                    case 8:
                        Literal += "هشتاد";
                        break;
                    case 9:
                        Literal += "نود";
                        break;
                }
            }
            return Literal;

        }

        /// <summary>
        /// برای تبدیل صدگان عدد به حرف استفاده می شود
        /// </summary>
        /// <param name="Literal">حاوی شکل حروفی اعداد تبدیل شده در مرحله قبل</param>
        /// <param name="digit_sadgan">صدگان عدد</param>
        /// <param name="haveup">آیا مرحله قبل وجود دارد</param>
        /// <returns>شکل حرفی عدد String</returns>
        public static string Hundreds(string Literal, int digit_sadgan, bool haveup)
        {
            if (haveup)
                Literal += " و ";
            switch (digit_sadgan)
            {
                case 1:
                    Literal += "یکصد";
                    break;
                case 2:
                    Literal += "دویست";
                    break;
                case 3:
                    Literal += "سیصد";
                    break;
                case 4:
                    Literal += "چهارصد";
                    break;
                case 5:
                    Literal += "پانصد";
                    break;
                case 6:
                    Literal += "ششصد";
                    break;
                case 7:
                    Literal += "هفتصد";
                    break;
                case 8:
                    Literal += "هشتصد";
                    break;
                case 9:
                    Literal += "نهصد";
                    break;
            }
            return Literal;
        }
        /// <summary>
        /// تبدیل عدد سه رقمی به شکل حروفی اش
        /// </summary>
        /// <param name="input">یک عدد سه رقمی برای تبدیل شدن به شکل حروفی اش</param>
        /// <returns>شکل حروفی عدد وارد شده</returns>
        public static string Thousand(int input)
        {
            List<int> digits = new List<int>();
            while (input >= 0)
            {
                if (input == 0 && digits.Count == 0)
                {
                    digits.Add(input);
                    break;
                }
                else if (input == 0 && digits.Count != 0)
                    break;
                digits.Add(input % 10);
                input /= 10;
            }
            if (digits.Count == 1)
            {
                return Ones("", digits[0], 0, false);
            }
            else if (digits.Count == 2)
            {
                return Ones(Tens("", digits[0], digits[1], false), digits[0], digits[1], true);
            }
            else
            {
                return Ones(Tens(Hundreds("", digits[2], false), digits[0], digits[1], true), digits[0], digits[1], true);
            }


        }
        public static string Leveler(int level, bool flag)
        {
            if (flag)
            {
                switch (level)
                {
                    case 0:
                        return " ";
                    case 1:
                        return " هزار و ";
                    case 2:
                        return " میلیون و ";
                    case 3:
                        return " میلیارد و ";
                    case 4:
                        return " بیلیون و ";
                    case 5:
                        return " بیلیارد و ";
                    case 6:
                        return " تریلیون و ";
                    case 7:
                        return " تریلیارد و ";
                    case 8:
                        return " کادریلیون و ";
                    default:
                        return "ERROR";
                }
            }
            else
            {
                switch (level)
                {
                    case 0:
                        return " ";
                    case 1:
                        return " هزار ";
                    case 2:
                        return " میلیون ";
                    case 3:
                        return " میلیارد ";
                    case 4:
                        return " بیلیون ";
                    case 5:
                        return " بیلیارد ";
                    case 6:
                        return " تریلیون ";
                    case 7:
                        return " تریلیارد ";
                    case 8:
                        return " کادریلیون ";
                    default:
                        return "ERROR";
                }
            }
        }
    }
}
