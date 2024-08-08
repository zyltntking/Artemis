using Artemis.Service.Business.VisionScreen.Context.Configuration;
using Artemis.Service.Business.VisionScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Context;

/// <summary>
/// 视力档案记录反馈
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisRecordFeedbackConfiguration))]
public sealed class ArtemisRecordFeedback : RecordFeedback
{
    /// <summary>
    /// 反馈所属的视力档案
    /// </summary>
    public ArtemisVisionScreenRecord VisionScreenRecord { get; set; }
}