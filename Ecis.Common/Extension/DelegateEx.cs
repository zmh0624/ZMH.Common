using System;

namespace ZMH.Common.Extension
{
    /// <summary>
    /// Func and Predicate Extension
    /// Author: Minghua
    /// </summary>
    public static class DelegateEx
    {
        public static Predicate<T> ToPredicate<T>(this Func<T, bool> source)
        {
            Predicate<T> result = new Predicate<T>(source);
            return result;
        }
    }
}