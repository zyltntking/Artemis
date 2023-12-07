using Artemis.Data.Core.Socket;

namespace Artemis.Test.DataTest;

/// <summary>
///     测试代码片段
/// </summary>
public class SegmentTests
{
    [Fact]
    public void Test()
    {
        var str = AsciiCharacter.RandomNonAlphanumeric(8);
    }
}