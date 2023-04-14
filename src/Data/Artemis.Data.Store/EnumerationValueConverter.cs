using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Artemis.Data.Core;

namespace Artemis.Data.Store;

/// <summary>
/// 枚举类型值转换器
/// </summary>
/// <typeparam name="T">枚举类</typeparam>
public class EnumerationValueConverter<T> : ValueConverter<T, string> where T : Enumeration
{
    /// <summary>
    /// 构造
    /// </summary>
    public EnumerationValueConverter()
        : base(
            v => v.ToString(),
            value => Enumeration.FromName<T>(value))
    {
    }
}