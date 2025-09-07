using Confluent.Kafka;

var builder = WebApplication.CreateBuilder(args);

// ✅ Increase Kestrel request body size limit (100 MB)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100 MB
});

builder.Services.AddControllers();

// ✅ Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ Producer with increased message size
builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:9092",
        MessageMaxBytes = 104857600 // 100 MB
    };
    return new ProducerBuilder<Null, string>(config).Build();
});

// ✅ Consumer with increased fetch size
builder.Services.AddSingleton<IConsumer<Null, string>>(sp =>
{
    var config = new ConsumerConfig
    {
        BootstrapServers = "localhost:9092",
        GroupId = "test-group",
        AutoOffsetReset = AutoOffsetReset.Earliest,
        FetchMaxBytes = 104857600,          // allow fetching large messages
        MaxPartitionFetchBytes = 104857600  // per-partition fetch limit
    };
    return new ConsumerBuilder<Null, string>(config).Build();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();




//using Confluent.Kafka;

//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddControllers();

//// ✅ Add Swagger services
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddSingleton<IProducer<Null, string>>(sp =>
//{
//    var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
//    return new ProducerBuilder<Null, string>(config).Build();
//});

//builder.Services.AddSingleton<IConsumer<Null, string>>(sp =>
//{
//    var config = new ConsumerConfig
//    {
//        BootstrapServers = "localhost:9092",
//        GroupId = "test-group",
//        AutoOffsetReset = AutoOffsetReset.Earliest
//    };
//    return new ConsumerBuilder<Null, string>(config).Build();
//});

//var app = builder.Build();

//// ✅ Enable Swagger UI
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.MapControllers();
//app.Run();
