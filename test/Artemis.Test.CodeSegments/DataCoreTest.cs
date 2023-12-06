using Artemis.Data.Core;

namespace Artemis.Test.CodeSegments;

public class DataCoreTest
{
    [Theory]
    [InlineData("1q2w3e$R")]
    [InlineData("1123581321")]
    [InlineData("pAssworD")]
    [InlineData("@!#$$%^^&")]
    public void PasswordShouldVerifyHash(string password)
    {
        var passwordHash = Hash.ArtemisHash(password);

        var passwordVerify = Hash.ArtemisHashVerify(passwordHash, password);

        Assert.True(passwordVerify);
    }
}