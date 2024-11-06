namespace BluffCityInfo;

using RabbitMQ.Client;
using System.Text;

public class MessageSender
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageSender(string hostname)
    {
        var factory = new ConnectionFactory() { HostName = hostname };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void DeclareQueue(string queueName)
    {
        _channel.QueueDeclare(queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);
    }

    public void SendMessage(string queueName, string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "",
            routingKey: queueName,
            basicProperties: null,
            body: body);
        System.Console.WriteLine($"[x] Sent to {queueName}: {message}");
    }

    public void Close()
    {
        _channel.Close();
        _connection.Close();
    }
}
