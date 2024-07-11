using Artemis.Data.Core.Fundamental.Model;

namespace Artemis.Service.Shared.Identity;

/// <summary>
///     用户档案接口
/// </summary>
public interface IUserProfile : IMetadata
{
    /// <summary>
    ///     用户标识
    /// </summary>
    Guid UserId { get; set; }
}