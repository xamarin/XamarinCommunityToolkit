using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms.Platform.UWP;
using Xamarin.CommunityToolkit.UI.Views.Helpers;

namespace Xamarin.CommunityToolkit.UI.Views
{
	class SnackBar
	{
		static DispatcherTimer snackbarTimer;

		T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
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

		internal void Show(Forms.Page page, SnackbarArguments arguments)
		{

			var snackBarLayout = new SnackbarLayout(arguments.Message, arguments.ActionButtonText, arguments.Action);
			var pageControl = Platform.GetRenderer(page).ContainerElement.Parent;
			var grid = FindVisualChildByName<Border>(pageControl, "BottomCommandBarArea").Parent as Grid;
			var snackBarRow = new RowDefinition() { Height = GridLength.Auto };
			snackbarTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(arguments.Duration) };
			snackbarTimer.Tick += (sender, e) =>
			{
				grid.Children.Remove(snackBarLayout);
				grid.RowDefinitions.Remove(snackBarRow);
				snackbarTimer.Stop();
				arguments.SetResult(false);
			};
			snackBarLayout.OnSnackbarActionExecuted += () =>
			{
				grid.Children.Remove(snackBarLayout);
				grid.RowDefinitions.Remove(snackBarRow);
				snackbarTimer.Stop();
				arguments.SetResult(true);
			};
			snackbarTimer.Start();
			grid.RowDefinitions.Add(snackBarRow);
			grid.Children.Add(snackBarLayout);
			Grid.SetRow(snackBarLayout, grid.RowDefinitions.Count - 1);
		}
	}
}