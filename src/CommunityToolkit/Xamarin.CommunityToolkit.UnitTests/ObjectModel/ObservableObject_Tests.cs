using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel
{
	public sealed class ObservableObject_Tests
	{
		Person person;

		public ObservableObject_Tests()
		{
			person = new Person
			{
				FirstName = "James",
				LastName = "Montemagno"
			};
		}

		[Fact]
		public void OnPropertyChanged()
		{
			PropertyChangedEventArgs updated = null;
			person.PropertyChanged += (sender, args) =>
			{
				updated = args;
			};

			person.FirstName = "Motz";

			Assert.NotNull(updated);
			Assert.Equal(nameof(person.FirstName), updated.PropertyName);
		}

		[Fact]
		public void OnDidntChange()
		{
			PropertyChangedEventArgs updated = null;
			person.PropertyChanged += (sender, args) =>
			{
				updated = args;
			};

			person.FirstName = "James";

			Assert.Null(updated);
		}

		[Fact]
		public void OnChangedEvent()
		{
			var triggered = false;
			person.Changed = () =>
			{
				triggered = true;
			};

			person.FirstName = "Motz";

			Assert.True(triggered, "OnChanged didn't raise");
		}

		[Fact]
		public void OnChangingEvent()
		{
			var triggered = false;
			person.Changing = () =>
			{
				triggered = true;
			};

			person.FirstName = "Motz";

			Assert.True(triggered, "OnChanging didn't raise");
		}

		[Fact]
		public void ValidateEvent()
		{
			var contol = "Motz";
			var triggered = false;
			person.Validate = (oldValue, newValue) =>
			{
				triggered = true;
				return oldValue != newValue;
			};

			person.FirstName = contol;

			Assert.True(triggered, "ValidateValue didn't raise");
			Assert.Equal(person.FirstName, contol);
		}

		[Fact]
		public void NotValidateEvent()
		{
			var contol = person.FirstName;
			var triggered = false;
			person.Validate = (oldValue, newValue) =>
			{
				triggered = true;
				return false;
			};

			person.FirstName = "Motz";

			Assert.True(triggered, "ValidateValue didn't raise");
			Assert.Equal(person.FirstName, contol);
		}

		[Fact]
		public async Task ValidateEventException()
		{
			person.Validate = (oldValue, newValue) =>
			{
				throw new ArgumentOutOfRangeException();
			};

			var result = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
			{
				person.FirstName = "Motz";
				return Task.CompletedTask;
			});

			Assert.NotNull(result);
		}

		public class Person : ObservableObject
		{
			string firstName;
			string lastName;

			public Action Changed { get; set; }

			public Action Changing { get; set; }

			public Func<string, string, bool> Validate { get; set; }

			public string FirstName
			{
				get => firstName;
				set => SetProperty(ref firstName, value, onChanged: Changed, onChanging: Changing, validateValue: Validate);
			}

			public string LastName
			{
				get => lastName;
				set => SetProperty(ref lastName, value, onChanged: Changed, onChanging: Changing, validateValue: Validate);
			}
		}
	}
}