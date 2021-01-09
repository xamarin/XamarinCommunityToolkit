using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;
using System.Collections.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class DrawingViewPage : BasePage
	{
		public DrawingViewPage()
		{
			InitializeComponent();
			DrawingViewControl.Points = GeneratePoints(200);
			DrawingViewControl.DrawingCompletedCommand = new Command<ObservableCollection<Point>>(points =>
			{
				Logs.Text += "GestureCompletedCommand executed" + Environment.NewLine;
				DrawImage(points);
			});
		}

		void LoadPointsButtonClicked(object sender, EventArgs e) => DrawingViewControl.Points = GeneratePoints(50);

		void DisplayHiddenLabelButtonClicked(object sender, EventArgs e) => HiddenLabel.IsVisible = !HiddenLabel.IsVisible;

		void GetCurrentDrawingViewImageClicked(object sender, EventArgs e)
		{
			var stream = DrawingViewControl.GetImageStream(GestureImage.Width, GestureImage.Height);
			GestureImage.Source = ImageSource.FromStream(() => stream);
		}

		void GetImageClicked(object sender, EventArgs e)
		{
			var points = GeneratePoints(100);
			DrawImage(points);
		}

		static ObservableCollection<Point> GeneratePoints(int count)
		{
			var points = new ObservableCollection<Point>();
			for (var i = 0; i < count; i++)
			{
				points.Add(new Point(i, i));
			}

			return points;
		}

		void DrawImage(IEnumerable<Point> points)
		{
			var stream = DrawingView.GetImageStream(points, new Size(GestureImage.Width, GestureImage.Height), 10,
				Color.White, Color.Black);
			GestureImage.Source = ImageSource.FromStream(() => stream);
		}
	}
}