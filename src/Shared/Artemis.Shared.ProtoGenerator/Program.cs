using Artemis.Shared.Identity;
using ProtoBuf.Grpc.Reflection;
using ProtoBuf.Meta;

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
            ProtoSyntax = ProtoSyntax.Proto3
        };

        var schema = generator.GetSchema<IRoleService>();

        await using var writer = new StreamWriter("account.proto");

        await writer.WriteAsync(schema);

        Console.WriteLine("Hello, World!");
    }
}