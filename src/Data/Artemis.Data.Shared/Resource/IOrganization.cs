namespace Artemis.Data.Shared.Resource;

/// <summary>
///     组织机构接口
/// </summary>
public interface IOrganization
{
    /// <summary>
    ///     机构名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     机构类型
    /// </summary>
    string Type { get; set; }

    /// <summary>
    ///     机构邮箱
    /// </summary>
    string? Email { get; set; }

    /// <summary>
    ///     机构邮编
    /// </summary>
    string? PostCode { get; set; }

    /// <summary>
    ///     机构状态
    /// </summary>
    string Status { get; set; }

    /// <summary>
    ///     机构地址
    /// </summary>
    string? Address { get; set; }

    /// <summary>
    ///     机构描述
    /// </summary>
    string? Description { get; set; }
}