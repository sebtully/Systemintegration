using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Request_Reply
{
    public class Consumer
    {
        private readonly string _queueName;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Consumer(string queueName, IConnection connection)
        {
            _queueName = queueName;
            _connection = connection;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void Receive()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("Received message");
                Console.WriteLine($"\tTime:       {DateTime.Now:HH:mm:ss.ffffff}");
                Console.WriteLine($"\tContents:   {message}");
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }
    }
}