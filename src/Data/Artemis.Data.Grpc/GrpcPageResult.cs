using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;

namespace Artemis.Data.Grpc;

/// <summary>
/// GRPC分页信息对象
/// </summary>
[DataContract]
public record GrpcPageResult : AbstractPageResult
{
    #region Overrides of PageBase

    /// <summary>
    ///     当前页码(从1开始)
    /// </summary>
    /// <example>1</example>
    [Required]
    [DataMember(Order = 1)]
    public override required int Page { get; set; }

    /// <summary>
    ///     页面大小
    /// </summary>
    /// <example>20</example>
    [Required]
    [DataMember(Order = 2)]
    public override required int Size { get; set; }

    /// <summary>
    ///     过滤后数据条数
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public override required long Count { get; set; }

    /// <summary>
    ///     数据总量
    /// </summary>
    [Required]
    [DataMember(Order = 4)]
    public override required long Total { get; set; }

    #endregion
}