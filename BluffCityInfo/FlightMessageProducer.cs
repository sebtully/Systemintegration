using System.Collections.Generic;
using System.Xml;

namespace BluffCityInfo;

public class FlightMessageProducer
{
    private readonly FlightInfoParser _parser;
    private readonly MessageSender _sender;

    public FlightMessageProducer(FlightInfoParser parser, MessageSender sender)
    {
        _parser = parser;
        _sender = sender;
    }

    public string GetPassengerInfoMessage()
    {
        var passengerInfo = _parser.GetPassengerInfo();
        return passengerInfo?.OuterXml;
    }

    public IEnumerable<string> GetLuggageInfoMessages()
    {
        var luggageNodes = _parser.GetLuggageInfo();
        foreach (XmlNode luggageNode in luggageNodes)
        {
            yield return luggageNode.OuterXml;
        }
    }
}