using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace SneakerPeeker.Mobile
{
	public partial class PredictionListPage : ContentPage
	{
		ItemsViewModel viewModel;

		public PredictionListPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new ItemsViewModel();
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			var item = args.SelectedItem as Prediction;
			if(item == null)
				return;

			//await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

			// Manually deselect item
			ItemsListView.SelectedItem = null;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if(viewModel.Items.Count == 0)
				viewModel.LoadItemsCommand.Execute(null);
		}
	}
}
