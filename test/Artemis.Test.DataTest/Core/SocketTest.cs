using Artemis.Data.Core.AscII;

namespace Artemis.Test.DataTest.Core;

/// <summary>
///     Socket测试
/// </summary>
public class SocketTest
{
    [Theory]
    [InlineData('0', true)]
    [InlineData('9', true)]
    [InlineData('a', true)]
    [InlineData('z', true)]
    [InlineData('A', true)]
    [InlineData('Z', true)]
    [InlineData('!', true)]
    [InlineData('~', true)]
    [InlineData(' ', false)]
    public void CharacterShouldVerify(char character, bool expect)
    {
        var verify = AsciiCharacter.IsCharacter(character);

        Assert.Equal(expect, verify);
    }

    [Theory]
    [InlineData('0', true)]
    [InlineData('9', true)]
    [InlineData('a', false)]
    [InlineData('z', false)]
    [InlineData('A', false)]
    [InlineData('Z', false)]
    [InlineData('!', false)]
    [InlineData('~', false)]
    [InlineData(' ', false)]
    public void DigitShouldVerify(char character, bool expect)
    {
        var verify = AsciiCharacter.IsDigit(character);

        Assert.Equal(expect, verify);
    }

    [Theory]
    [InlineData('0', false)]
    [InlineData('9', false)]
    [InlineData('a', false)]
    [InlineData('z', false)]
    [InlineData('A', true)]
    [InlineData('Z', true)]
    [InlineData('!', false)]
    [InlineData('~', false)]
    [InlineData(' ', false)]
    public void UpperCaseShouldVerify(char character, bool expect)
    {
        var verify = AsciiCharacter.IsUpperCase(character);

        Assert.Equal(expect, verify);
    }

    [Theory]
    [InlineData('0', false)]
    [InlineData('9', false)]
    [InlineData('a', true)]
    [InlineData('z', true)]
    [InlineData('A', false)]
    [InlineData('Z', false)]
    [InlineData('!', false)]
    [InlineData('~', false)]
    [InlineData(' ', false)]
    public void LowCaseShouldVerify(char character, bool expect)
    {
        var verify = AsciiCharacter.IsLowerCase(character);

        Assert.Equal(expect, verify);
    }

    [Theory]
    [InlineData('0', false)]
    [InlineData('9', false)]
    [InlineData('a', true)]
    [InlineData('z', true)]
    [InlineData('A', true)]
    [InlineData('Z', true)]
    [InlineData('!', false)]
    [InlineData('~', false)]
    [InlineData(' ', false)]
    public void LetterShouldVerify(char character, bool expect)
    {
        var verify = AsciiCharacter.IsLetter(character);

        Assert.Equal(expect, verify);
    }

    [Theory]
    [InlineData('0', true)]
    [InlineData('9', true)]
    [InlineData('a', true)]
    [InlineData('z', true)]
    [InlineData('A', true)]
    [InlineData('Z', true)]
    [InlineData('!', false)]
    [InlineData('~', false)]
    [InlineData(' ', false)]
    public void LetterOrDigitShouldVerify(char character, bool expect)
    {
        var verify = AsciiCharacter.IsLetterOrDigit(character);

        Assert.Equal(expect, verify);
    }

    [Theory]
    [InlineData('0', false)]
    [InlineData('9', false)]
    [InlineData('a', false)]
    [InlineData('z', false)]
    [InlineData('A', false)]
    [InlineData('Z', false)]
    [InlineData('!', true)]
    [InlineData('~', true)]
    [InlineData(' ', false)]
    public void NonAlphanumericShouldVerify(char character, bool expect)
    {
        var verify = AsciiCharacter.IsNonAlphanumeric(character);

        Assert.Equal(expect, verify);
    }
}