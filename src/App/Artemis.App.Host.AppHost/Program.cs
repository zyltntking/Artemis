using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("RedisInstance", 6379)
    .WithImage("redis", "7")
    .WithDataVolume("redis-volume");

//var username = builder.AddParameter("username");

var password = builder.AddParameter("password");

var postgres = builder.AddPostgres("PostgresInstance", password: password, port: 5432)
    .WithImage("postgres", "16")
    .WithDataVolume("postgres-volume");

var artemisDb = postgres.AddDatabase("ArtemisDb", "Artemis");

builder.AddProject<Artemis_App_Identity>("IdentityService")
    .WithReference(redis)
    .WithReference(artemisDb);

builder.Build().Run();