using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Artemis.Data.Store.ValueConverter;

/// <summary>
///     字符串集合值转换器
/// </summary>
public class StringCollectionValueConverter : ValueConverter<List<string>, string>
{
    /// <summary>
    ///     构造
    /// </summary>
    public StringCollectionValueConverter()
        : base(
            toValue => string.Join(',', toValue),
            fromValue => fromValue.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
    {
    }
}