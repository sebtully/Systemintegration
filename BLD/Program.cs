using System;
using System.Threading.Tasks;

namespace BLD;

abstract class Program
{
    static void Main(string[] args)
    {
        // Simulating baggage transaction message
        string baggageTransactionXml = @"<?xml version='1.0' ?>
        <BaggageTransaction>
            <TransactionId>1234</TransactionId>
            <FlightNumber>SW497</FlightNumber>
            <AirlineCompany>South West Airlines</AirlineCompany>
            <GateNumber>14</GateNumber>
            <Weight>14.55</Weight>
            <Class>A2</Class>
            <Priority>2nd</Priority>
            <Destination>AMS</Destination>
        </BaggageTransaction>";

        // Start sending baggage transactions
        Task.Run(() =>
        {
            BaggageLoadingSystem.SendBaggageTransaction(baggageTransactionXml);
        });

        // Start five consumers simulating five identical programs
        for (int i = 0; i < 5; i++)
        {
            Task.Run(() =>
            {
                BaggageStatisticsConsumer.StartListening();
            });
        }

        // Keep the main thread alive
        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();
    }
}