using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SneakerPeeker.Mobile
{
	public interface IDataStore
	{
		Task<IEnumerable<Prediction>> GetPredictionsAsync(bool forceRefresh = false);
		Task<Prediction> MakePredictionAsync(Prediction item);
		Task<Prediction> GetPredictionAsync(string id);
		//Task<bool> DeleteItemAsync(string id);
		//Task<bool> UpdateItemAsync(T item);
	}

	public static class DataStore
	{
		static IDataStore _instance;
		public static IDataStore Instance => _instance ?? (_instance = DependencyService.Get<IDataStore>());
	}
}
