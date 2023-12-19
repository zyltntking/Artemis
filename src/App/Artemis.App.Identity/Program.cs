using Artemis.Extensions.Web.Serilog;

namespace Artemis.App.Identity;

/// <summary>
///     Program
/// </summary>
public static class Program
{
    /// <summary>
    ///     Main
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        LogHost.CreateWebApp<Startup>(args);
    }
}