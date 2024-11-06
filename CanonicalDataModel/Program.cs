namespace Canonical_Data_Model;

class Program
{
    static void Main(string[] args)
    {
        // Opret en instans af AirlineCompanySAS
        AirlineCompany sasFlight = new AirlineCompany(
            airline: "SAS",
            flightNo: "SK239",
            destination: "JFK",
            origin: "CPH",
            arivalDeparture: "D",
            date: new DateTime(2017, 3, 6),
            time: new TimeSpan(16, 45, 0)
        );

        // Brug adapteren til at transformere SAS-flyet til Canonical Data Model
        Adapter adapter = new Adapter(sasFlight);
        CanonicalFlightInfo canonicalFlight = adapter.TransformToCanonical();

        // Udskriv de transformerede data
        canonicalFlight.PrintFlightInfo();
    }
}