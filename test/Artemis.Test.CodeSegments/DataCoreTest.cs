using Artemis.Data.Core;

namespace Artemis.Test.CodeSegments;

public class DataCoreTest
{
    [Fact]
    public void BytesUtilityTest()
    {
        var a = "";

        var bb = string.Format(a,10);

        var dd = Hash.ArtemisHash("1q2w3e$R");

        var vary = Hash.ArtemisHashVerify(dd, "1q2w3e$R");
    }
}