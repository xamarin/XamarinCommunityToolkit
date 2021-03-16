using System;
using System.ComponentModel;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel
{
	public sealed class ObservableObject_Tests
	{
		readonly Person person = new Person
		{
			FirstName = "James",
			LastName = "Montemagno"
		};

		[Test]
		public void OnPropertyChanged()
		{
			PropertyChangedEventArgs? updated = null;
			person.PropertyChanged += (sender, args) =>
			{
				updated = args;
			};

			person.FirstName = "Motz";

			Assert.NotNull(updated);
			Assert.AreEqual(nameof(person.FirstName), updated?.PropertyName);
		}

		[Test]
		public void OnDidntChange()
		{
			PropertyChangedEventArgs? updated = null;
			person.PropertyChanged += (sender, args) =>
			{
				updated = args;
			};

			person.FirstName = "James";

			Assert.Null(updated);
		}

		[Test]
		public void OnChangedEvent()
		{
			var triggered = false;
			person.Changed = () =>
			{
				triggered = true;
			};

			person.FirstName = "Motz";

			Assert.IsTrue(triggered, "OnChanged didn't raise");
		}

		[Test]
		public void OnChangingEvent()
		{
			var triggered = false;
			person.Changing = () =>
			{
				triggered = true;
			};

			person.FirstName = "Motz";

			Assert.IsTrue(triggered, "OnChanging didn't raise");
		}

		[Test]
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

			Assert.IsTrue(triggered, "ValidateValue didn't raise");
			Assert.AreEqual(person.FirstName, contol);
		}

		[Test]
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

			Assert.IsTrue(triggered, "ValidateValue didn't raise");
			Assert.AreEqual(person.FirstName, contol);
		}

		[Test]
		public void ValidateEventException()
		{
			person.Validate = (oldValue, newValue) =>
			{
				throw new ArgumentOutOfRangeException();
			};

			var result = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
			{
				person.FirstName = "Motz";
				return Task.CompletedTask;
			});

			Assert.NotNull(result);
		}
	}
}