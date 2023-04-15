namespace Artemis.Data.Core.Fundamental;

/// <summary>
/// 枚举接口
/// </summary>
/// <typeparam name="TEnum">枚举类</typeparam>
public interface IEnumeration<TEnum> : IComparable, IEquatable<TEnum>
{
    /// <summary>
    /// 转换为int
    /// </summary>
    /// <returns></returns>
    int ToInt();
}