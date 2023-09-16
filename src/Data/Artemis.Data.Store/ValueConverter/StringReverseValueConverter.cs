using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Artemis.Data.Store.ValueConverter;

/// <summary>
///     字符串反转值转换器
/// </summary>
public class StringReverseValueConverter : ValueConverter<string, string>
{
    /// <summary>
    ///     构造
    /// </summary>
    public StringReverseValueConverter()
        : base(
            toValue => new string(toValue.Reverse().ToArray()),
            fromValue => new string(fromValue.Reverse().ToArray()))
    {
    }
}