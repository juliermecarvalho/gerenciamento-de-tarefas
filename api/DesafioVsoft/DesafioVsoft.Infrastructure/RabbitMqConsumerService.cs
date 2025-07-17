using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace DesafioVsoft.Infrastructure;

public class RabbitMqConsumerService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly string _queueName = "user-task-changed";
    private readonly Channel<string> _eventChannel;

    public RabbitMqConsumerService(IConfiguration config, Channel<string> eventChannel)
    {
        _eventChannel = eventChannel;

        var factory = new ConnectionFactory
        {
            HostName = config["RabbitMQ:HostName"] ?? "localhost",
            UserName = config["RabbitMQ:UserName"] ?? "guest",
            Password = config["RabbitMQ:Password"] ?? "guest"
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.QueueDeclareAsync(queue: _queueName,
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null).GetAwaiter().GetResult();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);


        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            await _eventChannel.Writer.WriteAsync(message, stoppingToken);
        };

        _channel.BasicConsumeAsync(queue: _queueName,
                              autoAck: true,
                              consumer: consumer);

        return Task.CompletedTask;

    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}
