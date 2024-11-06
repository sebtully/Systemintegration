using System;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Request_Reply
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();

            var producer = new Producer("test_queue", connection);
            var consumer = new Consumer("test_queue", connection);
            
            // Send a message with a TTL of 5000 milliseconds (5 seconds)
            producer.Send("Hello, World!", 5000);

            // Start the consumer to receive messages
            //consumer.Receive();

            // Wait for 6 seconds to ensure the message TTL expires
            Console.WriteLine("Waiting for message to expire...");
            Thread.Sleep(6000);
        }
    }
}