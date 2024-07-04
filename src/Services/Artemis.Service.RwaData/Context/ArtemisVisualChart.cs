using Artemis.Service.RwaData.Context.Configuration;
using Artemis.Service.RwaData.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.RwaData.Context;

/// <summary>
/// 视力表数据
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisVisualChartConfiguration))]
public class ArtemisVisualChart : VisualChart
{
    
}