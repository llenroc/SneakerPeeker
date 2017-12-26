
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Cognitive.CustomVision;
using System;
using Microsoft.Cognitive.CustomVision.Models;
using System.Collections.Generic;

namespace SneakerPeeker.Backend
{
    public static class MakePrediction
    {
        [FunctionName(nameof(MakePrediction))]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

			try
			{
				string requestBody = new StreamReader(req.Body).ReadToEnd();
				var prediction = JsonConvert.DeserializeObject<Prediction>(requestBody);

				var api = new TrainingApi(new TrainingApiCredentials(prediction.TrainingId));
				var account = api.GetAccountInfo();
				var predictionKey = account.Keys.PredictionKeys.PrimaryKey;

				var creds = new PredictionEndpointCredentials(predictionKey);
				var endpoint = new PredictionEndpoint(creds);

				//This is where we run our prediction against the default iteration
				var result = endpoint.PredictImageUrl(new Guid(prediction.ProjectId), new ImageUrl(prediction.ImageUrl));
				prediction.Results = new Dictionary<string, decimal>();
				// Loop over each prediction and write out the results
				foreach (var outcome in result.Predictions)
				{
					if (outcome.Probability > .70)
						prediction.Results.Add(outcome.Tag, (decimal)outcome.Probability);
				}

				return (ActionResult)new OkObjectResult(prediction);
			}
			catch (Exception e)
			{
				return new BadRequestObjectResult(e.GetBaseException().Message);

			}
		}
    }
}
