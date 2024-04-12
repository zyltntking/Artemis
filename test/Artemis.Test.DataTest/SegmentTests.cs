using System.Text.RegularExpressions;

namespace Artemis.Test.DataTest;

/// <summary>
///     测试代码片段
/// </summary>
public class SegmentTests
{
    [Fact]
    public void Test()
    {
        var cb = @"AQMYAAAH/QAEAAACjgAAAAA97AJGAAAAADuiJZA=";

        var dd = Convert.FromBase64String(cb);

        var regex = new Regex(
            @"^(?:(?:\+|00)86)?1(?:(?:3[\d])|(?:4[5-79])|(?:5[0-35-9])|(?:6[5-7])|(?:7[0-8])|(?:8[\d])|(?:9[01256789]))\d{8}$");

        var input = "13888888888";

        var bb = regex.IsMatch(input);
    }
}