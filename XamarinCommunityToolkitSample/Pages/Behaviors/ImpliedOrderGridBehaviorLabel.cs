using System;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Behaviors
{
	public class ImpliedOrderGridBehaviorLabel : Label
	{
		static int count = 1;
		static readonly Random Rnd = new Random();

		public ImpliedOrderGridBehaviorLabel()
		{
			FontSize = 24;
			FontAttributes = FontAttributes.Bold;
			BackgroundColor = Color.FromRgb(Rnd.Next(256), Rnd.Next(256), Rnd.Next(256));
			SizeChanged += OnTestLabelSizeChanged;
		}

		private void OnTestLabelSizeChanged(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(Text))
				Text = $"L: {count}" +
					   $"\r\nr:{GetValue(Grid.RowProperty)} c:{GetValue(Grid.ColumnProperty)}" +
					   $"\r\nrs:{GetValue(Grid.RowSpanProperty)} cs:{GetValue(Grid.ColumnSpanProperty)}";
			count++;
		}

		
	}

}
