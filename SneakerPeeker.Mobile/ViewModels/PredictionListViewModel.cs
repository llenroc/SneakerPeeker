using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SneakerPeeker.Mobile
{
	public class ItemsViewModel : BaseViewModel
	{
		public ObservableCollection<Prediction> Items { get; set; }
		public Command LoadItemsCommand { get; set; }

		public ItemsViewModel()
		{
			Title = "Past Predictions";
			Items = new ObservableCollection<Prediction>();
			LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

			//MessagingCenter.Subscribe<NewItemPage, Prediction>(this, "AddItem", async (obj, item) =>
			//{
			//	var _item = item as Prediction;
			//	Items.Add(_item);
			//	_item = await DataStore.MakePredictionAsync(item);
			//});
		}

		async Task ExecuteLoadItemsCommand()
		{
			if(IsBusy)
				return;

			IsBusy = true;

			try
			{
				Items.Clear();
				var items = await DataStore.Instance.GetPredictionsAsync(true);
				foreach(var item in items)
				{
					Items.Add(item);
				}
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}
