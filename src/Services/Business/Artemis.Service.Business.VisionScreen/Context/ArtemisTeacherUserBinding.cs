using Artemis.Service.Business.VisionScreen.Context.Configuration;
using Artemis.Service.Business.VisionScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Context;

/// <summary>
/// 教师用户绑定关系数据模型
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisTeacherUserBindingConfiguration))]
public sealed class ArtemisTeacherUserBinding : TeacherUserBinding
{
    
}