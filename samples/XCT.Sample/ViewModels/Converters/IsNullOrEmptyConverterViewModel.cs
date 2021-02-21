using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class IsNullOrEmptyConverterViewModel : BaseViewModel
	{
		public ObservableCollection<string> DummyItemSource { get; set; } = new ObservableCollection<string>
		{
			"Dummy Item 0",
			"Dummy Item 1",
			"Dummy Item 2",
			"Dummy Item 3",
			"Dummy Item 4",
			"Dummy Item 5",
		};

		string selectedItem;
		public string SelectedItem
		{
			get => selectedItem;
			set
			{
				selectedItem = value;
				OnPropertyChanged(nameof(SelectedItem));
			}
		}

		public ICommand ClearSelectionCommand => new Command(() =>
		{
			SelectedItem = null;
		});
	}
}
