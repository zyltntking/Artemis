using Projects;

var builder = DistributedApplication.CreateBuilder(args);

//var username = builder.AddParameter("username");

var password = builder.AddParameter("password");

var redis = builder.AddRedis("RedisInstance", 6379)
    .WithImage("redis", "7")
    .WithDataVolume("redis-volume");

//var mongo = builder.AddMongoDB("MongoInstance", 27017)
//    .WithImage("mongo", "4.4")
//    .WithDataVolume("mongo-volume");

//var mongodb = mongo.AddDatabase("Artemis");

//var rabbitMq = builder.AddRabbitMQ("RabbitMqInstance", username, password, 5672)
//    .WithImage("rabbitmq", "3.13.2")
//    .WithDataVolume("rabbitmq-volume");

var postgres = builder.AddPostgres("PostgresInstance", password: password, port: 5432)
    .WithImage("postgres", "16")
    .WithDataVolume("postgres-volume");

var artemisDb = postgres.AddDatabase("ArtemisDb", "ArtemisDev");

builder.AddProject<Artemis_App_Gateway>("ApiGateway");

var identity = builder.AddProject<Artemis_App_Identity>("IdentityService")
    .WithReference(redis)
    .WithReference(artemisDb);

builder.AddProject<Artemis_App_Resource>("ResourceService")
    .WithReference(redis)
    .WithReference(artemisDb);

builder.AddProject<Artemis_App_School>("SchoolService")
    .WithReference(redis)
    .WithReference(artemisDb);

builder.AddProject<Artemis_App_Task>("TaskService")
    .WithReference(redis)
    .WithReference(artemisDb);

builder.Build().Run();