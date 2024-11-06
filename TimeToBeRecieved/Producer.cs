using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Request_Reply
{
    public class Producer
    {
        private readonly string _queueName;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Producer(string queueName, IConnection connection)
        {
            _queueName = queueName;
            _connection = connection;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void Send(string message, int ttlMilliseconds)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);

            var props = _channel.CreateBasicProperties();
            props.Expiration = ttlMilliseconds.ToString();

            _channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: props, body: messageBytes);

            Console.WriteLine("Sent message with TTL");
            Console.WriteLine($"\tTime:       {DateTime.Now:HH:mm:ss.ffffff}");
            Console.WriteLine($"\tTTL:        {ttlMilliseconds} ms");
            Console.WriteLine($"\tContents:   {message}");
        }
    }
}