using System;

namespace Ecis.Common.Extension
{
    /// <summary>
    /// Guid Extension
    /// Author: Minghua
    /// </summary>
    public static class GuidEx
    {
        public static bool IsNotNullOrEmpty(this Guid gd)
        {
            return gd != null && gd != Guid.Empty;
        }

        public static bool IsNullOrEmpty(this Guid gd)
        {
            return gd == null || gd == Guid.Empty;
        }
    }
}