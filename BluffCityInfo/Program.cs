using System;
using System.IO;

namespace BluffCityInfo;

class Program
{
    static void Main(string[] args)
    {
        // Read XML data from file
        string xmlFilePath = "FlightDetailsInfoResponse.xml";
        string xml = File.ReadAllText(xmlFilePath);

        // Create parser for XML content
        var parser = new FlightInfoParser(xml);

        // Initialize message sender
        var sender = new MessageSender("localhost");

        // Define the single output queue
        string outputQueue = "combined_info";
        sender.DeclareQueue(outputQueue);

        // Create producer, resequencer, and aggregator
        var producer = new FlightMessageProducer(parser, sender);
        var resequencer = new Resequencer();
        var aggregator = new Aggregator();

        // Collect messages
        resequencer.AddMessage(producer.GetPassengerInfoMessage());
        foreach (var luggageMessage in producer.GetLuggageInfoMessages())
        {
            resequencer.AddMessage(luggageMessage);
        }

        // Combine ordered messages
        var combinedMessage = aggregator.CombineMessages(resequencer.GetOrderedMessages());

        // Send combined message to the single output queue
        sender.SendMessage(outputQueue, combinedMessage);

        // Close connection
        sender.Close();
    }
}