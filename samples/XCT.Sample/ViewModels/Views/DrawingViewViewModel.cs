using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Xamarin.CommunityToolkit.Extensions;
using System.Collections.ObjectModel;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class DrawingViewViewModel : BaseViewModel
	{
		ObservableCollection<Point> macroPoints = new ObservableCollection<Point>();

		public DrawingViewViewModel()
		{
			SetPointsCommand = new Command(() =>
			{
				var points = GeneratePoints(100).ToList();
				MacroPoints = new ObservableCollection<Point>(points);

				MacroPoints2.Clear();
				foreach (var point in points)
				{
					MacroPoints2.Add(point);
				}
				OnPropertyChanged(nameof(MacroPoints2));
			});
			GetPointsCommand = new AsyncCommand(async () =>
			{
				await Application.Current.MainPage.DisplayToastAsync($"PointsCount: {MacroPoints.Count}");
				await Application.Current.MainPage.DisplayToastAsync($"Points2Count: {MacroPoints2.Count}");
			});
		}

		public ICommand SetPointsCommand { get; }

		public ICommand GetPointsCommand { get; }

		public ObservableCollection<Point> MacroPoints
		{
			get => macroPoints;
			set => SetProperty( ref macroPoints, value);
		}

		public ObservableCollection<Point> MacroPoints2 { get; } = new ObservableCollection<Point>();

		static IEnumerable<Point> GeneratePoints(int count)
		{
			var points = new List<Point>();
			for (var i = 0; i < count; i++)
			{
				points.Add(new Point(i, i));
			}

			return points;
		}
	}
}