using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Plugin.Connectivity;

namespace SneakerPeeker.Mobile
{
	public class CloudDataStore : IDataStore
	{
		HttpClient client;
		IEnumerable<Prediction> _predictions;

		public CloudDataStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{Keys.AzureFunctionsEndpoint}/");

			_predictions = new List<Prediction>();
		}

		public async Task<IEnumerable<Prediction>> GetPredictionsAsync(bool forceRefresh = false)
		{
			if(forceRefresh && CrossConnectivity.Current.IsConnected)
			{
				var json = await client.GetStringAsync($"api/prediction");
				_predictions = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Prediction>>(json));
			}

			return _predictions;
		}

		public async Task<Prediction> GetPredictionAsync(string id)
		{
			if(id != null && CrossConnectivity.Current.IsConnected)
			{
				var json = await client.GetStringAsync($"api/prediction/{id}");
				return await Task.Run(() => JsonConvert.DeserializeObject<Prediction>(json));
			}

			return null;
		}

		public async Task<Prediction> MakePredictionAsync(Prediction item)
		{
			if(item == null || !CrossConnectivity.Current.IsConnected)
				return null;
			
			var serializedItem = JsonConvert.SerializeObject(item);
			var response = await client.PostAsync($"api/MakePrediction", new StringContent(serializedItem, Encoding.UTF8, "application/json"));
			var result = JsonConvert.DeserializeObject<Prediction>(response.Content.ReadAsStringAsync().Result);
			return result;
		}

		/*
		public async Task<bool> UpdateItemAsync(Item item)
		{
			if(item == null || item.Id == null || !CrossConnectivity.Current.IsConnected)
				return false;

			var serializedItem = JsonConvert.SerializeObject(item);
			var buffer = Encoding.UTF8.GetBytes(serializedItem);
			var byteContent = new ByteArrayContent(buffer);

			var response = await client.PutAsync(new Uri($"api/item/{item.Id}"), byteContent);

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteItemAsync(string id)
		{
			if(string.IsNullOrEmpty(id) && !CrossConnectivity.Current.IsConnected)
				return false;

			var response = await client.DeleteAsync($"api/item/{id}");

			return response.IsSuccessStatusCode;
		}
		*/
	}
}
