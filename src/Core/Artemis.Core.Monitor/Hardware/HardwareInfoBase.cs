namespace Artemis.Core.Monitor.Hardware;

/// <summary>
///     硬件信息基类
/// </summary>
internal abstract class HardwareInfoBase
{
    /// <summary>
    ///  采集元数据
    /// </summary>
    public bool GatherMetadata { get; init; }

    #region LINUX

    /// <summary>
    /// 从文件中读取文本
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    internal static string TryReadTextFromFile(string path)
    {
        try
        {
            return File.ReadAllText(path).Trim();
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    ///     从文件中读取所有行
    /// </summary>
    /// <param name="path">文件路径</param>
    /// <returns></returns>
    internal static string[] TryReadLinesFromFile(string path)
    {
        try
        {
            return File.ReadAllLines(path);
        }
        catch
        {
            return Array.Empty<string>();
        }
    }

    /// <summary>
    /// Kb标记
    /// </summary>
    internal const string KbToken = "kB";

    /// <summary>
    ///  获取字节数
    /// </summary>
    /// <param name="memoryLine"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    internal static ulong GetBytesFromMemoryLine(string memoryLine, string token)
    {
        var mem = memoryLine.Replace(token, string.Empty).Replace(KbToken, string.Empty).Trim();

        if (ulong.TryParse(mem, out var memKb))
            return memKb * 1024;

        return 0;
    }

    /// <summary>
    ///  尝试从信息中读取整数
    /// </summary>
    /// <param name="batteryInfo"></param>
    /// <param name="possibleInfoToken"></param>
    /// <returns></returns>
    internal static uint TryReadIntegerFromBatteryInfos(string[] batteryInfo, params string[] possibleInfoToken)
    {
        foreach (var token in possibleInfoToken)
        {
            var infoLine = batteryInfo.FirstOrDefault(line => line.StartsWith(token));

            if (infoLine != null)
            {
                var info = infoLine.Replace(token, string.Empty).Trim();

                if (uint.TryParse(info, out uint integer))
                {
                    return integer;
                }
            }
        }

        return 0;
    }

    /// <summary>
    /// 尝试从信息中读取文本
    /// </summary>
    /// <param name="batteryInfo"></param>
    /// <param name="possibleInfoToken"></param>
    /// <returns></returns>
    internal static string TryReadTextFromBatteryInfos(string[] batteryInfo, params string[] possibleInfoToken)
    {
        foreach (var token in possibleInfoToken)
        {
            var infoLine = batteryInfo.FirstOrDefault(line => line.StartsWith(token));

            if (infoLine != null)
            {
                var info = infoLine.Replace(token, string.Empty).Trim();

                return info;
            }
        }

        return string.Empty;
    }

    #endregion

    #region WINDOWS

    /// <summary>
    ///  获取属性值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    internal static T GetPropertyValue<T>(object? obj) where T : struct
    {
        return (obj == null) ? default : (T)obj;
    }

    /// <summary>
    ///   获取属性字符串
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    internal static string GetPropertyString(object property)
    {
        return property as string ?? string.Empty;
    }

    /// <summary>
    ///  获取内存字节数
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    internal static ulong GetMemoryPropertyBytes(object property)
    {
        var mem = property is ulong size ? size : 0;

        return mem * 1024;
    }

    #endregion

    
}