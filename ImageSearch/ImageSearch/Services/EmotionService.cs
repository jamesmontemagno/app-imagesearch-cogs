using Acr.UserDialogs;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSearch.Services
{
    public class EmotionService
    {
        private static async Task<FaceResult[]> GetHappinessAsync(Stream stream)
        {
            var loading = UserDialogs.Instance.Loading("Analyzing...");
            loading.Show();

            var emotionClient = new EmotionServiceClient(
                CognitiveServicesKeys.Emotion);

            try
            {
                var emotionResults = await emotionClient.RecognizeAsync(stream);

                if (emotionResults == null || emotionResults.Count() == 0)
                {
                    throw new Exception("Can't detect face");
                }

                return emotionResults;
            }
            finally
            {
                loading.Hide();
            }
        }

        //Average happiness calculation in case of multiple people
        public static async Task<float> GetAverageHappinessScoreAsync(Stream stream)
        {
            FaceResult[] emotionResults = await GetHappinessAsync(stream);

            float score = 0;
            foreach (var emotionResult in emotionResults)
            {
                score = score + emotionResult.FaceAttributes.Emotion.Happiness;
            }

            return score / emotionResults.Count();
        }

        public static string GetHappinessMessage(float score)
        {
            score = score * 100;
            double result = Math.Round(score, 2);

            if (score >= 51)
                return result + " % :-)";
            else
                return result + "% :-(";
        }
    }
}
