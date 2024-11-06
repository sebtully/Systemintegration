using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace ETA_Messaging2
{
    public class Replier
    {
        private readonly string _queueName;
        private readonly IConnection _connection;

        public Replier(string queueName, IConnection connection)
        {
            _queueName = queueName;
            _connection = connection;
        }

        public async Task ReceiveMessages()
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName,
                                     durable: false,
                                     exclusive: false, // Ensure the queue is not exclusive
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var deserializedMessage = JsonConvert.DeserializeObject<dynamic>(message);

                    // Process the message and create a response
                    var responseMessage = new
                    {
                        Status = "Processed",
                        OriginalMessage = deserializedMessage
                    };

                    var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseMessage));
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = ea.BasicProperties.CorrelationId;

                    channel.BasicPublish(exchange: "",
                                         routingKey: ea.BasicProperties.ReplyTo,
                                         basicProperties: replyProps,
                                         body: responseBytes);
                };

                channel.BasicConsume(queue: _queueName,
                                     autoAck: true,
                                     consumer: consumer);

                // Keep the task running to listen for messages
                await Task.Delay(-1);
            }
        }
    }
}