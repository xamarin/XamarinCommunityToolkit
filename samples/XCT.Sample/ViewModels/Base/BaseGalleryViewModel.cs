using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Sample.Models;

namespace Xamarin.CommunityToolkit.Sample.ViewModels
{
	public abstract class BaseGalleryViewModel : BaseViewModel
	{
		public BaseGalleryViewModel()
		{
			Filter();
			FilterCommand = CommandFactory.Create(Filter);
		}

		public ICommand FilterCommand => filterCommand ??= new Command(Filter);

		public IReadOnlyList<SectionModel> Items { get; }

		public ICommand FilterCommand { get; }

		public string FilterValue { private get; set; }

		protected abstract IEnumerable<SectionModel> CreateItems();

		void Filter()
		{
			FilterValue ??= string.Empty;
			FilteredItems = Items.Where(item => item.Title.IndexOf(FilterValue, StringComparison.InvariantCultureIgnoreCase) >= 0);
			OnPropertyChanged(nameof(FilteredItems));
		}
	}
}