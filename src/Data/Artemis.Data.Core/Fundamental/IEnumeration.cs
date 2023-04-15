namespace Artemis.Data.Core.Fundamental;

/// <summary>
/// 枚举接口
/// </summary>
/// <typeparam name="TEnum">枚举类</typeparam>
public interface IEnumeration<TEnum> : IComparable, IEquatable<TEnum>
{
    
}