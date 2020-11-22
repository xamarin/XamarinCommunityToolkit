using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class DrawingView : View
	{
		const int minValueGranularity = 5;

		public static readonly BindableProperty GranularityProperty =
			BindableProperty.Create(nameof(Granularity), typeof(int), typeof(DrawingView), minValueGranularity, coerceValue: CoerceValue);

		public static readonly BindableProperty EnableSmoothedPathProperty =
			BindableProperty.Create(nameof(EnableSmoothedPath), typeof(bool), typeof(DrawingView), default(bool));

		public static readonly BindableProperty LineColorProperty =
			BindableProperty.Create(nameof(LineColor), typeof(Color), typeof(DrawingView), Color.Default);

		public static readonly BindableProperty LineWidthProperty =
			BindableProperty.Create(nameof(LineWidth), typeof(float), typeof(DrawingView), 5f);

		public static readonly BindableProperty ClearOnFinishProperty =
			BindableProperty.Create(nameof(ClearOnFinish), typeof(bool), typeof(DrawingView), default(bool));

		public static readonly BindableProperty PointsProperty = BindableProperty.Create(
			nameof(Points), typeof(ObservableCollection<Point>), typeof(DrawingView), new ObservableCollection<Point>(),
			BindingMode.TwoWay);

		public static readonly BindableProperty DrawingCompletedCommandProperty = BindableProperty.Create(
			nameof(DrawingCompletedCommand), typeof(ICommand), typeof(DrawingView), default(ICommand));

		public ObservableCollection<Point> Points
		{
			get => (ObservableCollection<Point>)GetValue(PointsProperty);
			set => SetValue(PointsProperty, value);
		}

		public ICommand DrawingCompletedCommand
		{
			get => (ICommand)GetValue(DrawingCompletedCommandProperty);
			set => SetValue(DrawingCompletedCommandProperty, value);
		}

		public int Granularity
		{
			get => (int)GetValue(GranularityProperty);
			set => SetValue(GranularityProperty, value);
		}

		public bool EnableSmoothedPath
		{
			get => (bool)GetValue(EnableSmoothedPathProperty);
			set => SetValue(EnableSmoothedPathProperty, value);
		}

		public Color LineColor
		{
			get => (Color)GetValue(LineColorProperty);
			set => SetValue(LineColorProperty, value);
		}

		public float LineWidth
		{
			get => (float)GetValue(LineWidthProperty);
			set => SetValue(LineWidthProperty, value);
		}

		public bool ClearOnFinish
		{
			get => (bool)GetValue(ClearOnFinishProperty);
			set => SetValue(ClearOnFinishProperty, value);
		}

		static object CoerceValue(BindableObject bindable, object value)
			=> ((DrawingView)bindable).CoerceValue((int)value);

		int CoerceValue(int value) => value < minValueGranularity ? minValueGranularity : value;

		public Stream GetImageStream(double imageSizeWidth, double imageSizeHeight) =>
			DrawingViewService.GetImageStream(Points.ToList(), new Size(imageSizeWidth, imageSizeHeight), LineWidth, LineColor,
				BackgroundColor);

		public static Stream GetImageStream(IEnumerable<Point> points,
			Size imageSize,
			float lineWidth,
			Color strokeColor,
			Color backgroundColor) =>
			DrawingViewService.GetImageStream(points.ToList(), imageSize, lineWidth, strokeColor, backgroundColor);
	}
}