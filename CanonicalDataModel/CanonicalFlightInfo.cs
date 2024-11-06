namespace Canonical_Data_Model;

public class CanonicalFlightInfo
{
    public string Airline { get; set; }
    public string FlightNumber { get; set; }
    public string Destination { get; set; }
    public string Origin { get; set; }
    public string Type { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string TimePeriod { get; set; }

    // Constructor for initialization
    public CanonicalFlightInfo(string airline, string flightNumber, string destination, string origin,
        string type, DateTime date, TimeSpan time, string timePeriod = null)
    {
        Airline = airline;
        FlightNumber = flightNumber;
        Destination = destination;
        Origin = origin;
        Type = type;
        Date = date;
        Time = time;
        TimePeriod = timePeriod;
    }

    public void PrintFlightInfo()
    {
        Console.WriteLine($"Airline: {Airline}");
        Console.WriteLine($"Flight Number: {FlightNumber}");
        Console.WriteLine($"Destination: {Destination}");
        Console.WriteLine($"Origin: {Origin}");
        Console.WriteLine($"Type: {Type}");
        Console.WriteLine($"Date: {Date:yyyy-MM-dd}");
        Console.WriteLine($"Time: {Time}");
        if (!string.IsNullOrEmpty(TimePeriod))
        {
            Console.WriteLine($"Time Period: {TimePeriod}");
        }
    }
}