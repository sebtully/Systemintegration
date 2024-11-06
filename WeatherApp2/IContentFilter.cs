namespace WeatherApp2;

public interface IContentFilter
{
    object Filter(WeatherData data);
}

public class AirTrafficControlFilter : IContentFilter
{
    public object Filter(WeatherData data)
    {
        return new
        {
            data.CityName,
            data.Country,
            data.Temperature,
            data.Humidity,
            data.Pressure,
            data.WindSpeed,
            data.CloudCoverage,
            data.Visibility
        };
    }
}

public class AirportInfoCenterFilter : IContentFilter
{
    public object Filter(WeatherData data)
    {
        return new
        {
            data.CityName,
            data.Country,
            data.Sunrise,
            data.Sunset,
            data.Temperature
        };
    }
}

public class AirlineFilter : IContentFilter
{
    public object Filter(WeatherData data)
    {
        return new
        {
            data.CityName,
            data.Country,
            data.Temperature,
            data.CloudCoverage
        };
    }
}
