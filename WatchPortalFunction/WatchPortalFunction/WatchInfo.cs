using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WatchPortalFunction
{
    public static class WatchInfo
    {
        [FunctionName("WatchInfo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string model = req.Query["model"];

            if (model != null)
            {
                dynamic watchInfo = new { Manufacturer = "Abc", CaseType = "Solid", Bezel = "Titanium", Dial = "Roman", CaseFinish = "Silver", Jewels = 15 };

                return (ActionResult)new OkObjectResult($"Watch Details: {watchInfo.Manufacturer}, {watchInfo.CaseType}, {watchInfo.Bezel}, {watchInfo.Dial}, {watchInfo.CaseFinish}, {watchInfo.Jewels}");
            }

            return new BadRequestObjectResult("Please provide a watch model in the query string");

        }
    }
}
