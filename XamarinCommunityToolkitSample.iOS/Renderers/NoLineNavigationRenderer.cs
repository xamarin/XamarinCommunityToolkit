using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinCommunityToolkitSample.iOS.Renderers;
using XamarinCommunityToolkitSample.Pages;

[assembly: ExportRenderer(typeof(BaseNavigationPage), typeof(NoLineNavigationPageRenderer))]
namespace XamarinCommunityToolkitSample.iOS.Renderers
{
    public class NoLineNavigationPageRenderer : NavigationRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (Element is NavigationPage)
            {
                //iOS older version fix
                NavigationBar.SetBackgroundImage(new UIKit.UIImage(), UIKit.UIBarMetrics.Default);
                NavigationBar.ShadowImage = new UIKit.UIImage();
                NavigationBar.ClipsToBounds = true;

                try //Newest iOS version fix - trycatch isn't optimal
                {
                    NavigationBar.ScrollEdgeAppearance.ShadowImage = new UIKit.UIImage();
                    NavigationBar.ScrollEdgeAppearance.ShadowColor = null;
                }
                catch (Exception) { }
            }
        }
    }
}
