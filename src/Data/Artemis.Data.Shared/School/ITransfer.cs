namespace Artemis.Data.Shared.School;

/// <summary>
/// 流转接口
/// </summary>
public interface ITransfer
{
    /// <summary>
    /// 转入时间
    /// </summary>
    DateTime MoveIn { get; set; }

    /// <summary>
    /// 转出时间
    /// </summary>
    DateTime? MoveOut { get; set; }
}