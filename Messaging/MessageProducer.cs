using Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using Contracts.Configuration;

namespace Messaging;

public class MessageProducer : IMessageProducer
{
    private readonly ILogger<MessageProducer> _logger;

    private readonly RabbitMqConfiguration _rabbitMqConfiguration;

    private IConnection _connection;
    private readonly IModel _channel;

    private const int RetryCount = 3;

    public MessageProducer(ILogger<MessageProducer> logger, IOptions<RabbitMqConfiguration> rabbitMqConfiguration)
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

    public void SendMessage<T>(T message) where T : class
    {
        var jsonString = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(jsonString);

        _channel.BasicPublish(exchange: "", routingKey: _rabbitMqConfiguration.RoutingKey, body: body);

        _logger.LogInformation("Data was sent to RabbitMQ");
    }
}