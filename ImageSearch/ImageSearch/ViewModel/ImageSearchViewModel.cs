using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using ImageSearch.Services;
using System.Net.Http;
using Newtonsoft.Json;
using ImageSearch.Model;
using Plugin.Media;
using Acr.UserDialogs;
using Plugin.Media.Abstractions;
using ImageSearch.Model.BingSearch;
using Plugin.Connectivity;
using Xamarin.Forms;
using System.Windows.Input;

namespace ImageSearch.ViewModel
{
    public class ImageSearchViewModel : BaseViewModel
    {

        string searchQuery = "Happy Guy";
        public string SearchQuery
        {
            get => searchQuery;
            set => SetProperty(ref searchQuery, value);
        }


        public ObservableRangeCollection<ImageResult> Images { get; }

        public ImageResult SelectedImage { get; set; }
        

        public ImageSearchViewModel()
        {
            Images = new ObservableRangeCollection<ImageResult>();
            SearchForImageCommand = new Command(async () => await SearchForImagesAsync());
            AnalyzeImageCommand = new Command<string>(async (query) => await AnalyzeImageAsync(query));
            TakePhotoAndAnalyzeCommand = new Command<bool>(async (useCamera) => await TakePhotoAndAnalyzeAsync(useCamera));
        }

        public ICommand SearchForImageCommand { get; }

        async Task SearchForImagesAsync()
        {
            if (IsBusy)
                return;


            if(string.IsNullOrWhiteSpace(SearchQuery))
            {
                await UserDialogs.Instance.AlertAsync("Please search for cute things");
                return;
            }

            if(!CrossConnectivity.Current.IsConnected)
            {
                await UserDialogs.Instance.AlertAsync("On interwebs :(");
                return;

            }
            
			//Bing Image API
			var url = $"https://api.cognitive.microsoft.com/bing/v7.0/images/" + 
				      $"search?q={SearchQuery}" +
					  $"&count=20&offset=0&mkt=en-us&safeSearch=Strict";

            IsBusy = true;
            try
            {
                var headerKey = "Ocp-Apim-Subscription-Key";
                var headerValue = CognitiveServicesKeys.BingSearch;

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add(
                    headerKey, headerValue);

                var json = await client.GetStringAsync(url);

                var result = JsonConvert.DeserializeObject<SearchResult>(
                    json);

                var images = result.Images.Select(i => new ImageResult
                {
                    ContextLink = i.HostPageUrl,
                    FileFormat = i.EncodingFormat,
                    ImageLink = i.ContentUrl,
                    ThumbnailLink = i.ThumbnailUrl ?? i.ContentUrl,
                    Title = i.Name
                });

                Images.ReplaceRange(images);
               
            }
            catch (Exception ex)
            {

                await UserDialogs.Instance.AlertAsync("Something went terribly wrong, please open a ticket with support.");
                return;
            }
            finally
            {
                IsBusy = false;
            }

			return;
        }

        public Command<string> AnalyzeImageCommand { get; }
        async Task AnalyzeImageAsync(string imageUrl)
        {
            if (IsBusy)
                return;

            var result = string.Empty;
            IsBusy = true;
            try
            {
                using (var client = new HttpClient())
                {
                    var stream = await client.GetStreamAsync(imageUrl);

                    var emotion = await EmotionService.GetAverageHappinessScoreAsync(stream);

                    result = EmotionService.GetHappinessMessage(emotion);
                }
            }
            catch(Exception ex)
            {
                result =  "Unable to analyze image";
            }
            finally
            {
                IsBusy = false;
            }
            await UserDialogs.Instance.AlertAsync(result);

        }


        public Command<bool> TakePhotoAndAnalyzeCommand { get; }

        async Task TakePhotoAndAnalyzeAsync(bool useCamera = true)
        {
            if (IsBusy)
                return;

            string result = "Error";
            MediaFile file = null;

            try
            {
                IsBusy = true;
                await CrossMedia.Current.Initialize();

                if (useCamera)
                {
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "face.jpg",
                        PhotoSize = PhotoSize.Medium,
                        DefaultCamera = CameraDevice.Front
                    });
                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync();
                }
               

                if (file == null)
                    result = "No photo taken.";
                else
                {
                    var emotion = await EmotionService.GetAverageHappinessScoreAsync(file.GetStream());

                    result = EmotionService.GetHappinessMessage(emotion);
                }
            }
            catch(Exception ex)
            {
                result =  ex.Message;
            }
            finally
            {
                IsBusy = false;
            }

            await UserDialogs.Instance.AlertAsync(result);
        }

    }
}
