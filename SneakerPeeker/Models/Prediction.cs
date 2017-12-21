using System;
using System.Collections.Generic;

namespace SneakerPeeker.Mobile
{
	public class Prediction
	{
		public string Id { get; set; }
		public string ProjectId { get; set; }
		public string ImageUrl { get; set; }
		public Dictionary<string, decimal> Results { get; set; }
	}
}
