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
}