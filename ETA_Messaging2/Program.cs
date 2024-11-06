using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace ETA_Messaging2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Hardcoded flight information
            string flightNo = "AB123";
            string airline = "AirlineName";
            string scheduledTimeInput = "14:30:00";
            TimeSpan scheduledTime = TimeSpan.Parse(scheduledTimeInput);

            TimeSpan estimatedTime = scheduledTime.Add(TimeSpan.FromMinutes(30));

            string scheduledArrivalTime = DateTime.UtcNow.Date.Add(scheduledTime).ToString("dd-MMMM-yyyy HH:mm:ss");
            string estimatedArrivalTime = DateTime.UtcNow.Date.Add(estimatedTime).ToString("dd-MMMM-yyyy HH:mm:ss");

            string origin = "ODD";
            string destination = "AAR";

            var etaMessage = new
            {
                Header = new
                {
                    Timestamp = DateTime.UtcNow.ToString("o"),
                    Sender = "Air Traffic Control",
                    Receiver = "Airport Information Center"
                },
                Body = new
                {
                    FlightNo = flightNo,
                    Airline = airline,
                    ScheduledArrivalTime = scheduledArrivalTime,
                    EstimatedArrivalTime = estimatedArrivalTime,
                    Origin = origin,
                    Destination = destination
                }
            };

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                var producer = new Requester(connection);
                var response = producer.Call(etaMessage, airline);

                Console.WriteLine(" [x] Received response: {0}", response);

                var channel = connection.CreateModel();
                channel.ExchangeDeclare(exchange: "ETAExchange", type: "direct");

                channel.QueueDeclare(queue: "SASQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "SWAQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "KLMQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                channel.QueueBind(queue: "SASQueue", exchange: "ETAExchange", routingKey: "SAS");
                channel.QueueBind(queue: "SWAQueue", exchange: "ETAExchange", routingKey: "SWA");
                channel.QueueBind(queue: "KLMQueue", exchange: "ETAExchange", routingKey: "KLM");

                var consumerSAS = new Replier("SASQueue", connection);
                var consumerSWA = new Replier("SWAQueue", connection);
                var consumerKLM = new Replier("KLMQueue", connection);

                await Task.WhenAll(consumerSAS.ReceiveMessages(), consumerSWA.ReceiveMessages(), consumerKLM.ReceiveMessages());
            }
        }
    }
}