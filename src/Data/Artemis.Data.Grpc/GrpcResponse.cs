using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;

namespace Artemis.Data.Grpc;

/// <summary>
/// GRPC响应对象
/// </summary>
/// <typeparam name="T"></typeparam>
[DataContract]
public abstract record GrpcResponse<T>
{
    /// <summary>
    /// 结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required GrpcPageResult Result { get; set; }

    /// <summary>
    /// 结果数据
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public virtual required T Data { get; set; } = default!;
}

/// <summary>
/// GRPC空响应对象
/// </summary>
[DataContract]
public sealed record GrpcEmptyResponse : GrpcResponse<EmptyRecord>
{
    #region Overrides of GrpcResponse<EmptyRecord>

    /// <summary>
    /// 结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcPageResult Result { get; set; }

    /// <summary>
    /// 结果数据
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required EmptyRecord Data { get; set; }

    #endregion
}

/// <summary>
/// GRPC分页响应对象
/// </summary>
/// <typeparam name="T"></typeparam>
[DataContract]
public abstract record GrpcPageResponse<T>
{
    /// <summary>
    /// 结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required GrpcPageResult Result { get; set; }

    /// <summary>
    /// 分页信息
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public virtual required GrpcPageResult Page { get; set; }

    /// <summary>
    /// 数据集
    /// </summary>
    [DataMember(Order = 3)]
    public virtual required IEnumerable<T> Date { get; set; }
}