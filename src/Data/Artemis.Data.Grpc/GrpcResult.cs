using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;

namespace Artemis.Data.Grpc;

/// <summary>
///     GRPC结果对象
/// </summary>
[DataContract]
public record GrpcResult : AbstractResult
{
    #region Overrides of AbstractResult

    /// <summary>
    ///     消息码
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required int Code { get; set; }

    /// <summary>
    ///     是否成功
    /// </summary>
    [DataMember(Order = 2)]
    public override bool Succeeded
    {
        get => Code == ResultStatus.Success;
        set
        {
            //ignore
        }
    }

    /// <summary>
    ///     消息
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public override required string Message { get; set; } = string.Empty;

    /// <summary>
    ///     异常信息
    /// </summary>
    [DataMember(Order = 4)]
    public override string? Error { get; set; }

    /// <summary>
    ///     本地时间戳
    /// </summary>
    [Required]
    [DataMember(Order = 5)]
    public override required DateTime DateTime { get; set; }

    /// <summary>
    ///     时间戳
    /// </summary>
    [Required]
    [DataMember(Order = 6)]
    public override required long Timestamp { get; set; }

    #endregion
}