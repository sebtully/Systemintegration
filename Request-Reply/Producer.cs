// Producer.cs
using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Request_Reply
{
    public class Producer
    {
        private readonly string _requestQueueName;
        private readonly string _replyQueueName;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public Producer(string requestQueueName, string replyQueueName, IConnection connection)
        {
            _requestQueueName = requestQueueName;
            _replyQueueName = replyQueueName;
            _connection = connection;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _requestQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: _replyQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void Send(string airline)
        {
            var message = new { Airline = airline };
            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var props = _channel.CreateBasicProperties();
            props.ReplyTo = _replyQueueName;
            props.CorrelationId = Guid.NewGuid().ToString();
            props.Headers = new Dictionary<string, object> { { "Label", airline.Substring(0, 2) } };

            _channel.BasicPublish(exchange: "", routingKey: _requestQueueName, basicProperties: props, body: messageBytes);

            Console.WriteLine("Sent request");
            Console.WriteLine($"\tTime:       {DateTime.Now:HH:mm:ss.ffffff}");
            Console.WriteLine($"\tCorrel. ID: {props.CorrelationId}");
            Console.WriteLine($"\tReply to:   {props.ReplyTo}");
            Console.WriteLine($"\tContents:   {airline}");
        }

        public void ReceiveSync()
        {
            var consumer = new EventingBasicConsumer(_channel);
            string response = null;

            consumer.Received += (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId != null)
                {
                    response = Encoding.UTF8.GetString(ea.Body.ToArray());
                    Console.WriteLine("Received reply");
                    Console.WriteLine($"\tTime:       {DateTime.Now:HH:mm:ss.ffffff}");
                    Console.WriteLine($"\tCorrel. ID: {ea.BasicProperties.CorrelationId}");
                    Console.WriteLine($"\tContents:   {response}");
                }
            };

            _channel.BasicConsume(queue: _replyQueueName, autoAck: true, consumer: consumer);

            while (response == null)
            {
                Task.Delay(100).Wait();
            }
        }
    }
}