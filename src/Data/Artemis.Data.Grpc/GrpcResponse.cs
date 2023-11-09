using System.ComponentModel.DataAnnotations;

namespace Artemis.Data.Grpc;

/// <summary>
/// GRPC响应对象
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract record GrpcResponse<T>
{
    /// <summary>
    /// 结果信息
    /// </summary>
    [Required]
    public required GrpcPageResult Result { get; set; }

    /// <summary>
    /// 结果数据
    /// </summary>
    public T? Data { get; set; }
}

/// <summary>
/// GRPC分页响应对象
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract record GrpcPageResponse<T>
{
    /// <summary>
    /// 结果信息
    /// </summary>
    [Required]
    public required GrpcPageResult Result { get; set; }

    /// <summary>
    /// 分页信息
    /// </summary>
    [Required]
    public required GrpcPageResult Page { get; set; }

    /// <summary>
    /// 数据集
    /// </summary>
    public IEnumerable<T>? Date { get; set; }
}