using Artemis.Service.Business.VisionScreen.Context.Configuration;
using Artemis.Service.Business.VisionScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Context;

/// <summary>
/// 视力档案记录
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisVisionScreenRecordConfiguration))]
public class ArtemisVisionScreenRecord : VisionScreenRecord
{
    /// <summary>
    /// 该档案的验光仪数据历史记录
    /// </summary>
    public ICollection<ArtemisOptometer> Optometers { get; set; }

    /// <summary>
    /// 该档案的视力表数据历史记录
    /// </summary>
    public ICollection<ArtemisVisualChart> VisualCharts { get; set; }
}