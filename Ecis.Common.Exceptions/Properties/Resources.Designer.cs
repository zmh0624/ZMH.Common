﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ecis.Common.Exceptions.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Ecis.Common.Exceptions.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 开药异常.
        /// </summary>
        internal static string ApplyDrugMsg {
            get {
                return ResourceManager.GetString("ApplyDrugMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ECIS程序发生非致命应用程序错误:.
        /// </summary>
        internal static string AppTitleMsg {
            get {
                return ResourceManager.GetString("AppTitleMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ECIS程序发生参数无效错误:.
        /// </summary>
        internal static string ArgTitleMsg {
            get {
                return ResourceManager.GetString("ArgTitleMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ECIS打印发生异常:.
        /// </summary>
        internal static string EcisPrintMsg {
            get {
                return ResourceManager.GetString("EcisPrintMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ECIS程序发生文件输入输出错误:.
        /// </summary>
        internal static string IOTitleMsg {
            get {
                return ResourceManager.GetString("IOTitleMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 保存医嘱发生异常.
        /// </summary>
        internal static string OrderSaveMsg {
            get {
                return ResourceManager.GetString("OrderSaveMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ECIS程序发生错误:.
        /// </summary>
        internal static string TitleMsg {
            get {
                return ResourceManager.GetString("TitleMsg", resourceCulture);
            }
        }
    }
}
