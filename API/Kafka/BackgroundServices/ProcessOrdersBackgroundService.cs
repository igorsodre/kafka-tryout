using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using API.Domain;
using API.Services.Interfaces;
using Contracts.Enums;
using Contracts.Requests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Kafka.BackgroundServices;

public class ProcessOrdersBackgroundService : BackgroundService
{
    private readonly IEventConsumer<Order> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly ILogger<ProcessOrdersBackgroundService> _logger;

    public ProcessOrdersBackgroundService(
        IEventConsumer<Order> consumer,
        IServiceScopeFactory scopeFactory,
        ILogger<ProcessOrdersBackgroundService> logger)
    {
        _consumer = consumer;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) =>
        Task.Run(async () => {
            while (!stoppingToken.IsCancellationRequested)
            {
                var value = _consumer.Consume(stoppingToken);
                if (value is null)
                {
                    continue;
                }

                var content =
                    $"ProductId: {value.ProductId} === ProductQuantity: {value.Quantity} === Datetime: {DateTime.Now.ToString(CultureInfo.InvariantCulture)}";


                var result = await AddMessage(stoppingToken, content);
                if (result.Success)
                {
                    _consumer.Commit();
                }
                else
                {
                    _logger.LogError("Error consuming the message: {Content}", content);
                }
            }
        });


    private async Task<DefaultResult> AddMessage(CancellationToken stoppingToken, string content)
    {
        using var scope = _scopeFactory.CreateScope();
        var messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
        var result = await messageRepository.AddMessageAsync(
            new IndexRequest
            {
                Type = RequestMessageType.Primary,
                Content = content
            },
            stoppingToken
        );
        return result;
    }
}