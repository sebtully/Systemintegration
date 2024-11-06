using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace BLD;

abstract class BaggageStatisticsConsumer
{
    private static Random random = new Random();

    public static void StartListening()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Declare the queue if it hasn't been declared already
            channel.QueueDeclare(queue: "baggage_transactions",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
                // Simulate processing with gate number and interval
                int gateNumber = random.Next(1, 20);  // Random gate number between 1 and 20
                int interval = random.Next(1000, 5000); // Random processing time interval (ms)

                Console.WriteLine($" [x] Received baggage transaction: {message}");
                Console.WriteLine($" [x] Processing at Gate {gateNumber} for {interval / 1000} seconds...");

                // Simulate processing time
                Thread.Sleep(interval);

                Console.WriteLine(" [x] Finished processing transaction");
            };

            // Start consuming messages
            channel.BasicConsume(queue: "baggage_transactions",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" [*] Waiting for baggage transactions. To exit press CTRL+C");
            Console.ReadLine();
        }
    }
}
