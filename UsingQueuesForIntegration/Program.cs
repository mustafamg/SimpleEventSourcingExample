using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Models;
using Newtonsoft.Json;

namespace UsingQueuesForIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                CreateNewOrder();
            }
        }

        private static async Task CreateNewOrder()
        {
            var queue = await GetOrCreateQueue();
            var orderCommand = CreateOrderCommand();
            await queue.AddMessageAsync(CreateQueuMessage(orderCommand));

            var GlCommand = CreateGeneralLedgerCommand();
            await queue.AddMessageAsync(CreateQueuMessage(orderCommand));

        }

        private static RestfulCommand CreateGeneralLedgerCommand()
        {
            var command = new RestfulCommand()
            {
                NumberOfTries = 0,
                Uri = "http://localhost:5555/GeneralLedger",
                Verb = HttpVerb.Post
            };
            command.Argument.Add("OrderNumber", new Random(DateTime.Now.Millisecond).Next(int.MaxValue).ToString());
            return command;
        }

        private static CloudQueueMessage CreateQueuMessage(RestfulCommand<int> command)
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            return new CloudQueueMessage(serializedCommand);
        }

        private static RestfulCommand<int> CreateOrderCommand()
        {
            return new RestfulCommand<int>()
            {
                NumberOfTries = 0,
                Uri = "http://localhost:5555/Orders",
                Verb = HttpVerb.Post,
                Argument = new Random(DateTime.Now.Millisecond).Next(int.MaxValue)
            };
        }

        private static  async Task<CloudQueue> GetOrCreateQueue()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                "DefaultEndpointsProtocol=https;AccountName=pointofsaleexample;AccountKey=pvsRdSg4bvBriB1gFsEJir38mc98+t0vSyrybaLsemev8fYFcNJTA10nih37XLKHt8XhW5E7WJ4mDAE8iQyzjQ==;EndpointSuffix=core.windows.net");

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            CloudQueue queue = queueClient.GetQueueReference("CommandsQueue");

            // Create the queue if it doesn't already exist
            await queue.CreateIfNotExistsAsync();
            return queue;
        }
    }
}