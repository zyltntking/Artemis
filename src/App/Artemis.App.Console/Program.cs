using System.Text;

namespace Artemis.App.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello, World!");

            var c2 = "赵云龙";


            var b2 = Encoding.Unicode.GetBytes(c2).Reverse();

            var s2 = Convert.ToBase64String(b2.ToArray());

        }
    }
}