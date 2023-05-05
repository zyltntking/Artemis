using Artemis.App.Swashbuckle;
using Artemis.App.Swashbuckle.Options;
using Artemis.Core.Monitor.Fundamental.Types;
using Artemis.Data.Core.Fundamental;

namespace Artemis.App.ProbeService;

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
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers().AddJsonOptions(option =>
        {
            option.JsonSerializerOptions.AddConverter<CommandType>();
            option.JsonSerializerOptions.AddConverter<HostType>();
            option.JsonSerializerOptions.AddConverter<InstanceType>();
            option.JsonSerializerOptions.AddConverter<PlatformType>();
            option.JsonSerializerOptions.AddConverter<StorageUnit>();
        });

        builder.Services.Configure<HostConfig>(config =>
        {
            var hostType = builder.Configuration["HostConfig:HostType"];

            config.HostType = Enumeration.FromName<HostType>(hostType);

            var instanceType = builder.Configuration["HostConfig:InstanceType"];

            config.InstanceType = Enumeration.FromName<InstanceType>(instanceType);
        });
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var config = new GeneratorConfig();
            options.GenerateSwagger(config);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseReDoc();
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}