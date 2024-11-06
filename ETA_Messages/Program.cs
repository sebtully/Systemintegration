using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace ETA_Messages
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
                    //MessageId = Guid.NewGuid().ToString(),
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
                var producer = new Producer("ETAExchange", connection);
                producer.SendMessage(etaMessage);

                var channel = connection.CreateModel();
                channel.QueueDeclare(queue: "SASQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "SWAQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: "KLMQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                channel.QueueBind(queue: "SASQueue", exchange: "ETAExchange", routingKey: "");
                channel.QueueBind(queue: "SWAQueue", exchange: "ETAExchange", routingKey: "");
                channel.QueueBind(queue: "KLMQueue", exchange: "ETAExchange", routingKey: "");

                var consumerSAS = new Consumer("SASQueue", connection);
                var consumerSWA = new Consumer("SWAQueue", connection);
                var consumerKLM = new Consumer("KLMQueue", connection);

                //await Task.WhenAll(consumerSAS.ReceiveMessages(), consumerSWA.ReceiveMessages(), consumerKLM.ReceiveMessages());
            }
        }
    }
}