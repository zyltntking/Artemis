using System.Security.Cryptography;
using System.Text;
using Artemis.Data.Core.Fundamental;
using Artemis.Data.Core.Fundamental.Design;
using Artemis.Data.Core.Fundamental.Kit;
using Artemis.Data.Core.Fundamental.Protocol;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Resource.Context;

namespace Artemis.Data.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var record = Enumeration.ToRecordDictionary<DictionaryType>();

        var stamp = Base32.GenerateBase32();

        var code1 = DesignCode.Organization("5325", 1); //红河州ORG5325001

        var code2 = DesignCode.Organization("5325", 1, "ORG5325001"); //绿春县ORG5325001001

        var code3 = DesignCode.Organization("5325", 1, "ORG5325001001"); //大兴村ORG5325001001001


        var code4 = DesignCode.Task("ORG5325001", 1);

        var code5 = DesignCode.Task("ORG5325001", 1, "TA240802152000ORG5325001SK001");

        var code6 = DesignCode.Task("ORG5325001", 2, "TA240802152000ORG5325001SK001");

        var code7 = DesignCode.Task("ORG5325001", 3, "TA240802152000ORG5325001SK001002");

        var org1 = Instance.CreateInstance<ArtemisOrganization>();
        org1.Name = "红河州教体局";
        org1.Code = DesignCode.Organization("5325", 1);
        org1.DesignCode = DesignCode.Organization("5325", 1);
        org1.Type = OrganizationType.Management;
        org1.Status = OrganizationStatus.InOperation;

        var org2 = Instance.CreateInstance<ArtemisOrganization>();
        org2.Name = "蒙自市教体局";
        org2.Code = DesignCode.Organization("5325", 1, org1.Code);
        org2.DesignCode = DesignCode.Organization("5325", 1, org1.Code);
        org2.Type = OrganizationType.Management;
        org2.Status = OrganizationStatus.InOperation;
        org2.ParentId = org1.Id;

        var org3 = Instance.CreateInstance<ArtemisOrganization>();
        org3.Name = "西南联大蒙自小学";
        org3.Code = DesignCode.Organization("5325", 1, org2.Code);
        org3.DesignCode = DesignCode.Organization("5325", 1, org2.Code);
        org3.Type = OrganizationType.Management;
        org3.Status = OrganizationStatus.InOperation;
        org3.ParentId = org2.Id;

        var org4 = Instance.CreateInstance<ArtemisOrganization>();
        org4.Name = "蒙自市机关幼儿园";
        org4.Code = DesignCode.Organization("5325", 2, org2.Code);
        org4.DesignCode = DesignCode.Organization("5325", 2, org2.Code);
        org4.Type = OrganizationType.Management;
        org4.Status = OrganizationStatus.InOperation;
        org4.ParentId = org2.Id;


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