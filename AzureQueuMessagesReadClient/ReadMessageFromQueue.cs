using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;

internal class ReadMessageFromQueue
{
    static QueueClient queueClient;
    private static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json");
        IConfiguration config = builder.Build();
        string connectionString = config["StorageConnectionString"];
        string queueName = "demoqueue";
        queueClient = new QueueClient(connectionString, queueName);
        queueClient.CreateIfNotExists();

        while (true)
        {
            QueueMessage[] retrievedMessages = queueClient.ReceiveMessages(1); //Fetches only one message from queue (visibilitytimeout=30)
            if (retrievedMessages.Length == 0)
                Console.WriteLine("No Messages in last 5 secs...");
            foreach (var msg in retrievedMessages)
            {
                Console.WriteLine($"Dequeued message: '{msg.Body}'");
                queueClient.DeleteMessage(msg.MessageId, msg.PopReceipt);
            }
            System.Threading.Thread.Sleep(5000);
        }
    }

}