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

namespace ImageSearch.ViewModel
{
    public class ImageSearchViewModel
    {
        public ObservableRangeCollection<ImageResult> Images { get; }

        public ImageResult SelectedImage { get; set; }

        public ImageSearchViewModel()
        {
            Images = new ObservableRangeCollection<ImageResult>();
        }
        
        public async Task<bool> SearchForImagesAsync(string query)
        {
            if(string.IsNullOrWhiteSpace(query))
            {
                await UserDialogs.Instance.AlertAsync("Please search for cute things");
                return false;
            }

            if(!CrossConnectivity.Current.IsConnected)
            {
                await UserDialogs.Instance.AlertAsync("On interwebs :(");
                return false;

            }
            
			//Bing Image API
			var url = $"https://api.cognitive.microsoft.com/bing/v5.0/images/" + 
				      $"search?q={query}" +
					  $"&count=20&offset=0&mkt=en-us&safeSearch=Strict";

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
                return false;
            }

			return true;
        }


   


        public async Task AnalyzeImageAsync(string imageUrl)
        {
            var result = string.Empty;
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
            await UserDialogs.Instance.AlertAsync(result);

        }




        public async Task TakePhotoAndAnalyzeAsync(bool useCamera = true)
        {
            string result = "Error";
            MediaFile file = null;
            IProgressDialog progress;

            try
            {
                await CrossMedia.Current.Initialize();

                if (useCamera)
                {
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "face.jpg",
                        PhotoSize = PhotoSize.Medium
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

            await UserDialogs.Instance.AlertAsync(result);
        }

    }
}
