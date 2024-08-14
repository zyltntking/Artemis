using Artemis.Service.Business.VisionScreen.Context.Configuration;
using Artemis.Service.Business.VisionScreen.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Business.VisionScreen.Context;

/// <summary>
/// 机构用户绑定实体
/// </summary>
[EntityTypeConfiguration(typeof(ArtemisOrganizationUsersBindingConfiguration))]
public sealed class ArtemisOrganizationUsersBinding : OrganizationUsersBinding
{
    
}