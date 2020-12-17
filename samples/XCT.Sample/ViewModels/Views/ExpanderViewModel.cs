using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public partial class ExpanderViewModel : BaseViewModel
	{
		ICommand command;

		public ICommand Command => command ??= new Command(p =>
		{
			var sender = (Item)p;
			if (!sender.IsExpanded)
				return;

			foreach (var item in Items)
				item.IsExpanded = sender == item;
		});

		public Item[] Items { get; } = new Item[]
		{
			new Item
			{
				Name = "=> #1 <=",
			},
			new Item
			{
				Name = "=> #2 <=",
				IsExpanded = true
			},
			new Item
			{
				Name = "=> #3 <=",
			},
			new Item
			{
				Name = "=> #4 <=",
			},
			new Item
			{
				Name = "=> #5 <="
			},
		};

		public sealed class Item : BaseViewModel
		{
			string name;
			bool isExpanded;
			bool isEnabled = true;

			public string Name
			{
				get => name;
				set => SetProperty(ref name, value);
			}

			public bool IsExpanded
			{
				get => isExpanded;
				set => SetProperty(ref isExpanded, value);
			}

			public bool IsEnabled
			{
				get => isEnabled;
				set => SetProperty(ref isEnabled, value);
			}
		}
	}
}