using System.Runtime.InteropServices;

namespace Artemis.Test.ConsoleSnippets
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var a = RuntimeInformation.OSDescription;

            var b = RuntimeInformation.OSArchitecture;

            var c = RuntimeInformation.FrameworkDescription;

            var d = RuntimeInformation.ProcessArchitecture;

            var e = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            var f = Environment.OSVersion;

            var g = Environment.CommandLine;

            var h = Environment.MachineName;

            var i = Environment.Version;

            var bb = File.ReadAllLines("/etc/os-release");

            Console.WriteLine("Hello, World!");
        }
    }
}