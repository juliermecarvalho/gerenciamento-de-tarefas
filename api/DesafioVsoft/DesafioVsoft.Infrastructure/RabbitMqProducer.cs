using DesafioVsoft.Domain.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace DesafioVsoft.Infrastructure;

public class RabbitMqProducer : IRabbitMqProducer
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly string _queueName = "user-task-changed";

    public RabbitMqProducer(IConfiguration config)
    {

        var factory = new ConnectionFactory()
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

    public Task PublishUserChangedAsync(Guid userId, Guid taskId)
    {
        var message = JsonSerializer.Serialize(new { userId, taskId });
        var body = Encoding.UTF8.GetBytes(message);

        var props = new BasicProperties();
        _channel.BasicPublishAsync(exchange: "",
                              routingKey: _queueName,
                               mandatory: true,
                              basicProperties: props,
                              body: body).GetAwaiter().GetResult();

        return Task.CompletedTask;
    }


}


public class RabbitFacke : IRabbitMqProducer
{
    Task IRabbitMqProducer.PublishUserChangedAsync(Guid userId, Guid taskId)
    {
        return Task.CompletedTask;
    }
}
