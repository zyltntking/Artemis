using Artemis.Shared.Identity;
using ProtoBuf;
using ProtoBuf.Grpc.Reflection;

[assembly: CompatibilityLevel(CompatibilityLevel.Level200)]
namespace Artemis.Shared.ProtoGenerator
{
    /// <summary>
    /// Program
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        static async Task Main(string[] args)
        {
            var generator = new SchemaGenerator();


            var schema = generator.GetSchema<IAccount>();

            await using var writer = new StreamWriter("account.proto");
                
            await writer.WriteAsync(schema);

            Console.WriteLine("Hello, World!");
        }
    }
}