using System;

namespace Request_Reply
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new RequestReplyServer();
            var client = new RequestReplyClient();

            // Start server in a separate thread
            var serverThread = new System.Threading.Thread(() => server.StartServer());
            serverThread.Start();

            // Give the server some time to start
            System.Threading.Thread.Sleep(1000);

            // Send request from client
            client.SendRequest();
        }
    }
}