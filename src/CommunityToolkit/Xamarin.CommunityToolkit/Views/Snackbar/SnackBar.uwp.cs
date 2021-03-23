using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Xamarin.CommunityToolkit.UI.Views.Helpers;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms.Platform.UWP;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
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

		internal void Show(Forms.VisualElement visualElement, SnackBarOptions arguments)
		{
			var snackBarLayout = new SnackBarLayout(arguments);
			var pageControl = Platform.GetRenderer(visualElement).ContainerElement.Parent;
			var grid = (Grid)(FindVisualChildByName<Border>(pageControl, "BottomCommandBarArea")?.Parent ?? throw new NotSupportedException("Anchor Not Supported on UWP"));

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
				arguments.SetResult(true);
			};
			snackBarTimer.Start();
			grid.RowDefinitions.Add(snackBarRow);
			grid.Children.Add(snackBarLayout);
			Grid.SetRow(snackBarLayout, grid.RowDefinitions.Count - 1);
		}
	}
}