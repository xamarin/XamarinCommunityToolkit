using System;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Effects
{
	public class StatusBarEffectViewModel : BaseViewModel
	{
		int redSliderValue;
		int greanSliderValue;
		int blueSliderValue;
		bool isAutoChecked = true;
		bool isLightContentChecked;
		bool isDarkContentChecked;

		public Color StatusBarColor => Color.FromRgb(RedSliderValue, GreenSliderValue, BlueSliderValue);

		public StatusBarStyle StatusBarStyle
		{
			get
			{
				if (isAutoChecked)
				{
					return StatusBarStyle.Default;
				}
				if (IsLightContentChecked)
				{
					return StatusBarStyle.LightContent;
				}
				if (isDarkContentChecked)
				{
					return StatusBarStyle.DarkContent;
				}
				throw new InvalidOperationException("Style is not selected");
			}
		}

		public int RedSliderValue
		{
			get => redSliderValue;
			set => SetProperty(ref redSliderValue, value, onChanged: () => OnPropertyChanged(nameof(StatusBarColor)));
		}

		public int GreenSliderValue
		{
			get => greanSliderValue;
			set => SetProperty(ref greanSliderValue, value, onChanged: () => OnPropertyChanged(nameof(StatusBarColor)));
		}

		public int BlueSliderValue
		{
			get => blueSliderValue;
			set => SetProperty(ref blueSliderValue, value, onChanged: () => OnPropertyChanged(nameof(StatusBarColor)));
		}

		public bool IsAutoChecked
		{
			get => isAutoChecked;
			set => SetProperty(ref isAutoChecked, value, onChanged: () => OnPropertyChanged(nameof(StatusBarStyle)));
		}

		public bool IsLightContentChecked
		{
			get => isLightContentChecked;
			set => SetProperty(ref isLightContentChecked, value, onChanged: () => OnPropertyChanged(nameof(StatusBarStyle)));
		}

		public bool IsDarkContentChecked
		{
			get => isDarkContentChecked;
			set => SetProperty(ref isDarkContentChecked, value, onChanged: () => OnPropertyChanged(nameof(StatusBarStyle)));
		}
	}
}
