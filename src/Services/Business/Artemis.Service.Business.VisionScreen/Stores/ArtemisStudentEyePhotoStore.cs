using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Service.Business.VisionScreen.Context;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Business.VisionScreen.Stores;

#region Interface

/// <summary>
///     学生眼部照片存储接口
/// </summary>
public interface IArtemisStudentEyePhotoStore : IStore<ArtemisStudentEyePhoto>;

#endregion

/// <summary>
///     学生眼部照片存储
/// </summary>
public class ArtemisStudentEyePhotoStore : Store<ArtemisStudentEyePhoto>, IArtemisStudentEyePhotoStore
{
    /// <summary>
    ///     基本存储实例构造
    /// </summary>
    /// <param name="context"></param>
    /// <param name="storeOptions"></param>
    /// <param name="handlerProxy"></param>
    /// <param name="cacheProxy"></param>
    /// <param name="logger"></param>
    /// <param name="describer"></param>
    public ArtemisStudentEyePhotoStore(
        BusinessContext context,
        IStoreOptions? storeOptions = null,
        IHandlerProxy? handlerProxy = null,
        ICacheProxy? cacheProxy = null,
        ILogger? logger = null,
        StoreErrorDescriber? describer = null) : base(context, storeOptions, handlerProxy, cacheProxy, logger, describer)
    {
    }
}