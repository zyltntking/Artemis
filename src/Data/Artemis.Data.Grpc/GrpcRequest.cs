using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;

namespace Artemis.Data.Grpc;

[DataContract]
public abstract record GrpcPageRequest<T> : PageRequest<T>
{
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
    ///     过滤条件
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public override required T Filter { get; set; }

}