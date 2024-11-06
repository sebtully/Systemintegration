using RabbitMQ.Client;
using System;
using System.Text;

namespace Opgave7._1;

class Producer
{
    public static void SendETA()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Opret køen, hvis den ikke eksisterer
            channel.QueueDeclare(queue: "eta_queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Forbereder beskeden (ETA-oplysninger)
            string message = "Flight KL1108 (ARN) ETA 13:45";
            var body = Encoding.UTF8.GetBytes(message);

            // Sender beskeden til køen
            channel.BasicPublish(exchange: "",
                routingKey: "eta_queue",
                basicProperties: null,
                body: body);

            Console.WriteLine($"[x] Sent {message}");
        }
    }
}