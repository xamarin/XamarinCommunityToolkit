﻿using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Views.Popups
{
	public partial class PopupAnchorPage
	{
		public PopupAnchorPage() => InitializeComponent();

		void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
		{
			if (sender is Label label)
			{
				if (Device.RuntimePlatform == Device.Android)
				{
					label.TranslationX += e.TotalX;
					label.TranslationY += e.TotalY;
				}
				else
				{
					switch (e.StatusType)
					{
						case GestureStatus.Running:
							label.TranslationX = e.TotalX;
							label.TranslationY = e.TotalY;
							break;
						case GestureStatus.Completed:
							label.TranslationX += e.TotalX;
							label.TranslationY += e.TotalY;
							break;
					}
				}
			}
		}
	}
}