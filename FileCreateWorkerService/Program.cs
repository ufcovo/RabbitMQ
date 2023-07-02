using FileCreateWorkerService;
using FileCreateWorkerService.Models;
using FileCreateWorkerService.Services;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<AdventureWorks2019Context>(options =>
            options.UseSqlServer(context.Configuration.GetConnectionString("SqlServer"))
        );
        services.AddHostedService<Worker>();
        services.AddSingleton(sp =>
            new ConnectionFactory() 
            { Uri = new Uri(context.Configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync = true });
        services.AddSingleton<RabbitMQClientService>();
    })
    .Build();

await host.RunAsync();
