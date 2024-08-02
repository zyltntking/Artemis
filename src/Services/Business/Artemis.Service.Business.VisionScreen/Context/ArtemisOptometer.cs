using Artemis.Service.Business.VisionScreen.Context.Configuration;
using Artemis.Service.Business.VisionScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Context;

/// <summary>
///     验光仪数据
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisOptometerConfiguration))]
public sealed class ArtemisOptometer : Optometer
{
}