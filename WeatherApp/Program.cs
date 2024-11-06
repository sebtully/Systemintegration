namespace WeatherApp;

class Program
{
    static void Main(string[] args)
    {
        IWeatherFetcher fetcher = new WeatherFetcher();
        MessageRouter router = new MessageRouter(fetcher);
        router.RouteWeatherData();
    }
}