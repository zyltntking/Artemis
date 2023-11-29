using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Kit;

namespace Artemis.Test.CodeSegments
{
    public class DataCoreTest
    {
        [Fact]
        public void BytesUtilityTest()
        {
            var dd = Hash.ArtemisHash("1q2w3e$R");

            var vary = Hash.ArtemisHashVerify(dd, "1q2w3e$R");
        }
    }
}