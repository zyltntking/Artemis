using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Artemis.Shared.Identity.Transfer;

/// <summary>
///     凭据键
/// </summary>
[DataContract]
public record ClaimKey : IComparable<ClaimKey>
{
    #region Relational members

    /// <summary>
    ///     Compares the current instance with another object of the same type and returns an integer that indicates
    ///     whether the current instance precedes, follows, or occurs in the same position in the sort order as the other
    ///     object.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>
    ///     A value that indicates the relative order of the objects being compared. The return value has these meanings:
    ///     <list type="table">
    ///         <listheader>
    ///             <term> Value</term><description> Meaning</description>
    ///         </listheader>
    ///         <item>
    ///             <term> Less than zero</term>
    ///             <description> This instance precedes <paramref name="other" /> in the sort order.</description>
    ///         </item>
    ///         <item>
    ///             <term> Zero</term>
    ///             <description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description>
    ///         </item>
    ///         <item>
    ///             <term> Greater than zero</term>
    ///             <description> This instance follows <paramref name="other" /> in the sort order.</description>
    ///         </item>
    ///     </list>
    /// </returns>
    public int CompareTo(ClaimKey? other)
    {
        if (ReferenceEquals(this, other))
            return 0;
        if (other is null)
            return 1;
        var outerIdComparison = OuterId.CompareTo(other.OuterId);
        return outerIdComparison != 0
            ? outerIdComparison
            : string.Compare(CheckStamp, other.CheckStamp, StringComparison.Ordinal);
    }

    #endregion

    /// <summary>
    ///     凭据相关标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required Guid OuterId { get; init; }

    /// <summary>
    ///     凭据戳
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required string CheckStamp { get; init; }
}

/// <summary>
///     凭据键比较器
/// </summary>
public class ClaimKeyEqualityComparer : IEqualityComparer<ClaimKey>
{
    /// <summary>
    ///     判断相等
    /// </summary>
    /// <param name="lhs">左侧资源</param>
    /// <param name="rhs">右侧资源</param>
    /// <returns></returns>
    public bool Equals(ClaimKey? lhs, ClaimKey? rhs)
    {
        if (ReferenceEquals(lhs, rhs)) return true;
        if (lhs is null) return false;
        if (rhs is null) return false;
        if (lhs.GetType() != rhs.GetType()) return false;
        return lhs.OuterId.Equals(rhs.OuterId) && lhs.CheckStamp == rhs.CheckStamp;
    }

    /// <summary>
    ///     获取哈希码
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int GetHashCode(ClaimKey obj)
    {
        return HashCode.Combine(obj.OuterId, obj.CheckStamp);
    }
}