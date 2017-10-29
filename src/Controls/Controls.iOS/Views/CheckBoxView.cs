using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace FormsCommunityToolkit.Controls.iOS.Views
{
	[Register("CheckBoxView")]
	public class CheckBoxView : UIButton
	{
		public CheckBoxView()
		{
			Initialize();
		}

		public CheckBoxView(CGRect bounds)
			: base(bounds)
		{
			Initialize();
		}

		public bool IsChecked
		{
			set => Selected = value;
			get => Selected;
		}

		void Initialize()
		{
			try
			{
				AdjustEdgeInsets();
				ApplyStyle();

				TouchUpInside += (sender, args) => Selected = !Selected;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

		void AdjustEdgeInsets()
		{
			const float inset = 8f;

			HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
			ImageEdgeInsets = new UIEdgeInsets(0f, inset, 0f, 0f);
			TitleEdgeInsets = new UIEdgeInsets(0f, inset * 2, 0f, 0f);
		}

		void ApplyStyle()
		{
			var a = UIImage.FromBundle("CheckedCheckbox");
			var b = UIImage.FromBundle("UncheckedCheckbox");


			SetImage(a, UIControlState.Selected);
			SetImage(b, UIControlState.Normal);
		}
	}
}
