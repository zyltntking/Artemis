using Artemis.App.IdentityApplication.Protocols;
using ProtoBuf.Grpc;

namespace Artemis.App.IdentityApplication.Services;

public class SampleService : ISampleService
{
    #region Implementation of ISampleService

    public ValueTask<Foo> Test(Bar request, CallContext context = default)
    {
        var foo = new Foo
        {
            Description = "abc",
            Id = 999,
            Name = "zhaoy",
            Value = 0.2
        };

        return new ValueTask<Foo>(foo);
    }

    #endregion
}