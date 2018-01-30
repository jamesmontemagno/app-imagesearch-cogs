using System;
using Foundation;
using UIKit;



using ImageSearch.ViewModel;
using SDWebImage;
using ImageSearch.Shared.View;
using Xamarin.Forms;

namespace ImageSearch.iOS
{
    public partial class ViewController : UIViewController, IUICollectionViewDataSource, IUICollectionViewDelegate
	{

        ImageSearchViewModel viewModel;


		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            viewModel = new ImageSearchViewModel();

            CollectionViewImages.WeakDataSource = this;
            CollectionViewImages.WeakDelegate = this;

            viewModel.Images.CollectionChanged += (sender, args) => BeginInvokeOnMainThread(() => CollectionViewImages.ReloadData());
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            ButtonSearch.TouchUpInside += (sender, args) =>
            {
                viewModel.SearchForImageCommand.Execute(null);
            };

            SetupCamera();
            
		}

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            BeginInvokeOnMainThread(() =>
            {
                switch (e.PropertyName)
                {
                    case nameof(viewModel.IsBusy):
                        {
                            ButtonSearch.Enabled = !viewModel.IsBusy;
                            if (viewModel.IsBusy)
                                ActivityIsLoading.StartAnimating();
                            else
                                ActivityIsLoading.StopAnimating();
                        }
                        break;
                }
            });
        }

        public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}



        public nint GetItemsCount(UICollectionView collectionView, nint section) => 
            viewModel.Images.Count;

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell("imagecell", indexPath) as ImageCell;

            var item = viewModel.Images[indexPath.Row];

            cell.Caption.Text = item.Title;

            cell.Image.SetImage(new NSUrl(item.ThumbnailLink));



            return cell;
        }



        void SetupCamera()
        {
            NavigationItem.RightBarButtonItem = new UIBarButtonItem(
                UIBarButtonSystemItem.Camera,  delegate
                {
                     viewModel.TakePhotoAndAnalyzeCommand.Execute(false);
                });
        }

       


        [Export("collectionView:didSelectItemAtIndexPath:")]
        public void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = viewModel.Images[indexPath.Row];

            var page = new DetailsPage(item, viewModel);

            var controller = page.CreateViewController();
            controller.EdgesForExtendedLayout = UIRectEdge.None;

            NavigationController.PushViewController(controller, true);
        }
    }
}

