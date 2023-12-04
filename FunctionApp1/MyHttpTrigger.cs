using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp1
{
    public static class MyHttpTrigger
    {
        [FunctionName("MyHttpTrigger")]
        public static async Task<IActionResult> GetProducts(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Products")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            //string id = req.Query["id"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            //###################Tip Guvenli Kullanim
            //Product product=JsonConvert.DeserializeObject<Product>(requestBody);
            //return new OkObjectResult(product);

            //####### !!!!!!!! Tip Guvenli Degil
            dynamic data=JsonConvert.DeserializeObject(requestBody);
            return new OkObjectResult($"{data.Id}-{data.Name}-{data.Price}-{data.Category}");




            //return NotFoundResult()
            //normal mvc return ok(data)


        }

        [FunctionName("MyHttpTrigger2")]
        public static  IActionResult GetProductById(
         [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Products/{id}")] HttpRequest req,
         ILogger log,int id)
        {
            log.LogInformation("Gelen Id:"+id);

            return new OkResult();
        }
    }
}
