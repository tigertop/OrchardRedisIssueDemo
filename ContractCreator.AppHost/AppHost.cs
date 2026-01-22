var builder = DistributedApplication.CreateBuilder(args);

var redisDataPath = Path.GetFullPath("./redis-data");
Directory.CreateDirectory(redisDataPath);

var password = "mega_redis_password";
var redisPassword = builder.AddParameter("RedisPassword", password , secret: true);

#pragma warning disable ASPIRECERTIFICATES001
var redis = builder.AddRedis("redis-cache", password: redisPassword, port: 16379)
    .WithDataBindMount(redisDataPath, isReadOnly: false)
    .WithArgs("--appendonly", "yes", "--dir", "/data")
    .WithPersistence(interval: TimeSpan.FromSeconds(15), // Time between snapshots
        keysChangedThreshold: 1)
    .WithLifetime(lifetime: ContainerLifetime.Persistent)
    .WithoutHttpsCertificate();
#pragma warning restore ASPIRECERTIFICATES001

var orchard = builder.AddProject<Projects.ContractCreator_CMS>("ContractCreator-CMS")
    //.WithHttpsEndpoint(5001)
    .WithEnvironment((options) =>
    {
        // Configure the Redis connection.
        var redisEndpoint = redis.GetEndpoint("tcp");
        options.EnvironmentVariables.Add("OrchardCore__OrchardCore_Redis__Configuration",
            $"{redisEndpoint.Host}:{redisEndpoint.Port},password={password},connectTimeout=5000,allowAdmin=true");
        
        options.EnvironmentVariables.Add("OrchardCore__OrchardCore_Redis__InstancePrefix", "Magic:");
    })
    .WaitFor(redis);

var app = builder.Build();

await app.RunAsync();
