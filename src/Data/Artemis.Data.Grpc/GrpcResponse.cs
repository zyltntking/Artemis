using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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
    public required GrpcPageResult Result { get; set; }

    /// <summary>
    /// 结果数据
    /// </summary>
    [DataMember(Order = 2)]
    public T? Data { get; set; }
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
    public required GrpcPageResult Result { get; set; }

    /// <summary>
    /// 分页信息
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required GrpcPageResult Page { get; set; }

    /// <summary>
    /// 数据集
    /// </summary>
    [DataMember(Order = 3)]
    public IEnumerable<T>? Date { get; set; }
}