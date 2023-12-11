using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FunctionBlobTriggerApp
{
    public class ResizeFunction
    {
        [FunctionName("ResizeFunction")]
        public async Task Run([BlobTrigger("udemy-pictures/{name}", Connection = "MyAzureStorage")]Stream myBlob, string name, ILogger log, [Blob("udemy-pictures-resize/{name}",FileAccess.Write,Connection ="MyAzureStorage")] Stream outputBlobStream)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");



            var format=await Image.DetectFormatAsync(myBlob);

            var ResizeImage = Image.Load(myBlob);
            
            ResizeImage.Mutate(x=>x.Resize(100,100));
            ResizeImage.Save(outputBlobStream, format);
            log.LogInformation($"Resim resize islemi basaryla gerceklestirildi!");

        }
    }
}
