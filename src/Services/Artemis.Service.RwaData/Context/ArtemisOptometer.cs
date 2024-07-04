using Artemis.Service.RwaData.Context.Configuration;
using Artemis.Service.RwaData.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.RwaData.Context;

/// <summary>
///     验光仪数据
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisOptometerConfiguration))]
public sealed class ArtemisOptometer : Optometer
{
}