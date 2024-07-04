using Artemis.Data.Core;
using Artemis.Data.Shared.RawData;

namespace Artemis.Service.RawData.Models;

/// <summary>
///     筛查仪数据
/// </summary>
public class Optometer : ConcurrencyPartition, IOptometer
{
}