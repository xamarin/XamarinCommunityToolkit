using System;
using Xamarin.Forms;

namespace FormsCommunityToolkit.Controls
{
	public class CheckBox : View
	{
		public static readonly BindableProperty IsCheckedProperty =
			BindableProperty.Create(
				nameof(IsChecked), typeof(bool), typeof(bool), false,
				BindingMode.TwoWay);

		public EventHandler<bool> CheckedChanged;


		public bool IsChecked
		{
			get => (bool)GetValue(IsCheckedProperty);
			set
			{
				SetValue(IsCheckedProperty, value);
				CheckedChanged?.Invoke(this, value);
			}
		}
	}
}
