using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using RabbitMQ.Client;

namespace BluffCityCheckInOutput
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "AirportCheckInOutput",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                List<XElement> messages = MessageSplitter.SplitMessages(@"CheckedInPassenger.xml");
                var resequencer = new MessageResequencer(channel, "AirportCheckInOutput");
                resequencer.ResequenceAndSend(messages);
            }
        }
    }
}