using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class Monkey
	{
		public string Index { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public string Details { get; set; }
		public string Image { get; set; }
		public Color Color { get; set; }
	}

	public class TabItemsSourceViewModel : BaseViewModel
	{
		public TabItemsSourceViewModel()
		{
			LoadMonkeys();
		}

		public ObservableCollection<Monkey> Monkeys { get; set; }

		public ICommand ClearDataCommand => new Command(ClearData);

		public ICommand UpdateDataCommand => new Command(UpdateData);

		void LoadMonkeys()
		{
			Monkeys = new ObservableCollection<Monkey>
			{
				new Monkey
				{
					Index = "0",
					Name = "Baboon",
					Location = "Africa & Asia",
					Details = "Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.",
					Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Papio_anubis_%28Serengeti%2C_2009%29.jpg/200px-Papio_anubis_%28Serengeti%2C_2009%29.jpg",
					Color = Color.LightSalmon
				},

				new Monkey
				{
					Index = "1",
					Name = "Capuchin Monkey",
					Location = "Central & South America",
					Details = "The capuchin monkeys are New World monkeys of the subfamily Cebinae. Prior to 2011, the subfamily contained only a single genus, Cebus.",
					Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Capuchin_Costa_Rica.jpg/200px-Capuchin_Costa_Rica.jpg",
					Color = Color.LightBlue
				},

				new Monkey
				{
					Index = "2",
					Name = "Blue Monkey",
					Location = "Central and East Africa",
					Details = "The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia",
					Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/8/83/BlueMonkey.jpg/220px-BlueMonkey.jpg",
					Color = Color.LightSlateGray
				},

				new Monkey
				{
					Index = "3",
					Name = "Squirrel Monkey",
					Location = "Central & South America",
					Details = "The squirrel monkeys are the New World monkeys of the genus Saimiri. They are the only genus in the subfamily Saimirinae. The name of the genus Saimiri is of Tupi origin, and was also used as an English name by early researchers.",
					Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/2/20/Saimiri_sciureus-1_Luc_Viatour.jpg/220px-Saimiri_sciureus-1_Luc_Viatour.jpg",
					Color = Color.Chocolate
				},

				new Monkey
				{
					Index = "4",
					Name = "Golden Lion Tamarin",
					Location = "Brazil",
					Details = "The golden lion tamarin also known as the golden marmoset, is a small New World monkey of the family Callitrichidae.",
					Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/8/87/Golden_lion_tamarin_portrait3.jpg/220px-Golden_lion_tamarin_portrait3.jpg",
					Color = Color.Violet
				},

				new Monkey
				{
					Index = "5",
					Name = "Howler Monkey",
					Location = "South America",
					Details = "Howler monkeys are among the largest of the New World monkeys. Fifteen species are currently recognised. Previously classified in the family Cebidae, they are now placed in the family Atelidae.",
					Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/0/0d/Alouatta_guariba.jpg/200px-Alouatta_guariba.jpg",
					Color = Color.Aqua
				},

				new Monkey
				{
					Index = "6",
					Name = "Japanese Macaque",
					Location = "Japan",
					Details = "The Japanese macaque, is a terrestrial Old World monkey species native to Japan. They are also sometimes known as the snow monkey because they live in areas where snow covers the ground for months each",
					Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/c/c1/Macaca_fuscata_fuscata1.jpg/220px-Macaca_fuscata_fuscata1.jpg",
					Color = Color.OrangeRed
				},

				new Monkey
				{
					Index = "7",
					Name = "Mandrill",
					Location = "Southern Cameroon, Gabon, Equatorial Guinea, and Congo",
					Details = "The mandrill is a primate of the Old World monkey family, closely related to the baboons and even more closely to the drill. It is found in southern Cameroon, Gabon, Equatorial Guinea, and Congo.",
					Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/7/75/Mandrill_at_san_francisco_zoo.jpg/220px-Mandrill_at_san_francisco_zoo.jpg",
					Color = Color.MediumPurple
				},

				new Monkey
				{
					Index = "8",
					Name = "Proboscis Monkey",
					Location = "Borneo",
					Details = "The proboscis monkey or long-nosed monkey, known as the bekantan in Malay, is a reddish-brown arboreal Old World monkey that is endemic to the south-east Asian island of Borneo.",
					Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/e/e5/Proboscis_Monkey_in_Borneo.jpg/250px-Proboscis_Monkey_in_Borneo.jpg",
					Color = Color.Pink
				}
			};
		}

		void ClearData()
		{
			Monkeys.Clear();
		}

		void UpdateData()
		{
			Monkeys.Clear();

			Monkeys.Add(new Monkey
			{
				Index = "0",
				Name = "Howler Monkey",
				Location = "South America",
				Details = "Howler monkeys are among the largest of the New World monkeys. Fifteen species are currently recognised. Previously classified in the family Cebidae, they are now placed in the family Atelidae.",
				Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/0/0d/Alouatta_guariba.jpg/200px-Alouatta_guariba.jpg",
				Color = Color.Aqua
			});

			Monkeys.Add(new Monkey
			{
				Index = "1",
				Name = "Japanese Macaque",
				Location = "Japan",
				Details = "The Japanese macaque, is a terrestrial Old World monkey species native to Japan. They are also sometimes known as the snow monkey because they live in areas where snow covers the ground for months each",
				Image = "http://upload.wikimedia.org/wikipedia/commons/thumb/c/c1/Macaca_fuscata_fuscata1.jpg/220px-Macaca_fuscata_fuscata1.jpg",
				Color = Color.OrangeRed
			});
		}
	}
}