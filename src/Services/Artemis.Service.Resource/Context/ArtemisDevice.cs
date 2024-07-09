using Artemis.Service.Resource.Context.Configuration;
using Artemis.Service.Resource.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Context;

/// <summary>
///     设备实体配置
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisDeviceConfiguration))]
public sealed class ArtemisDevice : Device
{
}