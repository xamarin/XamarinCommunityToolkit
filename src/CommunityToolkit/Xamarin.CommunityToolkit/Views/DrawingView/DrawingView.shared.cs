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
		public static readonly BindableProperty ClearOnFinishProperty =
			BindableProperty.Create(nameof(ClearOnFinish), typeof(bool), typeof(DrawingView), default(bool));

		public static readonly BindableProperty MultiLineModeProperty =
			BindableProperty.Create(nameof(MultiLineMode), typeof(bool), typeof(DrawingView), default(bool));

		public static readonly BindableProperty LinesProperty = BindableProperty.Create(
			nameof(Lines), typeof(ObservableCollection<Line>), typeof(DrawingView), new ObservableCollection<Line>(),
			BindingMode.TwoWay);
		
		public static readonly BindableProperty DrawingLineCompletedCommandProperty = BindableProperty.Create(nameof(DrawingLineCompletedCommand), typeof(ICommand), typeof(DrawingView));
		
		public static readonly BindableProperty DefaultLineColorProperty =
			BindableProperty.Create(nameof(DefaultLineColor), typeof(Color), typeof(DrawingView), Color.Black);

		public static readonly BindableProperty DefaultLineWidthProperty =
			BindableProperty.Create(nameof(DefaultLineWidth), typeof(float), typeof(DrawingView), 5f);
		
		public Color DefaultLineColor
		{
			get => (Color) GetValue(DefaultLineColorProperty);
			set => SetValue(DefaultLineColorProperty, value);
		}

		public float DefaultLineWidth
		{
			get => (float) GetValue(DefaultLineWidthProperty);
			set => SetValue(DefaultLineWidthProperty, value);
		}
		
		public ICommand? DrawingLineCompletedCommand
		{
			get => (ICommand) GetValue(DrawingLineCompletedCommandProperty);
			set => SetValue(DrawingLineCompletedCommandProperty, value);
		}
		
		public ObservableCollection<Line> Lines
		{
			get => (ObservableCollection<Line>) GetValue(LinesProperty);
			set => SetValue(LinesProperty, value);
		}

		public bool MultiLineMode
		{
			get => (bool) GetValue(MultiLineModeProperty);
			set => SetValue(MultiLineModeProperty, value);
		}

		public bool ClearOnFinish
		{
			get => (bool) GetValue(ClearOnFinishProperty);
			set => SetValue(ClearOnFinishProperty, value);
		}

		// public Stream GetImageStream(double imageSizeWidth, double imageSizeHeight) =>
		// 	DrawingViewService.GetImageStream(Points.ToList(), new Size(imageSizeWidth, imageSizeHeight), LineWidth,
		// 		LineColor,
		// 		BackgroundColor);

		public static Stream GetImageStream(IEnumerable<Point> points,
			Size imageSize,
			float lineWidth,
			Color strokeColor,
			Color backgroundColor) =>
			DrawingViewService.GetImageStream(points.ToList(), imageSize, lineWidth, strokeColor, backgroundColor);
	}
}