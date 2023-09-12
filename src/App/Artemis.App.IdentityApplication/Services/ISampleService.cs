using ProtoBuf;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace Artemis.App.IdentityApplication.Services;

[Service("Hyper.Sample")]
public interface ISampleService
{
    ValueTask<Foo> Test(Bar request, CallContext context = default);
}

[ProtoContract]
public class Foo
{
    [ProtoMember(1)]
    public int Id { get; set; }
    [ProtoMember(2)]
    public string Name { get; set; }
    [ProtoMember(3)]
    public double Value { get; set; }
    [ProtoMember(4)]
    public string Description { get; set; }
}

[ProtoContract]
public class Bar
{
    [ProtoMember(1)]
    public string Name { get; set; }
}
