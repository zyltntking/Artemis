using Artemis.Service.Business.VisionScreen.Context.Configuration;
using Artemis.Service.Business.VisionScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Context;

/// <summary>
/// 学生眼部照片模型
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisStudentEyePhotoConfiguration))]
public sealed class ArtemisStudentEyePhoto : StudentEyePhoto
{
    
}