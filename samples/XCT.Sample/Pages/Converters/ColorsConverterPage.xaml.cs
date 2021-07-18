using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Converters
{
	public partial class ColorsConverterPage
	{
		readonly Dictionary<string, Color> colors = typeof(Color)
			.GetFields(BindingFlags.Static | BindingFlags.Public)
			.ToDictionary(c => c.Name, c => (Color)c.GetValue(null));

		public ColorsConverterPage() => InitializeComponent();

		protected override void OnAppearing()
		{
			var keys = colors.Keys.ToList();
			picker.ItemsSource = keys;
		}

		void Picker_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (colors.TryGetValue((string)picker.SelectedItem, out var color))
				boxView.BackgroundColor = color;
		}
	}
}