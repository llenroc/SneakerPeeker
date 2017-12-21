
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace SneakerPeeker.Backend
{
    public static class MakePrediction
    {
        [FunctionName(nameof(MakePrediction))]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

			string requestBody = new StreamReader(req.Body).ReadToEnd();
            var prediction = JsonConvert.DeserializeObject<Prediction>(requestBody);

            return prediction != null
                ? (ActionResult)new OkObjectResult($"Hello, {prediction.ProjectId}")
                : new BadRequestObjectResult("Please pass a prediction object in the request body");
        }
    }
}
