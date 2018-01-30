using ImageSearch.Model;
using ImageSearch.Shared.View;
using ImageSearch.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImageSearch.UWP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        ImageSearchViewModel vm;
        public SearchPage()
        {
            InitializeComponent();
            BindingContext = vm = new ImageSearchViewModel();
            
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as ImageResult;
            if (item == null)
                return;

            await Navigation.PushAsync(new DetailsPage(item, vm));
            
        }
    }
}
