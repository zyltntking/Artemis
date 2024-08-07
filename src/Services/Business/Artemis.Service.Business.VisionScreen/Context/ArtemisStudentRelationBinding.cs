using Artemis.Service.Business.VisionScreen.Context.Configuration;
using Artemis.Service.Business.VisionScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Context;

/// <summary>
/// 学生用户亲属关系绑定模型
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisStudentRelationBindingConfiguration))]
public sealed class ArtemisStudentRelationBinding : StudentRelationBinding
{
    
}