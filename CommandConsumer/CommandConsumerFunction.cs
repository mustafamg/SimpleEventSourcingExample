using System;
using System.Collections.Specialized;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CommandConsumer
{
    public static class CommandConsumerFunction
    {
        /********************************************************************************************
         * in case of using Azure functions, no need to add retry logic as it is already implemented.
         * The poison messages are sent to CommandsQueue-poison (this applies to this example only. Usually it sends to queuName-poison queue
         * Please refere to https://docs.microsoft.com/en-us/sandbox/functions-recipes/queue-storage#poison-queue-messages-with-azure-storage-queue-triggers
         * *******************************************************************************************/
        [FunctionName("Function1")]
        public static void Run([QueueTrigger("CommandsQueue", Connection = "")]string serializedCommand, TraceWriter log)
        {
            var command = JsonConvert.DeserializeObject<Models.RestfulCommand>(serializedCommand);
            
            log.Info($"Use httpClient to call url '{command.Uri}' using '{command.Verb}' action with the following data in '{command.Verb}' Body:");
            
            log.Info(command.Argument.ToString());
        }
    }
}
