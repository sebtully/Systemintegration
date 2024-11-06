using RabbitMQ.Client;
using System;
using System.Text;

namespace BLD;

abstract class BaggageLoadingSystem
{
    public static void SendBaggageTransaction(string transactionXml)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Declare the queue where messages will be sent
            channel.QueueDeclare(queue: "baggage_transactions",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(transactionXml);

            // Send message to the queue
            channel.BasicPublish(exchange: "",
                routingKey: "baggage_transactions",
                basicProperties: null,
                body: body);
            Console.WriteLine(" [x] Sent baggage transaction");
        }
    }
}