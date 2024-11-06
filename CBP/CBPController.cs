using RabbitMQ.Client;
using System;
using System.Text;
using System.Xml.Linq;

namespace CBP;

class CBPController
{
    public static void SendPassengerInfo()
    {
        // Hardcoded path to the XML file
        string xmlFilePath = "PassengerData.xml";

        // Read XML from file
        string xmlData = Utils.ReadXmlFile(xmlFilePath);
        
        // Parse XML
        XDocument xmlDocument = XDocument.Parse(xmlData);
        var passports = xmlDocument.Descendants("Passport");

        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: "national_check", type: "direct");

            foreach (var passport in passports)
            {
                var nationality = passport.Element("Nationality")?.Value;
                if (nationality != null)
                {
                    var message = Encoding.UTF8.GetBytes(xmlData);
                    channel.BasicPublish(exchange: "national_check", routingKey: nationality, basicProperties: null, body: message);
                    Console.WriteLine($"Sent message for nationality: {nationality}");
                }
            }
        }
    }
}