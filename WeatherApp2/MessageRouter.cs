using RabbitMQ.Client;
using System.Text;

namespace WeatherApp2;

public class MessageRouter
{
    private readonly IWeatherFetcher _fetcher;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageRouter(IWeatherFetcher fetcher)
    {
        _fetcher = fetcher;

        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: "weather_data", durable: false, exclusive: false, autoDelete: false, arguments: null);
    }

    public void RouteWeatherData()
    {
        WeatherData data = _fetcher.FetchWeather("Bluff City");

        // Routing to Air Traffic Control Center
        var atcData = new AirTrafficControlFilter().Filter(data);
        var atcMessage = new StringTranslator().Translate(atcData);
        PublishMessage("ATC Data", atcMessage);

        // Routing to Airport Information Center
        var infoCenterData = new AirportInfoCenterFilter().Filter(data);
        var infoCenterMessage = new StringTranslator().Translate(infoCenterData);
        PublishMessage("Airport Info Center Data", infoCenterMessage);

        // Routing to Airlines
        var airlineData = new AirlineFilter().Filter(data);

        // KLM (String)
        var klmMessage = new StringTranslator().Translate(airlineData);
        PublishMessage("KLM Data", klmMessage);

        // SAS (Class)
        var sasMessage = new ClassTranslator().Translate(airlineData);
        PublishMessage("SAS Data", sasMessage);

        // South West & British Airways (XML)
        var swXmlMessage = new XmlTranslator().Translate(airlineData);
        PublishMessage("South West XML Data", swXmlMessage);

        var baXmlMessage = new XmlTranslator().Translate(airlineData);
        PublishMessage("British Airways XML Data", baXmlMessage);
    }

    private void PublishMessage(string routingKey, string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "", routingKey: "weather_data", basicProperties: null, body: body);
        Console.WriteLine($" [x] Sent {routingKey}: {message}");
    }

    ~MessageRouter()
    {
        _channel.Close();
        _connection.Close();
    }
}