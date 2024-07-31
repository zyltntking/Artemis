using Artemis.Extensions.ServiceConnect;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Artemis.App.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddServiceDefaults();

            builder.Services.AddOcelot();

            var app = builder.Build();

            app.UseOcelot();

            app.Run();
        }
    }
}
