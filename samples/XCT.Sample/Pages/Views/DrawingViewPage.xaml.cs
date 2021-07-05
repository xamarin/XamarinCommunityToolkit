using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views;
using System.Collections.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views
{
	public partial class DrawingViewPage : BasePage
	{
		static Random random = new Random();

		public DrawingViewPage()
		{
			InitializeComponent();
			DrawingViewControl.Lines = GenerateLines(5);
			DrawingViewControl.DrawingLineCompletedCommand = new Command<Line>(line =>
			{
				Logs.Text += "GestureCompletedCommand executed" + Environment.NewLine;
				DrawImage(line);
			});

			BindingContext = this;
		}

		void LoadPointsButtonClicked(object sender, EventArgs e) => DrawingViewControl.Lines = GenerateLines(50);

		void DisplayHiddenLabelButtonClicked(object sender, EventArgs e) =>
			HiddenLabel.IsVisible = !HiddenLabel.IsVisible;

		void GetCurrentDrawingViewImageClicked(object sender, EventArgs e)
		{
			//var stream = DrawingViewControl.GetImageStream(GestureImage.Width, GestureImage.Height);
			//GestureImage.Source = ImageSource.FromStream(() => stream);
		}

		void GetImageClicked(object sender, EventArgs e)
		{
			var lines = GenerateLines(10);
			DrawImage(lines[0]);
		}

		ObservableCollection<Line> GenerateLines(int count)
		{
			var lines = new ObservableCollection<Line>();
			for (var i = 0; i < count; i++)
			{
				lines.Add(new Line()
				{
					Points = GeneratePoints(10),
					LineColor = Color.FromRgb(random.Next(255), random.Next(255), random.Next(255)),
					LineWidth = 10,
					EnableSmoothedPath = false,
					Granularity = 5
				});
			}

			return lines;
		}

		ObservableCollection<Point> GeneratePoints(int count)
		{
			var points = new ObservableCollection<Point>();
			for (var i = 0; i < count; i++)
			{
				points.Add(new Point(random.Next(1, 100), random.Next(1, 100)));
			}

			return points;
		}

		void DrawImage(Line line)
		{
			var stream = DrawingView.GetImageStream(line.Points, new Size(GestureImage.Width, GestureImage.Height), 10,
				Color.White, Color.Black);
			GestureImage.Source = ImageSource.FromStream(() => stream);
		}

		private void AddNewLine(object sender, EventArgs e)
		{
			DrawingViewControl.Lines.Add(new Line()
			{
				Points = GeneratePoints(10),
				LineColor = Color.FromRgb(random.Next(255), random.Next(255), random.Next(255)),
				LineWidth = 10,
				EnableSmoothedPath = true,
				Granularity = 5
			});
		}
	}
}