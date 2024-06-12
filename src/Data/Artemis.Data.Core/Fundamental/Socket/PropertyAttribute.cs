namespace Artemis.Data.Core.Fundamental.Socket;

/// <summary>
///     字段注记
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class PropertyAttribute : Attribute
{
    /// <summary>
    ///     序列
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    ///     长度
    /// </summary>
    public long Length { get; set; } = Constants.Length.Default;

    /// <summary>
    ///     转换器类型
    /// </summary>
    public string Type { get; set; } = Constants.Type.Unknown;

    /// <summary>
    ///     从左边开始填充，以修补字节码长度
    /// </summary>
    public bool PaddingLeft { get; set; } = true;
}