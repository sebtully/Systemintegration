namespace CBP;

class Program
{
    static void Main(string[] args)
    {
        // Send passenger info
        CBPController.SendPassengerInfo();

        // Start listeners for each country
        NationalCheckConsumer.StartListening("DK");
        NationalCheckConsumer.StartListening("US");
    }
}