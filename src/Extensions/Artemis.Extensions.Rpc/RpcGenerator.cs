using ProtoBuf.Grpc.Reflection;
using ProtoBuf.Meta;

namespace Artemis.Extensions.Rpc;

/// <summary>
/// 生成器
/// </summary>
public static class RpcGenerator
{
    /// <summary>
    /// 生成协议缓冲文件
    /// </summary>
    /// <typeparam name="TSharedService">分享协议定义</typeparam>
    /// <param name="path">生成路径</param>
    /// <returns></returns>
    public static async Task ProtocolBuffer<TSharedService>(string path)
    {
        var generator = new SchemaGenerator
        {
            ProtoSyntax = ProtoSyntax.Proto3
        };

        var schema = generator.GetSchema<TSharedService>();

        await using var writer = new StreamWriter(path);

        await writer.WriteAsync(schema);
    }
}