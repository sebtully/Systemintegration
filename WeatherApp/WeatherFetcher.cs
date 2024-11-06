using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WeatherApp;

public class WeatherFetcher : IWeatherFetcher
{
    private const string ApiKey = "f80dd07ecbdd8f83b47b095cb13af603";
    private const string BaseUrl = "http://api.openweathermap.org/data/2.5/weather";

    public WeatherData FetchWeather(string city)
    {
        return FetchWeatherAsync(city).GetAwaiter().GetResult();
    }

    private async Task<WeatherData> FetchWeatherAsync(string city)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"{BaseUrl}?q={city}&appid={ApiKey}&units=metric";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject weatherJson = JObject.Parse(responseBody);

            return new WeatherData
            {
                CityName = weatherJson["name"].ToString(),
                Country = weatherJson["sys"]["country"].ToString(),
                Temperature = (double)weatherJson["main"]["temp"],
                Humidity = (int)weatherJson["main"]["humidity"],
                Pressure = (int)weatherJson["main"]["pressure"],
                WindSpeed = (double)weatherJson["wind"]["speed"],
                CloudCoverage = weatherJson["weather"][0]["description"].ToString(),
                Visibility = (int)weatherJson["visibility"],
                Sunrise = DateTimeOffset.FromUnixTimeSeconds((long)weatherJson["sys"]["sunrise"]).DateTime,
                Sunset = DateTimeOffset.FromUnixTimeSeconds((long)weatherJson["sys"]["sunset"]).DateTime
            };
        }
    }
}