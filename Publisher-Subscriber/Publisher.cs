using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Publisher_Subscriber
{
    public class Publisher
    {
        public void RunPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Opret fanout exchange
                channel.ExchangeDeclare(exchange: "eta_fanout", type: "fanout");

                // Opret ETA-besked
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

                // Konverter besked til JSON
                var messageBody = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(messageBody);

                // Send besked til fanout exchange
                channel.BasicPublish(exchange: "eta_fanout",
                    routingKey: "", // Routing key ignoreres i fanout
                    basicProperties: null,
                    body: body);

                Console.WriteLine(" [x] Sendte ETA besked til alle flyselskaber.");
            }
        }
    }
}