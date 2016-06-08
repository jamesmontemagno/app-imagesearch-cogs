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
            var url = $"https://www.googleapis.com/customsearch/v1" +
                $"?q={query}" +
                $"&searchType=image&key={GoogleSearchService.APIKey}" +
                $"&cx={GoogleSearchService.CX}";

            try
            {
                using (var client = new HttpClient())
                {
                    var json = await client.GetStringAsync(url);

                    var result = JsonConvert.DeserializeObject<SearchResult>(json);

                    Images.ReplaceRange(result.Items.Select(i => new ImageResult
                    {
                        ContextLink = i.Image.ContextLink,
                        FileFormat = i.FileFormat,
                        ImageLink = i.Link,
                        ThumbnailLink = i.Image.ThumbnailLink,
                        Title = i.Title
                    }));
                }
                

            }
            catch (Exception ex)
            {
                //TODO Log stuff!
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
