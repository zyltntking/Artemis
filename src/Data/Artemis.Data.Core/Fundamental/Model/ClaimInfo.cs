namespace Artemis.Data.Core.Fundamental.Model;

#region Interface

/// <summary>
///     凭据项目接口
/// </summary>
file interface IClaim : ICheckStamp
{
    /// <summary>
    ///     凭据类型
    /// </summary>
    string Type { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    string Value { get; set; }

}

#endregion

/// <summary>
/// 凭据信息
/// </summary>
public record ClaimInfo : IClaim
{
    #region Implementation of ICheckStamp

    /// <summary>
    ///     校验戳
    /// </summary>
    public required string CheckStamp { get; set; }

    #endregion

    #region Implementation of IClaim

    /// <summary>
    ///     凭据类型
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    public required string Value { get; set; }

    #endregion
}