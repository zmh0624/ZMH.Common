using ZMH.Common.Exceptions;
using System;

namespace ZMH.Common.CommonHelper
{
    /// <summary>
    /// Create Instance Helper
    /// Author: Minghua
    /// </summary>
    public class InstanceHelper
    {
        public static T GetInstance<T>(Type t) where T : new()
        {
            try
            {
                return (T)Activator.CreateInstance(t);
            }
            catch (Exception ex)
            {
                LogExcept.Log.Error(ex);
                return default(T);
            }
        }

        public static T GetInstance<T>(Type t, string pvid, string userName) where T : new()
        {
            try
            {
                return (T)Activator.CreateInstance(t, pvid, userName);
            }
            catch (Exception ex)
            {
                LogExcept.Log.Error(ex);
                return default(T);
            }
        }
    }
}