using Artemis.Extensions.Rpc;
using Artemis.Extensions.Web.Serilog;
using Artemis.Shared.Identity.Services;

namespace Artemis.App.Identity;

/// <summary>
///     Program
/// </summary>
public static class Program
{
    /// <summary>
    ///     Main
    /// </summary>
    /// <param name="args"></param>
    public static async Task Main(string[] args)
    {
        await RpcGenerator.ProtocolBuffer<IRoleService>("./protos/RoleService.proto");
        await RpcGenerator.ProtocolBuffer<IUserService>("./protos/UserService.proto");
        await RpcGenerator.ProtocolBuffer<IAccountService>("./protos/AccountService.proto");

        LogHost.CreateWebApp<Startup>(args);
    }
}