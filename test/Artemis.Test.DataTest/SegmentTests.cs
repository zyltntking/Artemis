﻿using System.Net.Sockets;
using Artemis.Data.Core.AscII;

namespace Artemis.Test.DataTest;

/// <summary>
///     测试代码片段
/// </summary>
public class SegmentTests
{
    [Fact]
    public void Test()
    {
        var bb = Convert.FromBase64String("AQUAAP8AjDo=");

        var str = AsciiCharacter.RandomNonAlphanumeric(8);

        var client = new TcpClient();

        var stream = client.GetStream();
    }
}