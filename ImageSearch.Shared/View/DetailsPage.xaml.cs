using ImageSearch.Model;
using ImageSearch.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageSearch.Shared.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DetailsPage : ContentPage
	{
        ImageResult result;
        ImageSearchViewModel viewModel;
        public DetailsPage (ImageResult result, ImageSearchViewModel viewModel)
		{
			InitializeComponent ();
            this.result = result;
            
            this.viewModel = viewModel;
            this.viewModel.SelectedImage = result;

            BindingContext = viewModel;

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var image = result.ThumbnailLink;
            await viewModel.AnalyzeImageAsync(image);

        }
    }
}