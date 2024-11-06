namespace Canonical_Data_Model;

public class AirlineCompany
{
    public string Airline { get; set; }            // SAS
    public string FlightNo { get; set; }           // SK239
    public string Destination { get; set; }        // JFK
    public string Origin { get; set; }             // CPH
    public string ArivalDeparture { get; set; }    // D (Departure)
    public DateTime Date { get; set; }             // 6. marts 2017
    public TimeSpan Time { get; set; }             // 16:45

    public AirlineCompany(string airline, string flightNo, string destination, string origin, string arivalDeparture, DateTime date, TimeSpan time)
    {
        Airline = airline;
        FlightNo = flightNo;
        Destination = destination;
        Origin = origin;
        ArivalDeparture = arivalDeparture;
        Date = date;
        Time = time;
    }
    
    
}