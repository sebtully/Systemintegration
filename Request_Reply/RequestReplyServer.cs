using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Request_Reply
{
    public class RequestReplyServer
    {
        public void StartServer()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "eta_request_queue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var etaMessage = JsonConvert.DeserializeObject<dynamic>(message);

                    Console.WriteLine(" [x] Received request for ETA");

                    var responseMessage = new
                    {
                        status = "Received",
                        flightNumber = etaMessage.flightNumber
                    };

                    var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseMessage));

                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = ea.BasicProperties.CorrelationId;

                    channel.BasicPublish(exchange: "",
                        routingKey: ea.BasicProperties.ReplyTo,
                        basicProperties: replyProps,
                        body: responseBytes);
                };

                channel.BasicConsume(queue: "eta_request_queue",
                    autoAck: true,
                    consumer: consumer);

                Console.WriteLine(" [x] Awaiting ETA requests");
                Console.ReadLine();
            }
        }
    }
}