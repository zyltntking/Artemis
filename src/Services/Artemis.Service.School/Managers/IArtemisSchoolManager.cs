using Artemis.Data.Store;
using Artemis.Service.School.Context;

namespace Artemis.Service.School.Managers;

/// <summary>
///     学校管理接口
/// </summary>
public interface IArtemisSchoolManager : IManager<ArtemisSchool, Guid, Guid>
{
}