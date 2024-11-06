using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public class MessageSplitter
{
    public static List<XElement> SplitMessages(string filePath)
    {
        XElement checkInFile = XElement.Load(filePath);
        List<XElement> messages = new List<XElement>();

        string messageId = Guid.NewGuid().ToString();
        int sequenceNumber = 1;

        // Split personal information
        XElement passengerInfo = checkInFile.Element("Passenger");
        if (passengerInfo != null)
        {
            XElement personalInfoMessage = new XElement("PersonalInfoMessage",
                new XElement("MessageId", messageId),
                new XElement("SequenceNumber", sequenceNumber++),
                passengerInfo);

            messages.Add(personalInfoMessage);
        }

        // Split luggage information
        IEnumerable<XElement> luggageElements = checkInFile.Elements("Luggage");
        foreach (var luggage in luggageElements)
        {
            XElement luggageInfoMessage = new XElement("LuggageInfoMessage",
                new XElement("MessageId", messageId),
                new XElement("SequenceNumber", sequenceNumber++),
                luggage);

            messages.Add(luggageInfoMessage);
        }

        return messages;
    }
}