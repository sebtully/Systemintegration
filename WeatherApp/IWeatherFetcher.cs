namespace WeatherApp;

public interface IWeatherFetcher
{
    WeatherData FetchWeather(string BluffCity);
}