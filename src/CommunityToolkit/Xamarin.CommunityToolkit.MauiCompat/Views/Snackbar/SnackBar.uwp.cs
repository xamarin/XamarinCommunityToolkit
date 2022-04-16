using System;using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.Maui.Controls;
using Microsoft.UI.Xaml.Media;
using Xamarin.CommunityToolkit.UI.Views.Helpers;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using Border = Microsoft.UI.Xaml.Controls.Border;
using Grid = Microsoft.UI.Xaml.Controls.Grid;
using RowDefinition = Microsoft.UI.Xaml.Controls.RowDefinition;

namespace Xamarin.CommunityToolkit.UI.Views
{
	partial class SnackBar
	{
		DispatcherTimer? snackBarTimer;

		T? FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
		{
			var childrenCount = VisualTreeHelper.GetChildrenCount(parent);

			for (var i = 0; i < childrenCount; i++)
			{
				var child = VisualTreeHelper.GetChild(parent, i);
				var controlName = child.GetValue(FrameworkElement.NameProperty) as string;

				if (controlName == name)
				{
					return child as T;
				}

				var control = FindVisualChildByName<T>(child, name);
				if (control != null)
					return control;
			}

			return null;
		}

		internal partial ValueTask Show(VisualElement visualElement, SnackBarOptions arguments)
		{
			var snackBarLayout = new SnackBarLayout(arguments);
			var pageControl = Platform.GetRenderer(visualElement).ContainerElement.Parent;

			var grid = (Grid)(FindVisualChildByName<Border>(pageControl, "BottomCommandBarArea")?.Parent ?? throw new NotSupportedException("Unable to find Snackbar/Toast container. Make sure your page is in NavigationPage. AnchorView is not supported in UWP."));
			var snackBarRow = new RowDefinition() { Height = GridLength.Auto };
			snackBarTimer = new DispatcherTimer { Interval = arguments.Duration };
			snackBarTimer.Tick += (sender, e) =>
			{
				grid.Children.Remove(snackBarLayout);
				grid.RowDefinitions.Remove(snackBarRow);
				snackBarTimer.Stop();
				arguments.SetResult(false);
			};
			snackBarLayout.OnSnackBarActionExecuted += () =>
			{
				grid.Children.Remove(snackBarLayout);
				grid.RowDefinitions.Remove(snackBarRow);
				snackBarTimer.Stop();
			};
			snackBarTimer.Start();
			grid.RowDefinitions.Add(snackBarRow);
			grid.Children.Add(snackBarLayout);
			Grid.SetRow(snackBarLayout, grid.RowDefinitions.Count - 1);
			return default;
		}
	}
}