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
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Content.PM;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace ImageSearch.Droid
{
    [Activity(Label = "Image Search", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {

        ImageSearchViewModel viewModel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);


            viewModel = new ImageSearchViewModel();


            var progress = FindViewById<ProgressBar>(Resource.Id.my_progress);
            var query = FindViewById<EditText>(Resource.Id.my_query);
            var button = FindViewById<Button>(Resource.Id.my_button);


            button.Click += async (sender, args) =>
            {
                button.Enabled = false;
                progress.Visibility = ViewStates.Visible;

                await viewModel.SearchForImagesAsync(query.Text.Trim());

                progress.Visibility = ViewStates.Gone;
                button.Enabled = true;
            };
           

            SetupMainView();
            SetupCamera();
        }




        RecyclerView recyclerView;
        RecyclerView.LayoutManager layoutManager;
        ImageAdapter adapter;
        void SetupMainView()
        {
            adapter = new ImageAdapter(this, viewModel);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.SetAdapter(adapter);

            layoutManager = new GridLayoutManager(this, 2);

            recyclerView.SetLayoutManager(layoutManager);

            adapter.ItemClick += (sender, args) =>
            {
                var image = viewModel.Images[args.Position];
                viewModel.SelectedImage = image;
                DetailsActivity.ViewModel = viewModel;

                StartActivity(typeof(DetailsActivity));
            };
        }

        void SetupCamera()
        {
            var fab = FindViewById<FloatingActionButton>(Resource.Id.fab_photo);
            fab.Visibility = ViewStates.Visible;

            fab.Click += async (sender, args) =>
            {
                fab.Enabled = false;
                await viewModel.TakePhotoAndAnalyzeAsync();
                fab.Enabled = true;
            };
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

