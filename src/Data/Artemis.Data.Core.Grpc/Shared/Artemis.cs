using System.Collections.ObjectModel;
using ProtoBuf;

namespace Artemis.Data.Core.Grpc.Shared;


/// <summary>
/// RPC共享协议结果
/// </summary>
[ProtoContract]
public class RpcDataResult<T> : ResultBase<T>
{
    #region Implementation of IDataResult

    /// <summary>
    ///     消息码
    /// </summary>
    [ProtoMember(1)]
    public override int Code { get; set; }

    /// <summary>
    /// 操作是否成功
    /// </summary>
    [ProtoMember(2)]
    public override bool Succeeded { set; get; }

    /// <summary>
    ///     消息
    /// </summary>
    [ProtoMember(3, IsRequired = true)]
    public override string Message { get; set; } = null!;

    /// <summary>
    ///     异常信息
    /// </summary>
    [ProtoMember(4)]
    public override string? Error { get; set; }

    /// <summary>
    /// 本地时间戳
    /// </summary>
    [ProtoMember(5)]
    public override DateTime DateTime { get; set; }

    /// <summary>
    ///     时间戳
    /// </summary>
    [ProtoMember(6)]
    public override long Timestamp { get; set; }

    #endregion

    #region Overrides of ResultBase<T>

    /// <summary>
    /// 描述器
    /// </summary>
    [ProtoMember(7)]
    public override Dictionary<string, Collection<string>>? Descriptor { get; set; }

    #endregion

    /// <summary>
    ///  响应数据项
    /// </summary>
    [ProtoMember(8)]
    public override T? Data { get; set; }
}

/// <summary>
/// RPC共享请求协议
/// </summary>
/// <typeparam name="T"></typeparam>
[ProtoContract]
public class RpcPageRequest<T> : PageRequest<T>
{
    #region Overrides of PageRequest<T>

    /// <summary>
    /// 当前页码(从1开始)
    /// </summary>
    [ProtoMember(1)]
    public override uint Page { get; set; }

    /// <summary>
    /// 页面大小
    /// </summary>
    [ProtoMember(2)]
    public override uint Size { get; set; }

    /// <summary>
    /// 过滤条件
    /// </summary>
    [ProtoMember(3)]
    public override T? FilterCondition { get; set; }

    #endregion
}

/// <summary>
/// 分页响应
/// </summary>
[ProtoContract]
public class RpcPageReply<T> : PageReply<T>
{
    #region Implementation of IPageBase

    /// <summary>
    /// 当前页码(从0开始)
    /// </summary>
    [ProtoMember(1)]
    public override uint Page { get; set; }

    /// <summary>
    /// 页面大小
    /// </summary>
    [ProtoMember(2)]
    public override uint Size { get; set; }

    #endregion

    #region Implementation of IPageReplay

    /// <summary>
    /// 过滤后数据条数
    /// </summary>
    [ProtoMember(3)]
    public override ulong? Count { get; set; }

    /// <summary>
    /// 数据总量
    /// </summary>
    [ProtoMember(4)]
    public override ulong? Total { get; set; }

    #endregion

    #region Overrides of PageReply<T>

    /// <summary>
    /// 数据包
    /// </summary>
    [ProtoMember(5)]
    public override T? Data { get; set; }

    #endregion
}