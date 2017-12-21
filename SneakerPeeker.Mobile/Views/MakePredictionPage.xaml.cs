using System;

using Xamarin.Forms;

namespace SneakerPeeker.Mobile
{
	public partial class MakePredictionPage : ContentPage
	{
		MakePredictionViewModel _viewModel = new MakePredictionViewModel();

		public MakePredictionPage()
		{
			BindingContext = _viewModel;
			InitializeComponent();
		}
	}
}
