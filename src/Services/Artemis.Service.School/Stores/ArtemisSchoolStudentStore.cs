using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Artemis.Data.Store;
using Artemis.Service.School.Context;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.School.Stores;

#region Interface

/// <summary>
///     学校学生关系存储接口
/// </summary>
public interface IArtemisSchoolStudentStore : IKeyLessStore<ArtemisSchoolStudent>;

#endregion

/// <summary>
///     学校学生关系存储
/// </summary>
public sealed class ArtemisSchoolStudentStore : KeyLessStore<ArtemisSchoolStudent>, IArtemisSchoolStudentStore
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
    public ArtemisSchoolStudentStore(
        SchoolContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        ICacheProxy? cacheProxy = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy, cacheProxy, logger,
        describer)
    {
    }

    /// <summary>
    ///     键生成委托
    /// </summary>
    protected override Func<ArtemisSchoolStudent, string>? EntityKey { get; init; } =
        schoolStudent => $"{schoolStudent.SchoolId}:{schoolStudent.StudentId}";
}