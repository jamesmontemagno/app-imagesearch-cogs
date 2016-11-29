using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using ImageSearch.Droid.Adapters;
using ImageSearch.ViewModel;
using Plugin.Permissions;
using Acr.UserDialogs;

namespace ImageSearch.Droid
{
    [Activity(Label = "Image Search", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : BaseActivity
    {
        RecyclerView recyclerView;
        RecyclerView.LayoutManager layoutManager;
        ImageAdapter adapter;
        ImageSearchViewModel viewModel;
        ProgressBar progressBar;

        protected override int LayoutResource => Resource.Layout.main;
        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            viewModel = new ImageSearchViewModel();

            //Setup RecyclerView

            adapter = new ImageAdapter(this, viewModel);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.SetAdapter(adapter);

            layoutManager = new GridLayoutManager(this, 2);

            recyclerView.SetLayoutManager(layoutManager);

            progressBar = FindViewById<ProgressBar>(Resource.Id.my_progress);
            progressBar.Visibility = ViewStates.Gone;

            //get edit text here

            // Get our button from the layout resource,
            // and attach an event to it


            

            UserDialogs.Init(this);
            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(false);

        }
    }
}

