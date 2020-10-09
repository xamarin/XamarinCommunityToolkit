using System;
using System.Collections.Generic;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class SegmentedViewModel : BaseViewModel
	{
		string title;

		public string Title
		{
			get => title;
			set => SetProperty(ref title, value);
		}

		public IList<string> Options => new List<string>
        {
			"Option A",
			"Option B",
			"Option C",
        };

		object selectedItem;

		public object SelectedItem
		{
			get => selectedItem;
			set => SetProperty(ref selectedItem, value);
		}

		public SegmentedViewModel()
		{
			Title = "SegmentedView";
		}
	}
}
