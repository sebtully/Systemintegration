using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json; // Ensure you have Newtonsoft.Json package installed

namespace Systemintegration.Messaging
{
    public class Producer
    {
        public void SendMessage()
        {
            var message = new
            {
                Header = new
                {
                    MessageType = "FlightInformation",
                    Timestamp = DateTime.UtcNow.ToString("o")
                },
                Body = new
                {
                    AirlineCompany = "ExampleAir",
                    ScheduledTime = DateTime.UtcNow.AddHours(2).ToString("o"),
                    FlightNo = "EX123",
                    Destination = "Bluff City International Airport",
                    CheckIn = "Terminal 1, Counter 5"
                }
            };

            var messageString = JsonConvert.SerializeObject(message);

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(messageString);

                channel.BasicPublish(exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: body);

                Console.WriteLine(" [x] Sent {0}", messageString);
            }
        }
    }
}