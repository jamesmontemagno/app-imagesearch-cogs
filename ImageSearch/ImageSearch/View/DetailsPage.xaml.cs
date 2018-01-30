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
        public DetailsPage()
        {
            InitializeComponent();
            this.result = new ImageResult
            {
                ContextLink = "http://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Capuchin_Costa_Rica.jpg/200px-Capuchin_Costa_Rica.jpg",
                FileFormat = "jpg",
                ImageLink = "http://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Capuchin_Costa_Rica.jpg/200px-Capuchin_Costa_Rica.jpg",
                ThumbnailLink = "http://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Capuchin_Costa_Rica.jpg/200px-Capuchin_Costa_Rica.jpg",
                Title="Capuchin"
            };

            this.viewModel = new ImageSearchViewModel
            {
                SelectedImage = this.result
            };

            BindingContext = viewModel;
        }
        public DetailsPage (ImageResult result, ImageSearchViewModel viewModel)
		{
			InitializeComponent ();
            this.result = result;
            
            this.viewModel = viewModel;
            this.viewModel.SelectedImage = result;

            BindingContext = viewModel;

        }
    }
}