using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Ecis.Common.CommonHelper
{
    /// <summary>
    /// 内存释放帮助类（峰值200M时释放物理内存）
    /// 推荐使用
    /// System.Windows.Forms.Timer进行动态释放
    /// </summary>
    public class MemoryHelper
    {
        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetProcessWorkingSetSize32(IntPtr proc, int min, int max);

        [DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize", SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetProcessWorkingSetSize64(IntPtr proc, long min, long max);

        /// <summary>
        /// 根据动态物理内存释放（默认峰值为200M）
        /// </summary>
        public static void FlushMemoryBySize()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            var currentProcess = Process.GetCurrentProcess();
            var physicalMemory = currentProcess.WorkingSet64;

            var size = MemoryFilesize(physicalMemory);
            if (size.Contains("PB"))
            {
                ClearMemory(currentProcess.Handle);
            }
            else if (size.Contains("TB"))
            {
                ClearMemory(currentProcess.Handle);
            }
            else if (size.Contains("GB"))
            {
                ClearMemory(currentProcess.Handle);
            }
            else if (size.Contains("MB"))
            {
                double memorysize = Convert.ToDouble(size.Replace("MB", ""));
                if (memorysize > 200.0)
                {
                    ClearMemory(currentProcess.Handle);
                }
            }
        }

        /// <summary>
        /// 根据进程Handle直接释放物理内存
        /// </summary>
        /// <param name="handle"></param>
        public static void ClearMemory(IntPtr handle)
        {
            if (Environment.Is64BitProcess)
            {
                //x64
                SetProcessWorkingSetSize64(handle, -1, -1);
            }
            else
            {
                //x86
                SetProcessWorkingSetSize32(handle, -1, -1);
            }
        }

        /// <summary>
        /// 内存的字节单位转换
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string MemoryFilesize(double size)
        {
            string[] units = new string[] { "B", "KB", "MB", "GB", "TB", "PB" };
            double mod = 1024.0;
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size, 2) + units[i];
        }
    }
}