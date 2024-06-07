using System.Linq;
using System.Text;

namespace Artemis.Com.ExcelTools
{
    public class Utils
    {
        public string ToNameStr(string orgin)
        {
            var arr = orgin.Split(',').Select(item =>
            {
                var value = int.Parse(item);

                if (value < 0)
                {
                    return (byte)(value + 256);
                }

                return (byte)value;
            }).ToArray();

            var name = Encoding.Unicode.GetString(arr);

            return new string(name.ToCharArray().Reverse().ToArray());
        }

        public string ToCodeStr(string orgin)
        {
            var arr = orgin.Split(',').Select(item =>
            {
                var value = int.Parse(item);

                if (value < 0)
                {
                    return (byte)(value + 256);
                }

                return (byte)value;
            }).ToArray();

            var code = Encoding.Unicode.GetString(arr);

            return new string(code.ToCharArray().Reverse().ToArray());
        }
    }
}
