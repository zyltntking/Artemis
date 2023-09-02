using Artemis.Extensions.Web;
using Artemis.Extensions.Web.OpenApi;
using Artemis.Extensions.Web.Serilog;

namespace Artemis.App.WebApi;

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
        WebApiHost.CreateHost(args);
    }
}