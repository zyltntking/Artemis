using Artemis.Service.RawData.Context.Configuration;
using Artemis.Service.RawData.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.RawData.Context;

/// <summary>
///     视力表数据
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisVisualChartConfiguration))]
public class ArtemisVisualChart : VisualChart
{
}