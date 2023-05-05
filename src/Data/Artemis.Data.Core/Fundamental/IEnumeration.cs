namespace Artemis.Data.Core.Fundamental;

/// <summary>
///     枚举接口
/// </summary>
/// <typeparam name="TEnum">枚举类</typeparam>
internal interface IEnumeration<TEnum> : IComparable, IEquatable<TEnum>
{
    /// <summary>
    ///     枚举名称
    /// </summary>
    string Name { get; }

    /// <summary>
    ///     枚举ID
    /// </summary>
    int Id { get; }
}