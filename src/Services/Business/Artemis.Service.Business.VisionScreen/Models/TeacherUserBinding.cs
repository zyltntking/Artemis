using Artemis.Data.Core;
using Artemis.Service.Shared.Business.VisionScreen;

namespace Artemis.Service.Business.VisionScreen.Models;

/// <summary>
/// 教师用户绑定模型
/// </summary>
public class TeacherUserBinding : ConcurrencyPartition, ITeacherUserBinding
{
    #region Implementation of ITeacherUserBindingPackage

    /// <summary>
    /// 用户标识
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 教师标识
    /// </summary>
    public Guid TeacherId { get; set; }

    #endregion
}