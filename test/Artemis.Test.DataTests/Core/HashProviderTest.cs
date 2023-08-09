using Artemis.Data.Core;
using Moq;

namespace Artemis.Test.DataTests.Core;

/// <summary>
///     HashProvider测试类
/// </summary>
public class HashProviderTest
{
    /// <summary>
    ///     HashProvider Mock
    /// </summary>
    private Mock<HashProvider> Mock { get; } = new();

    /// <summary>
    ///     Md5哈希方法输入字符串返回期望的哈希值
    /// </summary>
    /// <param name="input">输入</param>
    /// <param name="expected">期望值</param>
    /// <remarks>
    ///     期望值计算来源<![CDATA[https://md5calc.com/]]></remarks>
    [Theory]
    [InlineData("123456", "e10adc3949ba59abbe56e057f20f883e")]
    [InlineData("Artemis", "95630cefed3dc2578222c6219f8e069f")]
    [InlineData("TestString", "5b56f40f8828701f97fa4511ddcd25fb")]
    public void MD5HashInputString_ReturnExpectedValue(string input, string expected)
    {
        var provider = Mock.Object;

        var actual = provider.Md5Hash(input);

        Assert.Equal(actual, expected);
    }

    /// <summary>
    ///     SHA1哈希方法输入字符串返回期望的哈希值
    /// </summary>
    /// <param name="input">输入</param>
    /// <param name="expected">期望值</param>
    /// <remarks>
    ///     期望值计算来源<![CDATA[https://md5calc.com/]]></remarks>
    [Theory]
    [InlineData("123456", "7c4a8d09ca3762af61e59520943dc26494f8941b")]
    [InlineData("Artemis", "2a52519e206546b4ffe31c3383ab4858c17235ff")]
    [InlineData("TestString", "d598b03bee8866ae03b54cb6912efdfef107fd6d")]
    public void SHA1HashInputString_ReturnExpectedValue(string input, string expected)
    {
        var provider = Mock.Object;

        var actual = provider.Sha1Hash(input);

        Assert.Equal(actual, expected);
    }

    /// <summary>
    ///     SHA256哈希方法输入字符串返回期望的哈希值
    /// </summary>
    /// <param name="input">输入</param>
    /// <param name="expected">期望值</param>
    /// <remarks>
    ///     期望值计算来源<![CDATA[https://md5calc.com/]]></remarks>
    [Theory]
    [InlineData("123456", "8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92")]
    [InlineData("Artemis", "e3025e43344bf1fe29f4982090d36a62172a882e28e99dd02dfec2fe7327d96a")]
    [InlineData("TestString", "6dd79f2770a0bb38073b814a5ff000647b37be5abbde71ec9176c6ce0cb32a27")]
    public void SHA256HashInputString_ReturnExpectedValue(string input, string expected)
    {
        var provider = Mock.Object;

        var actual = provider.Sha256Hash(input);

        Assert.Equal(actual, expected);
    }

    /// <summary>
    ///     SHA384哈希方法输入字符串返回期望的哈希值
    /// </summary>
    /// <param name="input">输入</param>
    /// <param name="expected">期望值</param>
    /// <remarks>
    ///     期望值计算来源<![CDATA[https://md5calc.com/]]></remarks>
    [Theory]
    [InlineData("123456",
        "0a989ebc4a77b56a6e2bb7b19d995d185ce44090c13e2984b7ecc6d446d4b61ea9991b76a4c2f04b1b4d244841449454")]
    [InlineData("Artemis",
        "79c97a624f6cdeb78f656127974b5f7ebed0f39cc9b48dec8c9fa15efc35de396d1bd99c7f570e5077b9b7a630034267")]
    [InlineData("TestString",
        "c0a59eced4822f065701ec5abc51531c948864ae84391ec68e80c135d2f3fe50923445e9b436dfa2afdaa7cefa8367bb")]
    public void SHA384HashInputString_ReturnExpectedValue(string input, string expected)
    {
        var provider = Mock.Object;

        var actual = provider.Sha384Hash(input);

        Assert.Equal(actual, expected);
    }

    /// <summary>
    ///     SHA512哈希方法输入字符串返回期望的哈希值
    /// </summary>
    /// <param name="input">输入</param>
    /// <param name="expected">期望值</param>
    /// <remarks>
    ///     期望值计算来源<![CDATA[https://md5calc.com/]]></remarks>
    [Theory]
    [InlineData("123456",
        "ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413")]
    [InlineData("Artemis",
        "920e4a191d3282d8f49e94730175b0a00111afd3ce047118637e4b1a9494e57fa9b5c877afc07e0d249c4917f66e3bed342719fd1bd596bc20b77545b423e80b")]
    [InlineData("TestString",
        "69dfd91314578f7f329939a7ea6be4497e6fe3909b9c8f308fe711d29d4340d90d77b7fdf359b7d0dbeed940665274f7ca514cd067895fdf59de0cf142b62336")]
    public void SHA512HashInputString_ReturnExpectedValue(string input, string expected)
    {
        var provider = Mock.Object;

        var actual = provider.Sha512Hash(input);

        Assert.Equal(actual, expected);
    }
}