using ZMH.Common.Extension;
using System;
using System.Diagnostics;

namespace ZMH.Common
{
    public class CodeTimer : IDisposable
    {
        private readonly string _messageId;
        private readonly Stopwatch _stopwatch;

        private const string FORMATBEGIN = ">>[{0}]";
        private const string FORMATEND = "<<[{0}] - TraceTimer:{1}";

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable = true;

        public CodeTimer(string messageId)
        {
            if (IsEnable)
            {
                _messageId = messageId;
                _stopwatch = Stopwatch.StartNew();
                LogRepository.Log.Trace(string.Format(FORMATBEGIN, messageId));
            }
        }

        public void Dispose()
        {
            if (IsEnable)
            {
                _stopwatch.Stop();
                LogRepository.Log.Trace(FORMATEND, _messageId, _stopwatch.Elapsed.ToPerformanceTime());
            }
        }
    }
}