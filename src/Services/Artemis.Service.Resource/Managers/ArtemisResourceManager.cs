using Artemis.Data.Store;
using Artemis.Service.Resource.Context;
using Artemis.Service.Resource.Stores;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.Resource.Managers;

/// <summary>
/// 资源管理器
/// </summary>
public class ArtemisResourceManager : Manager<ArtemisDevice>, IArtemisResourceManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="deviceStore">存储访问器依赖</param>
    /// <param name="organizationStore"></param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <param name="dataDictionaryStore"></param>
    /// <param name="dataDictionaryItemStore"></param>
    /// <param name="divisionStore"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisResourceManager(
        IArtemisDeviceStore deviceStore, 
        IArtemisDataDictionaryStore dataDictionaryStore,
        IArtemisDataDictionaryItemStore dataDictionaryItemStore,
        IArtemisDivisionStore divisionStore,
        IArtemisOrganizationStore organizationStore,
        IManagerOptions? options = null, 
        ILogger? logger = null) : base(deviceStore, options, logger)
    {
        DeviceStore = deviceStore ?? throw new ArgumentNullException(nameof(deviceStore));
        DataDictionaryStore = dataDictionaryStore ?? throw new ArgumentNullException(nameof(dataDictionaryStore));
        DataDictionaryItemStore = dataDictionaryItemStore ?? throw new ArgumentNullException(nameof(dataDictionaryItemStore));
        DivisionStore = divisionStore ?? throw new ArgumentNullException(nameof(divisionStore));
        OrganizationStore = organizationStore ?? throw new ArgumentNullException(nameof(organizationStore));
    }

    #region Overrides of KeyLessManager<ArtemisDevice,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        DeviceStore.Dispose();
        DataDictionaryStore.Dispose();
        DataDictionaryItemStore.Dispose();
        DivisionStore.Dispose();
        OrganizationStore.Dispose();
    }

    #endregion

    #region StoreAccess

    /// <summary>
    /// 设备存储访问器
    /// </summary>
    private IArtemisDeviceStore DeviceStore { get; }

    /// <summary>
    /// 数据字典存储访问器
    /// </summary>
    private IArtemisDataDictionaryStore DataDictionaryStore { get; }

    /// <summary>
    /// 数据字典项存储访问器
    /// </summary>
    private IArtemisDataDictionaryItemStore DataDictionaryItemStore { get; }

    /// <summary>
    /// 行政区划存储访问器
    /// </summary>
    private IArtemisDivisionStore DivisionStore { get; }

    /// <summary>
    /// 组织机构存储访问器
    /// </summary>
    private IArtemisOrganizationStore OrganizationStore { get; }

    #endregion
}