using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ETA_Messaging2
{
    public class Requester
    {
        private readonly IConnection _connection;
        private readonly string _replyQueueName;
        private readonly EventingBasicConsumer _consumer;
        private readonly IBasicProperties _props;
        private readonly IModel _channel;

        public Requester(IConnection connection)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
            _replyQueueName = _channel.QueueDeclare().QueueName;
            _consumer = new EventingBasicConsumer(_channel);

            var correlationId = Guid.NewGuid().ToString();
            _props = _channel.CreateBasicProperties();
            _props.CorrelationId = correlationId;
            _props.ReplyTo = _replyQueueName;
        }

        public string Call(object message, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var response = string.Empty;

            _consumer.Received += (model, ea) =>
            {
                if (ea.BasicProperties.CorrelationId == _props.CorrelationId)
                {
                    response = Encoding.UTF8.GetString(ea.Body.ToArray());
                }
            };

            _channel.BasicPublish(exchange: "",
                                  routingKey: routingKey,
                                  basicProperties: _props,
                                  body: body);

            _channel.BasicConsume(consumer: _consumer,
                                  queue: _replyQueueName,
                                  autoAck: true);

            while (string.IsNullOrEmpty(response))
            {
                // Wait for the response
            }

            return response;
        }
    }
}