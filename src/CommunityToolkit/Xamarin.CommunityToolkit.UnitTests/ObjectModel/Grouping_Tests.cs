using System.Linq;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel
{
	public sealed class Grouping_Tests
	{
		readonly Person[] people;

		public Grouping_Tests()
		{
			people = new[]
			{
				new Person { FirstName = "Joseph", LastName = "Hill" },
				new Person { FirstName = "James", LastName = "Montemagno" },
				new Person { FirstName = "Pierce", LastName = "Boggan" },
			};
		}

		[Fact]
		public void Grouping()
		{
			var sorted = from person in people
				orderby person.FirstName
				group person by person.Group
				into personGroup
				select new Grouping<string, Person>(personGroup.Key, personGroup);
			var grouped = new ObservableRangeCollection<Grouping<string, Person>>();
			grouped.AddRange(sorted);

			Assert.Equal(2, grouped.Count);
			Assert.Equal("J", grouped[0].Key);
			Assert.Equal(2, grouped[0].Count);
			Assert.Single(grouped[1]);
			Assert.Equal(2, grouped[0].Items.Count);
			Assert.Single(grouped[1].Items);
		}

		[Fact]
		public void GroupingSubKey()
		{
			var sorted = from person in people
				orderby person.FirstName
				group person by person.Group
				into personGroup
				select new Grouping<string, string, Person>(personGroup.Key, personGroup.Key, personGroup);
			var grouped = new ObservableRangeCollection<Grouping<string, string, Person>>();
			grouped.AddRange(sorted);

			Assert.Equal(2, grouped.Count);
			Assert.Equal("J", grouped[0].SubKey);
			Assert.Equal("J", grouped[0].Key);
			Assert.Equal(2, grouped[0].Count);
			Assert.Single(grouped[1]);
			Assert.Equal(2, grouped[0].Items.Count);
			Assert.Single(grouped[1].Items);
		}
	}
}