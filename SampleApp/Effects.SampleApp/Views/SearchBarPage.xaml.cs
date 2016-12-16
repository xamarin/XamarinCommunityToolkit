using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace FormsCommunityToolkit.Effects.SampleApp.Views
{
	public partial class SearchBarPage : ContentPage
	{
		public SearchBarPage ()
		{
			InitializeComponent ();
			var searchableValues = new ObservableCollection<string>();
			for (int i = 1; i <= 50; i++)
			{
				searchableValues.Add("item " + i);
			}

			SearchBarSuggestionEffect.SetTextChangedAction(searchBar, new Action(() =>
			{
				if (searchBar.Text.Length == 0)
				{
					SearchBarSuggestionEffect.GetSuggestions(searchBar).Clear();
				}
				else
				{
					var filtered = searchableValues.Where(i => i.Contains(searchBar.Text.ToLower()));
					SearchBarSuggestionEffect.GetSuggestions(searchBar).Clear();
					foreach (string i in filtered)
					{
						SearchBarSuggestionEffect.GetSuggestions(searchBar).Add(i);
					}
				}
			}));
		}
	}
}
