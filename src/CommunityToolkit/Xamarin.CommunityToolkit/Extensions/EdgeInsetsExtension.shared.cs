using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Extensions
{
	/// <summary>
	/// An extension to declare padding or marging more easily, more conveniently
	/// </summary>
	[ContentProperty(nameof(All))]
	public class EdgeInsetsExtension : IMarkupExtension
	{
		public double Top { get; set; } = double.MinValue;
		public double Left { get; set; } = double.MinValue;
		public double Right { get; set; } = double.MinValue;
		public double Bottom { get; set; } = double.MinValue;
		public double All { get; set; } = 0d;
		public double Vertical { get; set; } = double.MinValue;
		public double Horizontal { get; set; } = double.MinValue;

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			return new Thickness(
				GetValue(Left, Horizontal, All),
				GetValue(Top, Vertical, All),
				GetValue(Right, Horizontal, All),
				GetValue(Bottom, Vertical, All)
			);
		}

		private double GetValue(double value, double alternative1, double alternative2)
		{
			var valueSpecified = Math.Abs(value - double.MinValue) > double.Epsilon;
			if (valueSpecified)
			{
				return value;
			}

			var alternative1Specified = Math.Abs(alternative1 - double.MinValue) > double.Epsilon;
			return alternative1Specified ? alternative1 : alternative2;
		}
	}
}