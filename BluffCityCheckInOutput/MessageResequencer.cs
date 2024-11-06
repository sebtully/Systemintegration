using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using RabbitMQ.Client;

public class MessageResequencer
{
    private IModel channel;
    private string outputQueue;

    public MessageResequencer(IModel channel, string outputQueue)
    {
        this.channel = channel;
        this.outputQueue = outputQueue;
    }

    public void ResequenceAndSend(List<XElement> messages)
    {
        var sortedMessages = messages.OrderBy(m => (int)m.Element("SequenceNumber")).ToList();

        foreach (var message in sortedMessages)
        {
            var body = Encoding.UTF8.GetBytes(message.ToString());
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.Headers = new Dictionary<string, object> { { "MessageType", message.Name.LocalName } };

            channel.BasicPublish(exchange: "",
                                 routingKey: outputQueue,
                                 basicProperties: properties,
                                 body: body);

            Console.WriteLine("Message sent: {0}", message);
        }
    }
}