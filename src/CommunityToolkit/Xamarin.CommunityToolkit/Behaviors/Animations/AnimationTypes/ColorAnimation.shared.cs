﻿using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class ColorAnimation : AnimationBase
	{
		public static readonly BindableProperty ToColorProperty =
			BindableProperty.Create(nameof(ToColor), typeof(Color), typeof(ColorAnimation), Color.Default,
				BindingMode.TwoWay, null);

		public Color ToColor
		{
			get => (Color)GetValue(ToColorProperty);
			set => SetValue(ToColorProperty, value);
		}

		protected override uint DefaultDuration { get; set; } = 1000;

		public override async Task Animate(View view)
		{
			var fromColor = view.BackgroundColor;

			await Task.Run(() =>
			{
				Device.BeginInvokeOnMainThread(async () =>
				{
					await view.ColorTo(fromColor, ToColor, c => view.BackgroundColor = c, Duration);
				});
			});
		}
	}
}
