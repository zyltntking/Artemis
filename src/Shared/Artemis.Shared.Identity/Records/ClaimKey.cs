using System.Runtime.Serialization;

namespace Artemis.Shared.Identity.Records;

/// <summary>
/// 凭据键
/// </summary>
[DataContract]
public record ClaimKey : IComparable<ClaimKey>
{
    /// <summary>
    /// 凭据相关标识
    /// </summary>
    [DataMember(Order = 1)]
    public Guid OuterId { get; init; }

    /// <summary>
    /// 凭据戳
    /// </summary>
    [DataMember(Order = 2)]
    public string CheckStamp { get; init; } = null!;

    #region Relational members

    /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
    public int CompareTo(ClaimKey? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var outerIdComparison = OuterId.CompareTo(other.OuterId);
        if (outerIdComparison != 0) return outerIdComparison;
        return string.Compare(CheckStamp, other.CheckStamp, StringComparison.Ordinal);
    }

    #endregion
}