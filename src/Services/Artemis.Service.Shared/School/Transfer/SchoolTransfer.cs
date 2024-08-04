namespace Artemis.Service.Shared.School.Transfer;

/// <summary>
///     学校信息
/// </summary>
public record SchoolInfo : SchoolPackage, ISchoolInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
///     学校数据包
/// </summary>
public record SchoolPackage : ISchoolPackage
{
    /// <summary>
    ///     学校名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     学校编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    ///     绑定标记
    /// </summary>
    public string? BindingTag { get; set; }

    /// <summary>
    ///     学校类型
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    ///     组织机构代码
    /// </summary>
    public string? OrganizationCode { get; set; }

    /// <summary>
    ///     学校所在地行政区划代码
    /// </summary>
    public string? DivisionCode { get; set; }

    /// <summary>
    ///     学校地址
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    ///     学校邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     学校网站
    /// </summary>
    public string? WebSite { get; set; }

    /// <summary>
    ///     学校联系电话
    /// </summary>
    public string? ContactNumber { get; set; }

    /// <summary>
    ///     学校建立时间
    /// </summary>
    public DateTime? EstablishTime { get; set; }

    /// <summary>
    ///     学校简介
    /// </summary>
    public string? Introduction { get; set; }

    /// <summary>
    ///     备注
    /// </summary>
    public string? Remark { get; set; }
}