using System;

namespace Ecis.Common.Extension
{
    public static class ObjectEx
    {
        public static bool IsNotNull(this object obj)
        {
            return obj != null && obj != DBNull.Value;
        }

        public static bool IsNull(this object obj)
        {
            return obj == null || obj == DBNull.Value;
        }
    }
}