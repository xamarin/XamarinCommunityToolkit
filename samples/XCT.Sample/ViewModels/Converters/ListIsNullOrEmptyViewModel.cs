using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class ListIsNullOrEmptyViewModel : BaseViewModel
	{
		public ObservableRangeCollection<Person> Items { get; } = new ObservableRangeCollection<Person>();

		public ListIsNullOrEmptyViewModel()
		{
			AddItemCommand = CommandFactory.Create(() =>
			{
				Items.Add(new Person
				{
					Id = Items.Count,
					Name = $"Person {Items.Count}"
				});
			});
			RemoveItemCommand = CommandFactory.Create(() => Items.RemoveAt(0));

			// ListIsNullOrEmptyConvertor needs to know that Items are updated
			Items.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(Items));
		}

		public Command AddItemCommand { get; }

		public Command RemoveItemCommand { get; }
	}
}