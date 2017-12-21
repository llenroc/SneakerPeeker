using System;

using Xamarin.Forms;

namespace SneakerPeeker.Mobile
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			DependencyService.Register<CloudDataStore>();
			MainPage = new MainPage();
		}
	}
}
