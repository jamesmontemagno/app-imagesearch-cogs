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
        Button button;
        ProgressBar progress;
        EditText query;
        FloatingActionButton fab;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main);

            Xamarin.Essentials.Platform.Init(this, bundle);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);


            viewModel = new ImageSearchViewModel();


            progress = FindViewById<ProgressBar>(Resource.Id.my_progress);
            query = FindViewById<EditText>(Resource.Id.my_query);
            button = FindViewById<Button>(Resource.Id.my_button);

            button.Click += async (sender, args) =>
            {
                viewModel.SearchForImageCommand.Execute(null);
            };


            query.Text = viewModel.SearchQuery;
            query.TextChanged += (sender, args) => viewModel.SearchQuery = query.Text;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;


            SetupMainView();
            SetupCamera();
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                switch(e.PropertyName)
                {
                    case nameof(viewModel.IsBusy):
                        {
                            button.Enabled = !viewModel.IsBusy;
                            fab.Enabled = !viewModel.IsBusy;
                            progress.Visibility = viewModel.IsBusy ? ViewStates.Visible : ViewStates.Gone;
                        }
                        break;
                }
            });
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
            fab = FindViewById<FloatingActionButton>(Resource.Id.fab_photo);
            fab.Visibility = ViewStates.Visible;

            fab.Click +=  (sender, args) =>
            {
                viewModel.TakePhotoAndAnalyzeCommand.Execute(null);
            };
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

