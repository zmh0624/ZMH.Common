using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ecis.Common.CommonHelper
{
    /// <summary>
    /// Cache helper class for Control
    /// Author: Minghua
    /// </summary>
    public class PluginCacheHelper
    {
        public static List<PluginData> Plugins
        {
            get
            {
                return EcisCommonCacheManager.PluginCache;
            }
        }

        public static Control GetCachedPlugin(string applicationType, Func<Control> creator)
        {
            Control control = null;

            if (EcisCommonCacheManager.PluginCache.Exists(x => x.PluginKey == applicationType))
            {
                var pluginData = EcisCommonCacheManager.PluginCache.Find(x => x.PluginKey == applicationType);
                return pluginData == null ? null : pluginData.Plugin;
            }
            else
            {
                if (!string.IsNullOrEmpty(applicationType))
                {
                    using (new CodeTimer(string.Format("Plugin: [{0}] Add to Cache", applicationType)))
                    {
                        control = creator();
                        if (control != null)
                        {
                            EcisCommonCacheManager.PluginCache.Add(new PluginData() { PluginKey = applicationType, Plugin = control });
                        }
                    }
                }
            }
            return control;
        }

        public static Control FindPlugin(string applicationType)
        {
            if (!EcisCommonCacheManager.PluginCache.Exists(x => x.PluginKey == applicationType))
            {
                return null;
            }

            var pluginData = EcisCommonCacheManager.PluginCache.Find(x => x.PluginKey == applicationType);
            return pluginData == null ? null : pluginData.Plugin;
        }

        /// <summary>
        /// 释放内存
        /// </summary>
        public static void Dispose()
        {
            EcisCommonCacheManager.PluginCache.Clear();
        }
    }

    public class PluginData
    {
        public string PluginKey { get; set; }

        public Control Plugin { get; set; }
    }
}