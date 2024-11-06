using System;
using System.Linq;
using System.Xml.Linq;

namespace CargoLoaderTeam;

class Program
{
    static void Main(string[] args)
    {
        // Load the XML file
        XDocument xmlDoc = XDocument.Load("/Users/Sebastian/Desktop/4.SEM/Systemintegration/Rider_Systemintegration/CargoLoaderTeam/FlightDetailsInfoResponse.xml");

        // Extract flight information
        var flight = xmlDoc.Descendants("Flight").FirstOrDefault();
        string flightNumber = flight?.Attribute("number")?.Value;
        string flightDate = flight?.Attribute("Flightdate")?.Value;
        string origin = flight?.Element("Origin")?.Value;
        string destination = flight?.Element("Destination")?.Value;

        // Extract passenger information
        var passenger = xmlDoc.Descendants("Passenger").FirstOrDefault();
        string reservationNumber = passenger?.Element("ReservationNumber")?.Value;
        string firstName = passenger?.Element("FirstName")?.Value;
        string lastName = passenger?.Element("LastName")?.Value;

        // Initialize variables for baggage weights
        double totalPassengerWeight = 0;
        double totalFlightWeight = 0;

        // Iterate through the luggage items
        var luggages = xmlDoc.Descendants("Luggage");
        foreach (var luggage in luggages)
        {
            double weight = double.Parse(luggage.Element("Weight")?.Value);
            totalPassengerWeight += weight; // Add to passenger's total weight
            totalFlightWeight += weight;    // Add to flight's total weight
        }

        // Output flight information
        Console.WriteLine($"Flight Number: {flightNumber}");
        Console.WriteLine($"Flight Date: {flightDate}");
        Console.WriteLine($"Origin: {origin}");
        Console.WriteLine($"Destination: {destination}");
        Console.WriteLine();

        // Output passenger baggage weight
        Console.WriteLine($"Passenger: {firstName} {lastName} (Reservation: {reservationNumber})");
        Console.WriteLine($"Total Baggage Weight for {firstName} {lastName}: {totalPassengerWeight} kg");
        Console.WriteLine();

        // Output total flight baggage weight
        Console.WriteLine($"Total Baggage Weight for Flight {flightNumber}: {totalFlightWeight} kg");
    }
}
