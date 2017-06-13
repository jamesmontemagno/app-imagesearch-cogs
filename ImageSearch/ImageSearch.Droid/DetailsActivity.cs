using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ImageSearch.ViewModel;
using Android.Support.V7.App;

using Toolbar = Android.Support.V7.Widget.Toolbar;
using Xamarin.Forms;
using ImageSearch.Shared.View;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms.Platform.Android;

namespace ImageSearch.Droid
{
    [Activity(Label = "Details", Icon = "@drawable/icon")]
    public class DetailsActivity : AppCompatActivity
    {

        public static ImageSearchViewModel ViewModel { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.details);

            Xamarin.Forms.Forms.Init(this, bundle);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);


            SupportActionBar.Title = ViewModel.SelectedImage.Title;
            // #1 Initialize
            Forms.Init(this, null);
            // #2 Use it
            var frag = new DetailsPage(ViewModel.SelectedImage, ViewModel).CreateFragment(this);

            var ft = FragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.fragment_frame_layout, frag, "main");
            ft.Commit();

        }
    }
}