using System;

namespace ZMH.Common.Extension
{
    /// <summary>
    /// string Extension
    /// Author: Minghua
    /// </summary>
    public static class StringEx
    {
        public static bool IsTrue(this string s)
        {
            return s == "1" || s.ToLower() == "true";
        }

        public static bool IsFalse(this string s)
        {
            return s == "0" || s.ToLower() == "false";
        }

        public static bool IsNotNullOrEmpty(this string str)
        {
            return !str.IsNullOrEmpty();
        }

        public static bool IsNotNullOrWhitespace(this string str)
        {
            return !str.IsNullOrWhitespace();
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhitespace(this string value)
        {
            if (value != null)
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (!char.IsWhiteSpace(value[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool IsSameAs(this string str1, string str2)
        {
            return string.Compare(str1, str2, true) == 0;
        }

        public static string SafeTrim(this string val)
        {
            if (val == null)
            {
                return null;
            }
            return val.Trim();
        }

        public static string[] Split(this string str, string separator, bool removeEmptyItems)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            if (separator.IsNullOrEmpty())
            {
                return new string[] { str };
            }
            StringSplitOptions options = removeEmptyItems ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;
            return str.Split(new string[] { separator }, options);
        }

        public static string ToFormat(this string str, object arg1)
        {
            return string.Format(str, arg1);
        }

        public static string ToFormat(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        public static string ToFormat(this string str, object arg1, object arg2)
        {
            return string.Format(str, arg1, arg2);
        }

        public static string ToFormat(this string str, object arg1, object arg2, object arg3)
        {
            return string.Format(str, arg1, arg2, arg3);
        }

        public static string ToFormat(this string str, object arg1, object arg2, object arg3, object arg4)
        {
            return string.Format(str, new object[] { arg1, arg2, arg3, arg4 });
        }
    }
}