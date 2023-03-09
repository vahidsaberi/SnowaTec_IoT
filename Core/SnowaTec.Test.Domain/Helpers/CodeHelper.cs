using System;
using System.Collections.Generic;
using System.Linq;

namespace SnowaTec.Test.Domain.Helper
{
    public static class CodeHelper
    {
        public static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static T MostCommon<T>(this IEnumerable<T> list)
        {
            var most = list.GroupBy(i => i).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();

            return most;
        }
    }
}
