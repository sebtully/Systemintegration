namespace BluffCityInfo;

using System.Xml;

public class FlightInfoParser
{
    private readonly XmlDocument _doc;

    public FlightInfoParser(string xmlContent)
    {
        _doc = new XmlDocument();
        _doc.LoadXml(xmlContent);
    }

    public XmlNode GetPassengerInfo()
    {
        return _doc.SelectSingleNode("//Passenger");
    }

    public XmlNodeList GetLuggageInfo()
    {
        return _doc.SelectNodes("//Luggage");
    }
}
