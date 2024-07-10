using Artemis.Data.Store;
using Artemis.Service.RawData.Stores;
using Microsoft.Extensions.Logging;

namespace Artemis.Service.RawData.Managers;

/// <summary>
///     原始数据管理器
/// </summary>
public class ArtemisRawDataManager : Manager, IArtemisRawDataManager
{
    /// <summary>
    ///     创建新的管理器实例
    /// </summary>
    /// <param name="optometerStore">存储访问器依赖</param>
    /// <param name="visualChartStore"></param>
    /// <param name="options">配置依赖</param>
    /// <param name="logger">日志依赖</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArtemisRawDataManager(
        IArtemisOptometerStore optometerStore,
        IArtemisVisualChartStore visualChartStore,
        IManagerOptions? options = null,
        ILogger? logger = null) : base(options, logger)
    {
        OptometerStore = optometerStore ?? throw new ArgumentNullException(nameof(optometerStore));
        VisualChartStore = visualChartStore ?? throw new ArgumentNullException(nameof(visualChartStore));
    }

    #region Overrides of KeyLessManager<ArtemisOptometer,Guid>

    /// <summary>
    ///     释放托管的Store
    /// </summary>
    protected override void StoreDispose()
    {
        OptometerStore.Dispose();
        VisualChartStore.Dispose();
    }

    #endregion

    #region StoreAccess

    /// <summary>
    ///     验光仪存储访问器
    /// </summary>
    private IArtemisOptometerStore OptometerStore { get; }

    /// <summary>
    ///     视力表存储访问器
    /// </summary>
    private IArtemisVisualChartStore VisualChartStore { get; }

    #endregion
}