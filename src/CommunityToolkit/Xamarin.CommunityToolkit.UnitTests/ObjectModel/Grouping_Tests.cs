using System.Linq;
using NUnit.Framework;
using Xamarin.CommunityToolkit.ObjectModel;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel
{
	public sealed class Grouping_Tests
	{
		readonly Person[] people = new[]
		{
			new Person { FirstName = "Joseph", LastName = "Hill" },
			new Person { FirstName = "James", LastName = "Montemagno" },
			new Person { FirstName = "Pierce", LastName = "Boggan" },
		};

		[Test]
		public void Grouping()
		{
			var sorted = from person in people
						 orderby person.FirstName
						 group person by person.Group
				into personGroup
						 select new Grouping<string, Person>(personGroup.Key, personGroup);

			var grouped = new ObservableRangeCollection<Grouping<string, Person>>();
			grouped.AddRange(sorted);

			Assert.AreEqual(2, grouped.Count);
			Assert.AreEqual("J", grouped[0].Key);
			Assert.AreEqual(2, grouped[0].Count);
			Assert.AreEqual(1, grouped[1].Count);
			Assert.AreEqual(2, grouped[0].Items.Count);
			Assert.AreEqual(1, grouped[1].Items.Count);
		}

		[Test]
		public void GroupingSubKey()
		{
			var sorted = from person in people
						 orderby person.FirstName
						 group person by person.Group
				into personGroup
						 select new Grouping<string, string, Person>(personGroup.Key, personGroup.Key, personGroup);

			var grouped = new ObservableRangeCollection<Grouping<string, string, Person>>();
			grouped.AddRange(sorted);

			Assert.AreEqual(2, grouped.Count);
			Assert.AreEqual("J", grouped[0].SubKey);
			Assert.AreEqual("J", grouped[0].Key);
			Assert.AreEqual(2, grouped[0].Count);
			Assert.AreEqual(1, grouped[1].Count);
			Assert.AreEqual(2, grouped[0].Items.Count);
			Assert.AreEqual(1, grouped[1].Items.Count);
		}
	}
}