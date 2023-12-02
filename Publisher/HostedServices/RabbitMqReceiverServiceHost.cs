using Contracts;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using Contracts.Configuration;

namespace Publisher.HostedServices;

public class RabbitMqReceiverServiceHost : BackgroundService
{
    private readonly ILogger<RabbitMqReceiverServiceHost> _logger;

    private readonly RabbitMqConfiguration _rabbitMqConfiguration;

    private IConnection _connection;
    private readonly IModel _channel;

    private const int RetryCount = 3;

    public RabbitMqReceiverServiceHost(ILogger<RabbitMqReceiverServiceHost> logger,
        IOptions<RabbitMqConfiguration> rabbitMqConfiguration)
    {
        _logger = logger;

        _rabbitMqConfiguration = rabbitMqConfiguration.Value;

        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqConfiguration.HostName,
            Port = _rabbitMqConfiguration.Port
        };

        var policy = Policy.Handle<SocketException>().Or<BrokerUnreachableException>().WaitAndRetry(RetryCount,
            op => TimeSpan.FromSeconds(Math.Pow(2, op)),
            (ex, time) => { _logger.LogInformation("Couldn't connect to RabbitMQ server."); });

        policy.Execute(() => { _connection = factory.CreateConnection(); });

        _channel = _connection.CreateModel();
        _logger.LogInformation("Connection to RabbitMQ created");

        _channel.QueueDeclare(queue: _rabbitMqConfiguration.Queue, durable: false, exclusive: false,
            autoDelete: true, arguments: null);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested)
        {
            _channel.Dispose();
            _connection.Dispose();
            return Task.CompletedTask;
        }

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Message received from RabbitMQ: {message}");

            Console.WriteLine(message);
        };

        _channel.BasicConsume(queue: _rabbitMqConfiguration.Queue, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}