using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ImageSearch.UWP
{
    class FormsApp : Application
    {
        public FormsApp()
        {
            MainPage = new NavigationPage(new SearchPage());
        }
    }
}
