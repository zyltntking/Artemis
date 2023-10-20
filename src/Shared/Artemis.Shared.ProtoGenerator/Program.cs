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
        var binderConfiguration = BinderConfiguration.Create();

        var generator = new SchemaGenerator
        {
            ProtoSyntax = ProtoSyntax.Proto3,
           
        };

        var schema = generator.GetSchema<IRoleService>();

        await using var writer = new StreamWriter("role.proto");

        await writer.WriteAsync(schema);

        Console.WriteLine("Hello, World!");
    }
}