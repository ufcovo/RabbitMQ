using FileCreateWorkerService;
using FileCreateWorkerService.Services;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton(sp =>
            new ConnectionFactory() 
            { Uri = new Uri(context.Configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync = true });
        services.AddSingleton<RabbitMQClientService>();
    })
    .Build();

await host.RunAsync();
