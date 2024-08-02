using Artemis.Service.Business.VisionScreen.Context.Configuration;
using Artemis.Service.Business.VisionScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Context;

/// <summary>
///     视力表数据
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisVisualChartConfiguration))]
public class ArtemisVisualChart : VisualChart
{
}