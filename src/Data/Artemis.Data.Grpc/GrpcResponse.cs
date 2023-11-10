using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Artemis.Data.Core;

namespace Artemis.Data.Grpc;

/// <summary>
///     GRPC响应对象
/// </summary>
/// <typeparam name="T"></typeparam>
[DataContract]
public abstract record GrpcResponse<T>
{
    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required GrpcResult Result { get; set; }

    /// <summary>
    ///     结果数据
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public virtual required T? Data { get; set; }
}

/// <summary>
///     GRPC空响应对象
/// </summary>
[DataContract]
public sealed record GrpcEmptyResponse : GrpcResponse<EmptyRecord>
{
    #region Overrides of GrpcResponse<EmptyRecord>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcResult Result { get; set; }

    /// <summary>
    ///     结果数据
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required EmptyRecord? Data { get; set; }

    #endregion
}

/// <summary>
///     GRPC分页响应对象
/// </summary>
/// <typeparam name="T"></typeparam>
[DataContract]
public abstract record GrpcPageResponse<T>
{
    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required GrpcResult Result { get; set; }

    /// <summary>
    ///     分页信息
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public virtual required GrpcPageResult Page { get; set; }

    /// <summary>
    ///     数据集
    /// </summary>
    [DataMember(Order = 3)]
    public virtual required IEnumerable<T>? Data { get; set; }
}

/// <summary>
///     Grpc响应辅助函数
/// </summary>
public static class GrpcResponse
{
    /// <summary>
    ///     生成数据结果
    /// </summary>
    /// <param name="code">结果状态编码</param>
    /// <param name="message">结果消息</param>
    /// <param name="exception"></param>
    /// <returns>数据结果</returns>
    private static GrpcResult GenerateResult(int code, string message, Exception? exception = null)
    {
        return new GrpcResult
        {
            Code = code,
            Message = message,
            Error = exception?.ToString(),
            DateTime = DateTime.Now.ToLocalTime(),
            Timestamp = DateTime.Now.ToUnixTimeStamp()
        };
    }

    #region Result

    /// <summary>
    ///     生成成功结果
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>成功结果</returns>
    public static GrpcResult SuccessResult(string message = "成功")
    {
        return GenerateResult(ResultStatus.Success, message);
    }

    /// <summary>
    ///     生成失败结果
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>失败结果</returns>
    public static GrpcResult FailResult(string message = "失败")
    {
        return GenerateResult(ResultStatus.Fail, message);
    }

    /// <summary>
    ///     生成异常结果
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="code">结果编码</param>
    /// <param name="message">消息</param>
    /// <returns>异常结果</returns>
    public static GrpcResult ExceptionResult(Exception exception, int code = ResultStatus.Exception,
        string message = "异常")
    {
        return GenerateResult(code, message, exception);
    }

    #endregion

    #region EmptyResponse

    /// <summary>
    ///     生成成功结果
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>成功结果</returns>
    public static GrpcEmptyResponse EmptySuccess(string message = "成功")
    {
        return new GrpcEmptyResponse
        {
            Result = SuccessResult(message),
            Data = new EmptyRecord()
        };
    }

    /// <summary>
    ///     生成失败结果
    /// </summary>
    /// <param name="message">消息</param>
    /// <returns>失败结果</returns>
    public static GrpcEmptyResponse EmptyFail(string message = "失败")
    {
        return new GrpcEmptyResponse
        {
            Result = FailResult(message),
            Data = new EmptyRecord()
        };
    }

    /// <summary>
    ///     生成异常结果
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="code">结果编码</param>
    /// <param name="message">消息</param>
    /// <returns>异常结果</returns>
    public static GrpcEmptyResponse EmptyException(Exception exception, int code = ResultStatus.Exception,
        string message = "异常")
    {
        return new GrpcEmptyResponse
        {
            Result = ExceptionResult(exception, code, message),
            Data = new EmptyRecord()
        };
    }

    #endregion

    #region Page

    /// <summary>
    ///     分页结果
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static GrpcPageResult PageResult(AbstractPageResult result)
    {
        return PageResult(result.Page, result.Size, result.Count, result.Total);
    }

    /// <summary>
    ///     分页结果
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="count"></param>
    /// <param name="total"></param>
    /// <returns></returns>
    private static GrpcPageResult PageResult(int page, int size, long count, long total)
    {
        return new GrpcPageResult
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total
        };
    }

    #endregion
}