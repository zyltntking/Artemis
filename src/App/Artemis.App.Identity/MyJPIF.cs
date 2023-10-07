namespace Artemis.App.Identity;

//public static class MyJPIF
//{
//    public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
//    {
//        var builder = new ServiceCollection()
//            .AddLogging()
//            .AddMvc()
//            .AddNewtonsoftJson()
//            .Services.BuildServiceProvider();

//        return builder
//            .GetRequiredService<IOptions<MvcOptions>>()
//            .Value
//            .InputFormatters
//            .OfType<NewtonsoftJsonPatchInputFormatter>()
//            .First();
//    }
//}