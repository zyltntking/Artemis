using Artemis.Service.Business.VisionScreen.Context.Configuration;
using Artemis.Service.Business.VisionScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Context;

/// <summary>
/// 通知消息实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisNotificationMessageConfiguration))]
public sealed class ArtemisNotificationMessage : NotificationMessage
{

}