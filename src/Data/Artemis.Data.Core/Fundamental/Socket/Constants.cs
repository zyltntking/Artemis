namespace Artemis.Data.Core.Fundamental.Socket;

/// <summary>
///     协议预定义常量
/// </summary>
public static class Constants
{
    /// <summary>
    ///     类型
    /// </summary>
    public static class Type
    {
        /// <summary>
        ///     未知类型
        /// </summary>
        public const string Unknown = nameof(Unknown);

        #region Record

        /// <summary>
        ///     记录状态
        /// </summary>
        public const string StatusRecord = nameof(Socket.StatusRecord);

        /// <summary>
        ///     记录命令
        /// </summary>
        public const string CommandRecord = nameof(Socket.CommandRecord);

        /// <summary>
        ///     记录校验
        /// </summary>
        public const string CheckRecord = nameof(Socket.CheckRecord);

        /// <summary>
        ///     记录头
        /// </summary>
        public const string HeadRecord = nameof(Socket.HeadRecord);

        #endregion

        #region SystemDefine

        /// <summary>
        ///     Byte类型
        /// </summary>
        public const string Byte = nameof(System.Byte);

        /// <summary>
        ///     Long类型
        /// </summary>
        public const string Long = nameof(Int64);

        #endregion
    }

    /// <summary>
    ///     长度
    /// </summary>
    public static class Length
    {
        #region PreDefine

        /// <summary>
        ///     默认长度
        /// </summary>
        public const int Default = -9;

        /// <summary>
        ///     不定长
        /// </summary>
        public const int Unknown = -1;

        /// <summary>
        ///     空内容长度
        /// </summary>
        public const int Zero = 0;

        #endregion

        #region Record

        /// <summary>
        ///     记录状态
        /// </summary>
        public const int StatusRecord = 2;

        /// <summary>
        ///     记录命令
        /// </summary>
        public const int CommandRecord = 2;

        /// <summary>
        ///     记录校验
        /// </summary>
        public const int CheckRecord = 2;

        /// <summary>
        ///     记录头
        /// </summary>
        public const int HeadRecord = 12;

        #endregion

        #region SystemDefine

        /// <summary>
        ///     Byte类型
        /// </summary>
        public const int Byte = 1;

        /// <summary>
        ///     Long类型
        /// </summary>
        public const int Long = 8;

        #endregion
    }
}