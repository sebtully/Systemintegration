namespace WeatherApp;

public class MessageRouter
{
    private readonly IWeatherFetcher _fetcher;

    public MessageRouter(IWeatherFetcher fetcher)
    {
        _fetcher = fetcher;
    }

    public void RouteWeatherData()
    {
        WeatherData data = _fetcher.FetchWeather("Bluff City");

        // Routing to Air Traffic Control Center
        var atcData = new AirTrafficControlFilter().Filter(data);
        var atcMessage = new StringTranslator().Translate(atcData);
        Console.WriteLine($"ATC Data: {atcMessage}");

        // Routing to Airport Information Center
        var infoCenterData = new AirportInfoCenterFilter().Filter(data);
        var infoCenterMessage = new StringTranslator().Translate(infoCenterData);
        Console.WriteLine($"Airport Info Center Data: {infoCenterMessage}");

        // Routing to Airlines
        var airlineData = new AirlineFilter().Filter(data);

        // KLM (String)
        var klmMessage = new StringTranslator().Translate(airlineData);
        Console.WriteLine($"KLM Data: {klmMessage}");

        // SAS (Class)
        var sasMessage = new ClassTranslator().Translate(airlineData);
        Console.WriteLine($"SAS Data: {sasMessage}");

        // South West & British Airways (XML)
        var swXmlMessage = new XmlTranslator().Translate(airlineData);
        Console.WriteLine($"South West XML Data: {swXmlMessage}");

        var baXmlMessage = new XmlTranslator().Translate(airlineData);
        Console.WriteLine($"British Airways XML Data: {baXmlMessage}");
    }
}