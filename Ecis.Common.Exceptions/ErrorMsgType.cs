namespace ZMH.Common.Exceptions
{
    /// <summary>
    /// 错误级别
    /// </summary>
    public enum ErrorMsgType
    {
        /// <summary>
        /// 微量（写日志）
        /// </summary>
        Trace,

        /// <summary>
        /// 调试（写日志）
        /// </summary>
        Debug,

        /// <summary>
        /// 信息（返回友好信息）
        /// </summary>
        Info,

        /// <summary>
        /// 警告（返回友好信息）
        /// </summary>
        Warn,

        /// <summary>
        /// 错误（写日志）
        /// </summary>
        Error,

        /// <summary>
        /// 致命（写日志）
        /// </summary>
        Fatal
    }
}