using System;
using System.Text;
using System.Xml;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class AirlineCompanyXPath
{
    private IModel channel;

    public AirlineCompanyXPath(IModel channel)
    {
        this.channel = channel;

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += OnMessage;
        channel.BasicConsume(queue: "AirportInfoGateNo",
                             autoAck: true,
                             consumer: consumer);
    }

    private void OnMessage(object model, BasicDeliverEventArgs ea)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var airlineCompany = ea.BasicProperties.Headers["AirlineCompany"].ToString();
        Console.WriteLine($"Received message for {airlineCompany}: {message}");

        XmlDocument xml = new XmlDocument();
        xml.LoadXml(message);
        XmlNode itemNode = xml.SelectSingleNode("/AirportInfoGate/airline/Flight");
        if (itemNode != null)
        {
            XmlNode value = itemNode.SelectSingleNode("Gate");
            if (value != null)
            {
                String valueString = value.Attributes["No"].Value;
                Console.WriteLine("Længde : " + valueString.Length);
                if (valueString != null)
                {
                    Console.WriteLine("GateNo : " + valueString);
                }
            }
        }
        Console.WriteLine("Besked sendt");
    }
}