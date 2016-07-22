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

namespace ImageSearch.ViewModel
{
    public class ImageSearchViewModel
    {
        public ObservableRangeCollection<ImageResult> Images { get; }

        public ImageSearchViewModel()
        {
            Images = new ObservableRangeCollection<ImageResult>();
        }
        
        public async Task<bool> SearchForImagesAsync(string query)
        {
			//Bing Image API
			var url = $"https://api.cognitive.microsoft.com/bing/v5.0/images/" + 
				      $"search?q={query}" +
					  $"&count=20&offset=0&mkt=en-us&safeSearch=Strict";

			try
			{
                var headerKey = "Ocp-Apim-Subscription-Key";

                
			}
			catch (Exception ex)
			{	
				await UserDialogs.Instance.AlertAsync("Unable to query images: " + ex.Message);
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
            try
            {

                await CrossMedia.Current.Initialize();


                if(useCamera)
                {
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Samples",
                        Name = "test.jpg",
                        SaveToAlbum = true
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

            await UserDialogs.Instance.AlertAsync(result, "Emotion", "OK");
        }

    }
}
