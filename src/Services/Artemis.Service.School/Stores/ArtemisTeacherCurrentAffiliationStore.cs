using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Service.School.Context;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.School.Stores;

#region Interface

/// <summary>
///     教师当前所属关系存储接口
/// </summary>
public interface IArtemisTeacherCurrentAffiliationStore : IKeyLessStore<ArtemisTeacherCurrentAffiliation>;

#endregion

/// <summary>
///     教师当前所属关系存储
/// </summary>
public class ArtemisTeacherCurrentAffiliationStore : KeyLessStore<ArtemisTeacherCurrentAffiliation>,
    IArtemisTeacherCurrentAffiliationStore
{
    /// <summary>
    ///     无键模型基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="handlerProxy"></param>
    /// <param name="cacheProxy"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    public ArtemisTeacherCurrentAffiliationStore(
        SchoolContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        ICacheProxy? cacheProxy = null,
        ILogger? logger = null, StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy,
        cacheProxy, logger, describer)
    {
    }

    /// <summary>
    ///     键生成委托
    /// </summary>
    protected override Func<ArtemisTeacherCurrentAffiliation, string>? EntityKey { get; init; } =
        teacherCurrentAffiliation =>
            $"{teacherCurrentAffiliation.SchoolId}:{teacherCurrentAffiliation.ClassId}:{teacherCurrentAffiliation.TeacherId}";
}