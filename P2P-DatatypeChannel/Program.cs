namespace Opgave7._1;

class Program
{
    static void Main(string[] args)
    {
        // Kald Producer-klassen for at sende besked
        Producer.SendETA();

        // Start Consumer i en separat klasse for at modtage beskeder
        Consumer consumer = new Consumer();
        consumer.StartConsuming();
    }
}