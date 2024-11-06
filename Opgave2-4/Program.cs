using Systemintegration.Messaging;
using System.Threading.Tasks;

namespace Systemintegration
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var producer = new Producer();
            producer.SendMessage();

            //var consumer = new Consumer();
            //await consumer.ReceiveMessages();
        }
    }
}