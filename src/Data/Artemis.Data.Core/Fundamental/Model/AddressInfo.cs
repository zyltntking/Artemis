namespace Artemis.Data.Core.Fundamental.Model;

#region Interface

/// <summary>
///     地址接口
/// </summary>
file interface IAddress
{
    /// <summary>
    ///     国家
    /// </summary>
    string? State { get; set; }

    /// <summary>
    ///     省
    /// </summary>
    string? Province { get; set; }

    /// <summary>
    ///     地
    /// </summary>
    string? Prefecture { get; set; }

    /// <summary>
    ///     县
    /// </summary>
    string? County { get; set; }

    /// <summary>
    ///     乡
    /// </summary>
    string? Township { get; set; }

    /// <summary>
    ///    村
    /// </summary>
    string? Village { get; set; }

    /// <summary>
    /// 门牌号
    /// </summary>
    string? HouseNumber { get; set; }
}

file interface INormalizedAddress
{
    /// <summary>
    ///     国家
    /// </summary>
    string? NormalizedState { get; set; }

    /// <summary>
    ///     省
    /// </summary>
    string? NormalizedProvince { get; set; }

    /// <summary>
    ///     地
    /// </summary>
    string? NormalizedPrefecture { get; set; }

    /// <summary>
    ///     县
    /// </summary>
    string? NormalizedCounty { get; set; }

    /// <summary>
    ///     乡
    /// </summary>
    string? NormalizedTownship { get; set; }

    /// <summary>
    /// 村
    /// </summary>
    string? NormalizedVillage { get; set; }

    /// <summary>
    /// 门牌号
    /// </summary>
    string? NormalizedHouseNumber { get; set; }
}

#endregion

/// <summary>
///     地址信息
/// </summary>
public abstract class AddressInfo : IAddress
{
    #region Overrides of Object

    /// <summary>
    ///     Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        var segments = new List<string>();

        if (!string.IsNullOrWhiteSpace(State))
            segments.Add(State);

        if (!string.IsNullOrWhiteSpace(Province))
            segments.Add(Province);

        if (!string.IsNullOrWhiteSpace(Prefecture))
            segments.Add(Prefecture);

        if (!string.IsNullOrWhiteSpace(County))
            segments.Add(County);

        if (!string.IsNullOrWhiteSpace(Township))
            segments.Add(Township);

        if (!string.IsNullOrWhiteSpace(Village))
            segments.Add(Village);

        if (!string.IsNullOrWhiteSpace(HouseNumber))
            segments.Add(HouseNumber);

        var address = string.Join(" ", segments);

        return address;
    }

    #endregion

    #region Implementation of IAddress

    /// <summary>
    ///     国家
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    ///     省
    /// </summary>
    public string? Province { get; set; }

    /// <summary>
    ///     地
    /// </summary>
    public string? Prefecture { get; set; }

    /// <summary>
    ///     县
    /// </summary>
    public string? County { get; set; }

    /// <summary>
    ///     乡
    /// </summary>
    public string? Township { get; set; }

    /// <summary>
    ///    村
    /// </summary>
    public string? Village { get; set; }

    /// <summary>
    /// 门牌号
    /// </summary>
    public string? HouseNumber { get; set; }

    #endregion
}

/// <summary>
///     地址结构
/// </summary>
public abstract class AddressStructure : AddressInfo, INormalizedAddress
{
    #region Implementation of INormalizedAddress

    /// <summary>
    ///     国家
    /// </summary>
    public string? NormalizedState { get; set; }

    /// <summary>
    ///     省
    /// </summary>
    public string? NormalizedProvince { get; set; }

    /// <summary>
    ///     地
    /// </summary>
    public string? NormalizedPrefecture { get; set; }

    /// <summary>
    ///     县
    /// </summary>
    public string? NormalizedCounty { get; set; }

    /// <summary>
    ///     乡
    /// </summary>
    public string? NormalizedTownship { get; set; }

    /// <summary>
    /// 村
    /// </summary>
    public string? NormalizedVillage { get; set; }

    /// <summary>
    /// 门牌号
    /// </summary>
    public string? NormalizedHouseNumber { get; set; }

    #endregion
}