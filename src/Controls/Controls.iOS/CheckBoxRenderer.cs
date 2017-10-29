using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using FormsCommunityToolkit.Controls;
using FormsCommunityToolkit.Controls.iOS;
using FormsCommunityToolkit.Controls.iOS.Views;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]

namespace FormsCommunityToolkit.Controls.iOS
{
	public class CheckBoxRenderer : ViewRenderer<CheckBox, CheckBoxView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
		{
			base.OnElementChanged(e);

			try
			{
				BackgroundColor = Element.BackgroundColor.ToUIColor();

				if (Control == null)
				{
					var checkBox = new CheckBoxView(Bounds);
					checkBox.TouchUpInside += (s, args) => Element.IsChecked = Control.IsChecked;

					SetNativeControl(checkBox);
				}

				Control.IsChecked = e.NewElement.IsChecked;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception);
				throw;
			}
		}

		
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			switch (e.PropertyName)
			{
				case "IsChecked":
					Control.IsChecked = Element.IsChecked;
					break;
				case "Element":
					break;
				default:
					System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", e.PropertyName);
					return;
			}
		}
	}
}
