using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Artemis.Data.Store.ValueConverter;

/// <summary>
///     字符串规范化值转换器
/// </summary>
public class StringNormalizeValueConverter : ValueConverter<string, string>
{
    /// <summary>
    ///     构造
    /// </summary>
    public StringNormalizeValueConverter()
        : base(
            toValue => toValue,
            fromValue => fromValue.Normalize().ToUpper())
    {
    }
}