using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace SneakerPeeker.Mobile
{
	public class MakePredictionViewModel : BaseViewModel
	{
		public ICommand TakePictureCommand => new Command(TakePicture);
		public ICommand MakePredictionCommand => new Command(MakePrediction);

		string _status = "Snap a pic of a shoe to get started";
		public string Status
		{
			get { return _status; }
			set { SetProperty(ref _status, value); }
		}

		public bool HasImageSource => ImageSource != null;

		string _imageStoreUrl;
		ImageSource _imageSource;
		public ImageSource ImageSource
		{
			get { return _imageSource; }
			set { SetProperty(ref _imageSource, value); OnPropertyChanged(nameof(HasImageSource)); }
		}

		#region Take/Upload Picture

		async void TakePicture()
		{
			if(!CrossMedia.Current.IsCameraAvailable)
			{
				return;
			}

			var options = new StoreCameraMediaOptions
			{
				CompressionQuality = 50,
				PhotoSize = PhotoSize.Small,
			};

			var file = await CrossMedia.Current.TakePhotoAsync(options);

			if(file == null)
				return;

			IsBusy = true;
			Status = "Uploading picture...";

			var stream = file.GetStream();
			file.Dispose();

			_imageStoreUrl = await UploadImage(stream);
			ImageSource = ImageSource.FromUri(new Uri(_imageStoreUrl));

			Status = null;
			IsBusy = false;
		}

		async Task<string> UploadImage(Stream stream)
		{
			var blobId = Guid.NewGuid().ToString() + ".jpg";

			var container = new CloudBlobContainer(new Uri(Keys.AzureBlobStorageEndpoint));
			var blockBlob = container.GetBlockBlobReference(blobId);
			blockBlob.Properties.ContentType = "image/jpg";

			await blockBlob.UploadFromStreamAsync(stream);
			return blockBlob.StorageUri.PrimaryUri.ToString();
		}

		#endregion

		#region Make Prediction

		async void MakePrediction()
		{
			if(_imageStoreUrl == null)
			{
				Status = "Please take a picture first";
				return;
			}

			var prediction = new Prediction
			{
				ProjectId = Keys.CustomVisionProjectId,
				TrainingId = Keys.CustomVisionTrainingId,
			 	ImageUrl = _imageStoreUrl
			};

			IsBusy = true;
			Status = "Analyzing picture...";

			var result = await DataStore.Instance.MakePredictionAsync(prediction);

			if(result == null)
				return;

			var msg = "";
			foreach(var tag in result.Results)
			{
				msg += $"{tag.Key}, ";
			}

			Status = msg.TrimEnd(',').Trim();
			IsBusy = false;
		}

		#endregion
	}
}
