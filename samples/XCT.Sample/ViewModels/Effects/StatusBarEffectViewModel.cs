using System;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Effects
{
	public class StatusBarEffectViewModel : BaseViewModel
	{
		int redSliderValue;
		int greanSliderValue;
		int blueSliderValue;
		bool isDefaultChecked = true;
		bool isLightContentChecked;
		bool isDarkContentChecked;

		public Color StatusBarColor => Color.FromRgb(RedSliderValue, GreenSliderValue, BlueSliderValue);

		public StatusBarStyle StatusBarStyle
		{
			get
			{
				if (IsDefaultChecked)
				{
					return StatusBarStyle.Default;
				}
				if (IsLightContentChecked)
				{
					return StatusBarStyle.LightContent;
				}
				if (IsDarkContentChecked)
				{
					return StatusBarStyle.DarkContent;
				}
				throw new InvalidOperationException("Style is not selected");
			}
		}

		public NavigationBarStyle NavigationBarStyle => (NavigationBarStyle)(int)StatusBarStyle;

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

		public bool IsDefaultChecked
		{
			get => isDefaultChecked;
			set => SetProperty(ref isDefaultChecked, value, onChanged: NotifyStyleChanged);
		}

		public bool IsLightContentChecked
		{
			get => isLightContentChecked;
			set => SetProperty(ref isLightContentChecked, value, onChanged: NotifyStyleChanged);
		}

		public bool IsDarkContentChecked
		{
			get => isDarkContentChecked;
			set => SetProperty(ref isDarkContentChecked, value, onChanged: NotifyStyleChanged);
		}

		void NotifyStyleChanged()
		{
			OnPropertyChanged(nameof(StatusBarStyle));
			OnPropertyChanged(nameof(NavigationBarStyle));
		}
	}
}