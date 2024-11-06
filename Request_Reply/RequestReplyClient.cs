using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Request_Reply
{
    public class RequestReplyClient
    {
        public void SendRequest()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var replyQueueName = channel.QueueDeclare().QueueName;
                var consumer = new EventingBasicConsumer(channel);
                var correlationId = Guid.NewGuid().ToString();
                var props = channel.CreateBasicProperties();
                props.CorrelationId = correlationId;
                props.ReplyTo = replyQueueName;

                var message = new
                {
                    flightNumber = "BC123",
                    airline = "Bluff City Airlines",
                    origin = "JFK",
                    destination = "BCA",
                    aircraftType = "Boeing 737",
                    estimatedTimeOfArrival = "2024-09-11T18:45:00Z",
                    status = "On Time"
                };

                var messageBody = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageBody);

                channel.BasicPublish(exchange: "",
                    routingKey: "eta_request_queue",
                    basicProperties: props,
                    body: body);

                Console.WriteLine(" [x] Sent ETA request");

                consumer.Received += (model, ea) =>
                {
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        var response = Encoding.UTF8.GetString(ea.Body.ToArray());
                        Console.WriteLine(" [x] Received '{0}'", response);
                    }
                };

                channel.BasicConsume(consumer: consumer,
                    queue: replyQueueName,
                    autoAck: true);

                Console.ReadLine();
            }
        }
    }
}