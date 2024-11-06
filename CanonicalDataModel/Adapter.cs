namespace Canonical_Data_Model;

public class Adapter
{
    private AirlineCompany _sasFlight;

    public Adapter(AirlineCompany sasFlight)
    {
        _sasFlight = sasFlight;
    }

    // Transformation method to convert SAS flight to CanonicalFlightInfo
    public CanonicalFlightInfo TransformToCanonical()
    {
        // SAS uses "ArivalDeparture" to define whether it's an arrival or departure.
        string type = _sasFlight.ArivalDeparture == "D" ? "Departure" : "Arrival";

        // Transforming to CanonicalFlightInfo
        return new CanonicalFlightInfo(
            airline: _sasFlight.Airline,
            flightNumber: _sasFlight.FlightNo,
            destination: _sasFlight.Destination,
            origin: _sasFlight.Origin,
            type: type,
            date: _sasFlight.Date,
            time: _sasFlight.Time
        );
    }
}