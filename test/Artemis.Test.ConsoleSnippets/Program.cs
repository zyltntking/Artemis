using Artemis.Data.Core;

namespace Artemis.Test.ConsoleSnippets;

internal class Program
{
    private static void Main(string[] args)
    {
        var hash = new HashProvider();

        var bb = hash.Md5Hash("biu");

        Console.WriteLine("Hello, World!");
    }
}