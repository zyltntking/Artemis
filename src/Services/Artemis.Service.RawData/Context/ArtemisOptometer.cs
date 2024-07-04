using Artemis.Service.RawData.Context.Configuration;
using Artemis.Service.RawData.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.RawData.Context;

/// <summary>
///     验光仪数据
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisOptometerConfiguration))]
public sealed class ArtemisOptometer : Optometer
{
}