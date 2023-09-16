using Artemis.Data.Core.Fundamental;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Artemis.Data.Store.ValueConverter;

/// <summary>
///     枚举类型值转换器
/// </summary>
/// <typeparam name="T">枚举类</typeparam>
public class EnumerationValueConverter<T> : ValueConverter<T, string> where T : Enumeration
{
    /// <summary>
    ///     构造
    /// </summary>
    public EnumerationValueConverter()
        : base(
            toValue => toValue.ToString(),
            fromValue => Enumeration.FromName<T>(fromValue))
    {
    }
}