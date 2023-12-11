using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionBlobTriggerApp
{
    public class Function1
    {
        //public void Run([BlobTrigger("udemy-pictures/{name}.png", Connection = "MyAzureStorage")]Stream myBlob, string name, ILogger log)
        [FunctionName("Function1")]
        public void Run([BlobTrigger("udemy-pictures/{name}.jpg", Connection = "MyAzureStorage")]Stream myBlob, string name, ILogger log)

        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
