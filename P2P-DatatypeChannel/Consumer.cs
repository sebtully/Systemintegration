using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Opgave7._1;

class Consumer
{
    public void StartConsuming()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Opret køen, hvis den ikke allerede eksisterer
            channel.QueueDeclare(queue: "eta_queue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Opret en event for modtagelse af beskeder
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Received {message}");
            };

            // Start forbrug af beskeder fra køen
            channel.BasicConsume(queue: "eta_queue",
                autoAck: true,
                consumer: consumer);

            Console.WriteLine("Consumer is running. Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}