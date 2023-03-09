using System;
using System.Collections.Generic;

namespace SnowaTec.Test.Domain.Helper
{
    public static class StringHelper
    {
        public static List<string> Find(this string str, string from = null, string until = null, StringComparison comparison = StringComparison.InvariantCulture)
        {
            var list = new List<string>();

            var fromLength = (from ?? string.Empty).Length;

            var startIndex = 0;

            while (startIndex < str.Length)
            {
                startIndex = !string.IsNullOrEmpty(from)
                    ? str.IndexOf(from, startIndex, comparison) + fromLength
                    : 0;

                if (startIndex < fromLength) { return list; }


                var endIndex = !string.IsNullOrEmpty(until)
                ? str.IndexOf(until, startIndex, comparison)
                : str.Length;

                if (endIndex < 0) { return list; }

                list.Add(str.Substring(startIndex, endIndex - startIndex));

                startIndex = endIndex;
            }

            return list;
        }
    }
}
