using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using Xamarin.CommunityToolkit.Views;

#if __MOBILE__
using UIKit;
#endif

[assembly: ExportRenderer(typeof(AutoFitLabel), typeof(AutoFitLabelRenderer))]

namespace Xamarin.CommunityToolkit.Views
{
	public class AutoFitLabelRenderer : LabelRenderer
	{
#if __MOBILE__
		public override void LayoutSubviews()
		{
			if (Control is null)
				return;

			switch (Element.VerticalTextAlignment)
			{
				case TextAlignment.Start:
					Control.BaselineAdjustment = UIBaselineAdjustment.None;
					break;
				case TextAlignment.Center:
					Control.BaselineAdjustment = UIBaselineAdjustment.AlignCenters;
					break;
				case TextAlignment.End:
					Control.BaselineAdjustment = UIBaselineAdjustment.AlignBaselines;
					break;
			}

			base.LayoutSubviews();
		}
#endif

		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement != null)
				UpdateAutoFitMode();
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == AutoFitLabel.AutoFitModeProperty.PropertyName)
				UpdateAutoFitMode();
		}

		void UpdateAutoFitMode()
		{
#if __MOBILE__
			if (Element is AutoFitLabel autoFitLabel)
			{
				switch (autoFitLabel.AutoFitMode)
				{
					case AutoFitTextMode.FitToWidth:
						Control.AdjustsFontSizeToFitWidth = true;

						var uiFont = Control.Font;
						var minScaleFactor = (float)autoFitLabel.MinAutoFitFontSize / autoFitLabel.MaxAutoFitFontSize;

						Control.Font = uiFont.WithSize((float)autoFitLabel.MaxAutoFitFontSize);
						Control.MinimumScaleFactor = minScaleFactor;
						Control.AdjustsFontSizeToFitWidth = true;
						Control.Lines = 1;
						Control.LineBreakMode = UILineBreakMode.Clip;
						break;
					case AutoFitTextMode.None:
					default:
						Control.AdjustsFontSizeToFitWidth = false;
						OnElementPropertyChanged(this, new PropertyChangedEventArgs(Label.LineBreakModeProperty.PropertyName));
						OnElementPropertyChanged(this, new PropertyChangedEventArgs(Label.MaxLinesProperty.PropertyName));
						OnElementPropertyChanged(this, new PropertyChangedEventArgs(Label.FontProperty.PropertyName));
						break;
				}

				LayoutSubviews();
			}
#endif
		}
	}
}
