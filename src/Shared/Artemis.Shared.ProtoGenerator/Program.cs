using Artemis.Shared.Identity.Services;
using ProtoBuf;
using ProtoBuf.Grpc.Configuration;
using ProtoBuf.Grpc.Reflection;
using ProtoBuf.Meta;

[module: CompatibilityLevel(CompatibilityLevel.Level300)]

namespace Artemis.Shared.ProtoGenerator;

/// <summary>
///     Program
/// </summary>
internal static class Program
{
    /// <summary>
    ///     Main
    /// </summary>
    /// <param name="args"></param>
    private static async Task Main(string[] args)
    {
        var generator = new SchemaGenerator
        {
            ProtoSyntax = ProtoSyntax.Proto3,
        };

        var schema = generator.GetSchema<IRoleService>();

        await using var roleWriter = new StreamWriter("../../../protos/RoleService.proto");

        await roleWriter.WriteAsync(schema);

        schema = generator.GetSchema<IUserService>();

        await using var userWriter = new StreamWriter("../../../protos/UserService.proto");

        await userWriter.WriteAsync(schema);

        schema = generator.GetSchema<IAccountService>();

        await using var accountWriter = new StreamWriter("../../../protos/AccountService.proto");

        await accountWriter.WriteAsync(schema);


        Console.WriteLine("Hello, World!");
    }
}