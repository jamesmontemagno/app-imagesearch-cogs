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

            ButtonSearch.TouchUpInside += async (sender, args) =>
            {
                ButtonSearch.Enabled = false;

                ActivityIsLoading.StartAnimating();
                

                await viewModel.SearchForImagesAsync(TextFieldQuery.Text);
                CollectionViewImages.ReloadData();
                

                ActivityIsLoading.StopAnimating();

                ButtonSearch.Enabled = true;
            };

            SetupCamera();
            
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
                UIBarButtonSystemItem.Camera, async delegate
                {
                    ButtonSearch.Enabled = false;
                    ActivityIsLoading.StartAnimating();

                    await viewModel.TakePhotoAndAnalyzeAsync(false);

                    ButtonSearch.Enabled = true;
                    ActivityIsLoading.StopAnimating();
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

