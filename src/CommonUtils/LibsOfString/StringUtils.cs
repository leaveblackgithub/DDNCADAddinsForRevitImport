﻿using System.Linq;

namespace CommonUtils.LibsOfString
{
    public static class StringUtils
    {
        public static bool EqualsIgnoreCase(this string str1, string str2)
        {
            return string.Equals(str1, str2, System.StringComparison.OrdinalIgnoreCase);
        }
        //StartsWithIgnoreCase
        public static bool StartsWithIgnoreCase(this string str1, params string[] str2)
        {
            if (str2.Length == 0) return false;
            string prefix = str2.Length==1 ? str2[0]:string.Join("",str2);
            return str1.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase);
        }

        public static string[] SplitByString(this string str, string separator)
        {
            return str.Split(new[] {separator}, System.StringSplitOptions.None);
        }
    }
}