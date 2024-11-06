namespace WeatherApp2;

public interface IWeatherFetcher
{
    WeatherData FetchWeather(string BluffCity);
}