using System;

namespace Publisher_Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tast (1 eller 2)");
            Console.WriteLine("1. Send ETA-besked (Airport Information Center)");
            Console.WriteLine("2. Modtag ETA-besked (Flyselskab Subscriber)");
            var choice = Console.ReadLine();

            if (choice == "1")
            {
                var publisher = new Publisher();
                publisher.RunPublisher();
            }
            else if (choice == "2")
            {
                Console.WriteLine("Indtast flyselskab:");
                var airline = Console.ReadLine();
                var subscriber = new Subscriber();
                subscriber.RunSubscriber(airline);
            }
            else
            {
                Console.WriteLine("Ugyldigt valg. Afslutter programmet.");
            }
        }
    }
}