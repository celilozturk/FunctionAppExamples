using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EFCoreFunctionApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreFunctionApp
{
    public  class ProductFunction
    {

        private readonly AppDbContext _appDbContext;
        private const string Route = "Products";

        public ProductFunction(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [FunctionName("GetProducts")]
        public  async Task<IActionResult> GetProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Tum urunleri getir!");

            var products = await _appDbContext.Products.ToListAsync();
            
            return new OkObjectResult(products);
            
        }

        [FunctionName("SaveProducts")]
        public async Task<IActionResult> SaveProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Route)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Yeni urun ekle");

           string body=await new StreamReader(req.Body).ReadToEndAsync();
            var newProduct =JsonConvert.DeserializeObject<Product>(body);
            _appDbContext.Products.Add(newProduct);
            await _appDbContext.SaveChangesAsync();
            return new OkObjectResult(newProduct); 

        }
    }
}
