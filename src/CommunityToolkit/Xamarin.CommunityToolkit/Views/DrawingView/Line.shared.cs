using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class Line : BindableObject
	{
		const int minValueGranularity = 5;

		public static readonly BindableProperty GranularityProperty =
			BindableProperty.Create(nameof(Granularity), typeof(int), typeof(Line), minValueGranularity,
				coerceValue: CoerceValue);

		public static readonly BindableProperty EnableSmoothedPathProperty =
			BindableProperty.Create(nameof(EnableSmoothedPath), typeof(bool), typeof(Line), true);

		public static readonly BindableProperty PointsProperty = BindableProperty.Create(
			nameof(Points), typeof(ObservableCollection<Point>), typeof(Line), new ObservableCollection<Point>(),
			BindingMode.TwoWay);
		
		public static readonly BindableProperty LineColorProperty =
			BindableProperty.Create(nameof(LineColor), typeof(Color), typeof(Line), Color.Black);

		public static readonly BindableProperty LineWidthProperty =
			BindableProperty.Create(nameof(LineWidth), typeof(float), typeof(Line), 5f);
		
		public Color LineColor
		{
			get => (Color) GetValue(LineColorProperty);
			set => SetValue(LineColorProperty, value);
		}

		public float LineWidth
		{
			get => (float) GetValue(LineWidthProperty);
			set => SetValue(LineWidthProperty, value);
		}

		public ObservableCollection<Point> Points
		{
			get => (ObservableCollection<Point>) GetValue(PointsProperty);
			set => SetValue(PointsProperty, value);
		}

		public int Granularity
		{
			get => (int) GetValue(GranularityProperty);
			set => SetValue(GranularityProperty, value);
		}

		public bool EnableSmoothedPath
		{
			get => (bool) GetValue(EnableSmoothedPathProperty);
			set => SetValue(EnableSmoothedPathProperty, value);
		}

		static object CoerceValue(BindableObject bindable, object value)
			=> ((Line) bindable).CoerceValue((int) value);

		int CoerceValue(int value) => value < minValueGranularity ? minValueGranularity : value;

		public Stream GetImageStream(double imageSizeWidth, double imageSizeHeight, Color backgroundColor) =>
			DrawingViewService.GetImageStream(Points.ToList(), new Size(imageSizeWidth, imageSizeHeight), LineWidth,
				LineColor,
				backgroundColor);

		public static Stream GetImageStream(IEnumerable<Point> points,
			Size imageSize,
			float lineWidth,
			Color strokeColor,
			Color backgroundColor) =>
			DrawingViewService.GetImageStream(points.ToList(), imageSize, lineWidth, strokeColor, backgroundColor);
	}
}