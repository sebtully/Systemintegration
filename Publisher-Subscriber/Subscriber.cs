using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Publisher_Subscriber
{
    public class Subscriber
    {
        public void RunSubscriber(string airline)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Opret ny queue til flyselskabet
                string queueName = $"{airline}_queue";
                channel.QueueDeclare(queue: queueName,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                // Bind queue til fanout exchange
                channel.QueueBind(queue: queueName,
                    exchange: "eta_fanout",
                    routingKey: ""); // Routing key ignoreres i fanout

                Console.WriteLine($" [*] {airline} Venter på ETA-beskeder. Tryk CTRL+C for at afslutte.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var etaMessage = JsonConvert.DeserializeObject<dynamic>(message);

                    Console.WriteLine($" [x] {airline} Modtog ETA-besked for Fly {etaMessage.flightNumber}");
                    Console.WriteLine($"Ankomsttidspunkt: {etaMessage.estimatedTimeOfArrival}");
                    Console.WriteLine($"Status: {etaMessage.status}");
                };

                // Start forbrug af beskeder
                channel.BasicConsume(queue: queueName,
                    autoAck: true,
                    consumer: consumer);

                Console.ReadLine(); // Hold applikationen kørende for at lytte til beskeder
            }
        }
    }
}