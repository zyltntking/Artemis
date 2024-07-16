using System.Security.Cryptography;
using System.Text;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental;
using Artemis.Data.Core.Fundamental.Kit;
using Artemis.Data.Core.Fundamental.Protocol;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Service.Resource.Models;

namespace Artemis.Data.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var record = Enumeration.ToRecordDictionary<DictionaryType>();


        var dictionary = new DataDictionary
        {
            Id = Guid.NewGuid(),
            Name = "Test",
            Description = "Test",
            Valid = true,
            Code = "Test",
            Type = "Dic",
            CreatedAt = default,
            UpdatedAt = default,
            CreateBy = Guid.Empty.GuidToString(),
            ModifyBy = Guid.Empty.GuidToString()
        };

        var dictionaryItem = new DataDictionaryItem
        {
            DataDictionaryId = default,
            Key = null,
            Value = null,
            Description = null,
            Valid = false,
            CreatedAt = default,
            UpdatedAt = default,
            Id = default,
            CreateBy = null,
            ModifyBy = null
        };

        var d = Generator.CreateInstance<DataDictionary>();


        var stamp = Base32.GenerateBase32();

        //var bbc = Generator.IsInherit<IdentityUser>(typeof(IHandlerSlot<>));

        var rsa = RSA.Create();

        var aes = Aes.Create();

        var status = new byte[2];

        var sam = new StatusRecord(12, 17);

        Array.Copy(aes.Key, status, 2);

        var rec = new StatusRecord(status);

        var head = new HeadRecord(10, 11, 20, 21, 30, 31, 1987890782);

        var head2 = new HeadRecord(head.Bytes!);

        var keys = rsa.ExportParameters(true);

        var cc = rsa.ExportRSAPublicKeyPem();

        var orgin = "51,0,52,0,50,0,49,0,57,0,49,0,51,0,48,0,51,0,49,0,48,0,50,0,57,0,50,0,53,0,50,0,51,0,53,0,71,0";

        var abr = new List<byte>();

        var bb = orgin.Split(',');

        foreach (var b in bb) abr.Add(byte.Parse(b));

        var ss = Encoding.Unicode.GetString(abr.ToArray());

        var arr = orgin.Split(',').Select(item =>
        {
            var value = int.Parse(item);

            if (value < 0) return (byte)(value + 256);

            return (byte)value;
        }).ToArray();

        var result = Encoding.Unicode.GetString(arr);
    }
}