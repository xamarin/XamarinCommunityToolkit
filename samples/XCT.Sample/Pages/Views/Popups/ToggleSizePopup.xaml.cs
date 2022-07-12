﻿using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.Popups
{
	public partial class ToggleSizePopup
	{
		Size originalSize;

		public ToggleSizePopup()
		{
			InitializeComponent();
			originalSize = Size;
		}

		void Button_Clicked(object? sender, System.EventArgs e)
		{
			if (originalSize == Size)
			{
				Size = new Size(originalSize.Width * 1.25, originalSize.Height * 1.25);
			}
			else
			{
				Size = originalSize;
			}
		}
	}
}