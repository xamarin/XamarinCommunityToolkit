using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Behaviors
{
	public class AutoGridLayoutBehaviorTestLabel : Label
	{
		public AutoGridLayoutBehaviorTestLabel()
		{
			FontSize = 24;
			FontAttributes = FontAttributes.Bold;
			BackgroundColor = Color.FromRgb(Rnd.Next(256), Rnd.Next(256), Rnd.Next(256));
			SizeChanged += TestLabel_SizeChanged;
		}

		private void TestLabel_SizeChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(Text))
				Text = $"L: {_count}" +
					   $"\r\nr:{GetValue(Grid.RowProperty)} c:{GetValue(Grid.ColumnProperty)}" +
					   $"\r\nrs:{GetValue(Grid.RowSpanProperty)} cs:{GetValue(Grid.ColumnSpanProperty)}";
			_count++;
		}

		private static int _count = 1;
		private static readonly Random Rnd = new Random();
	}

}
