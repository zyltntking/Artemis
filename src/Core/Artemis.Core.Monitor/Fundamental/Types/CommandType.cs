using System.ComponentModel;
using Artemis.Data.Core.Fundamental;

namespace Artemis.Core.Monitor.Fundamental.Types;

/// <summary>
///     指令类型
/// </summary>
[Description("指令类型")]
public class CommandType : Enumeration
{
    /// <summary>
    ///     未知类型
    /// </summary>
    public static CommandType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     bash脚本
    /// </summary>
    public static CommandType BashScript = new(0, nameof(BashScript));

    /// <summary>
    ///     Cmd脚本
    /// </summary>
    public static CommandType CmdScript = new(1, nameof(CmdScript));

    /// <summary>
    ///     PowerShell脚本
    /// </summary>
    public static CommandType PowerShellScript = new(2, nameof(PowerShellScript));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private CommandType(int id, string name) : base(id, name)
    {
    }
}