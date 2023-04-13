namespace Artemis.Test.ConsoleSnippets
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bb = File.ReadAllLines("/etc/os-release");

            Console.WriteLine("Hello, World!");
        }
    }
}