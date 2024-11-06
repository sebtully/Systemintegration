// Consumer.cs
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Request_Reply
{
    public class Consumer
    {
        private readonly string _requestQueueName;
        private readonly string _invalidQueueName;
        private readonly IModel _channel;

        public Consumer(string requestQueueName, string invalidQueueName, IConnection connection)
        {
            _requestQueueName = requestQueueName;
            _invalidQueueName = invalidQueueName;
            _channel = connection.CreateModel();

            _channel.QueueDeclare(queue: _requestQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueDeclare(queue: _invalidQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void StartListening()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnReceiveCompleted;
            _channel.BasicConsume(queue: _requestQueueName, autoAck: false, consumer: consumer);
        }

        private void OnReceiveCompleted(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = ea.BasicProperties.CorrelationId;

            try
            {
                Console.WriteLine("Received request");
                Console.WriteLine($"\tTime:       {DateTime.Now:HH:mm:ss.ffffff}");
                Console.WriteLine($"\tCorrel. ID: {ea.BasicProperties.CorrelationId}");
                Console.WriteLine($"\tReply to:   {ea.BasicProperties.ReplyTo}");
                Console.WriteLine($"\tContents:   {message}");

                string contents = message;
                if (ea.BasicProperties.Headers != null &&
                    ea.BasicProperties.Headers.TryGetValue("Label", out var labelObj))
                {
                    var label = Encoding.UTF8.GetString((byte[])labelObj);
                    switch (label)
                    {
                        case "SK":
                            contents = "13:45";
                            break;
                        case "KL":
                            contents = "14:25";
                            break;
                        case "SW":
                            contents = "15:40";
                            break;
                    }
                }

                var responseBytes = Encoding.UTF8.GetBytes(contents);
                _channel.BasicPublish(exchange: "", routingKey: ea.BasicProperties.ReplyTo, basicProperties: replyProps, body: responseBytes);

                Console.WriteLine("Sent reply");
                Console.WriteLine($"\tTime:       {DateTime.Now:HH:mm:ss.ffffff}");
                Console.WriteLine($"\tCorrel. ID: {ea.BasicProperties.CorrelationId}");
                Console.WriteLine($"\tContents:   {contents}");

                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid message detected");
                _channel.BasicPublish(exchange: "", routingKey: _invalidQueueName, basicProperties: replyProps, body: body);
                Console.WriteLine("Sent to invalid message queue");
            }
        }
    }
}