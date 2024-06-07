using System.Security.Cryptography;
using System.Text;
using Artemis.Data.Core.Fundamental.Kit.Crypto;
using Artemis.Data.Core.Fundamental.Types;

namespace Artemis.Data.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var rsa = RSA.Create();

        var keys = rsa.ExportParameters(true);

        var cc = rsa.ExportRSAPublicKeyPem();

        var orgin = "51,0,52,0,50,0,49,0,57,0,49,0,51,0,48,0,51,0,49,0,48,0,50,0,57,0,50,0,53,0,50,0,51,0,53,0,71,0";

        var abr = new List<byte>();

        var bb = orgin.Split(',');

        foreach (var b in bb)
        {
            abr.Add(byte.Parse(b));
        }

        var ss = Encoding.Unicode.GetString(abr.ToArray());

        var arr = orgin.Split(',').Select(item =>
        {
            var value = int.Parse(item);

            if (value < 0)
            {
                return (byte)(value + 256);
            }

            return (byte)value;
        }).ToArray();

        var result = Encoding.Unicode.GetString(arr);
    }
}