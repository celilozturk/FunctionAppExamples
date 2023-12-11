using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace FunctionBlobTriggerApp
{
    public  class ResizeFunction
    {
        [FunctionName("ResizeFunction")]
        public async Task Run([BlobTrigger("udemy-pictures/{name}", Connection = "MyAzureStorage")]Stream myBlob, string name, ILogger log, [Blob("udemy-pictures-resize",Connection ="MyAzureStorage")] CloudBlobContainer cloudBlobContainer)
        {
            await cloudBlobContainer.CreateIfNotExistsAsync();
            MemoryStream ms = new MemoryStream();

            var format=await Image.DetectFormatAsync(myBlob);
            var blockBlob=cloudBlobContainer.GetBlockBlobReference($"{Guid.NewGuid()}-100.{format.FileExtensions.First()}");
           // var blockBlob=cloudBlobContainer.GetBlockBlobReference($"{Guid.NewGuid()}-200.{format.FileExtensions.First()}");


            var ResizeImage = Image.Load(myBlob);
            
            ResizeImage.Mutate(x=>x.Resize(100,100));
           

            ResizeImage.Save(ms, format);

            ms.Position = 0;
           await blockBlob.UploadFromStreamAsync(ms);
            ms.Close();
            ms.Dispose();

            log.LogInformation($"Resim resize islemi basaryla gerceklestirildi!");

        }
    }
}
