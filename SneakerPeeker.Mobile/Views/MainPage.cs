using System;

using Xamarin.Forms;

namespace SneakerPeeker.Mobile
{
	public class MainPage : TabbedPage
	{
		public MainPage()
		{
			var identifyPage = new NavigationPage(new MakePredictionPage())
			{
				Title = "Predict"
			};

			var itemsPage = new NavigationPage(new PredictionListPage())
			{
				Title = "History"
			};

			switch(Device.RuntimePlatform)
			{
				case Device.iOS:
					itemsPage.Icon = "tab_feed.png";
					identifyPage.Icon = "tab_about.png";
					break;
			}

			Children.Add(identifyPage);
			Children.Add(itemsPage);

			Title = Children[0].Title;
		}

		protected override void OnCurrentPageChanged()
		{
			base.OnCurrentPageChanged();
			Title = CurrentPage?.Title ?? string.Empty;
		}
	}
}
