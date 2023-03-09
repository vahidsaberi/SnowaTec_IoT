using SnowaTec.Test.Domain.Helper;
using System;
using System.Collections.Generic;

namespace SnowaTec.Test.Domain.DTO.Portal
{
    public class WeekDateDto
    {
        public WeekDateDto(List<DateTime> dates)
        {
            EnglishCurrentDate = DateTime.Now.Date;
            PersianCurrentDate = Convert.ToDateTime(PersianDateHelper.EnglishToPersianDate(EnglishCurrentDate));
            EnglishDates = dates;
            PersianDates = ConverToPersianDate(dates);
        }
        public DateTime EnglishCurrentDate { get; set; }
        public DateTime PersianCurrentDate { get; set; }
        public List<DateTime> PersianDates { get; set; } = new List<DateTime>();
        public List<DateTime> EnglishDates { get; set; }

        private List<DateTime> ConverToPersianDate(List<DateTime> dates)
        {
            var persianDates = new List<DateTime>();
            foreach (var item in dates)
            {
                persianDates.Add(Convert.ToDateTime(PersianDateHelper.EnglishToPersianDate(item)));
            }
            return persianDates;
        }
    }
}
