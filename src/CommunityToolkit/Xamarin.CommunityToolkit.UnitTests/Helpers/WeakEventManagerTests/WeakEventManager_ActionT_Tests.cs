using System;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.Helpers;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Helpers.WeakEventManagerTests
{
	public class WeakEventManager_ActionT_Tests : BaseWeakEventManagerTests
	{
		readonly WeakEventManager<string> actionEventManager = new WeakEventManager<string>();

		public event Action<string> ActionEvent
		{
			add => actionEventManager.AddEventHandler(value);
			remove => actionEventManager.RemoveEventHandler(value);
		}

		[Fact]
		public void WeakEventManagerActionT_HandleEvent_ValidImplementation()
		{
			// Arrange
			ActionEvent += HandleDelegateTest;
			var didEventFire = false;

			void HandleDelegateTest(string message)
			{
				Assert.NotNull(message);
				Assert.NotEmpty(message);

				didEventFire = true;
				ActionEvent -= HandleDelegateTest;
			}

			// Act
			actionEventManager.RaiseEvent("Test", nameof(ActionEvent));

			// Assert
			Assert.True(didEventFire);
		}

		[Fact]
		public void WeakEventManagerActionT_HandleEvent_InvalidHandleEventEventName()
		{
			// Arrange
			ActionEvent += HandleDelegateTest;
			var didEventFire = false;

			void HandleDelegateTest(string message)
			{
				Assert.NotNull(message);
				Assert.NotEmpty(message);

				didEventFire = true;
			}

			// Act
			actionEventManager.RaiseEvent("Test", nameof(TestEvent));

			// Assert
			Assert.False(didEventFire);
			ActionEvent -= HandleDelegateTest;
		}

		[Fact]
		public void WeakEventManagerActionT_UnassignedEvent()
		{
			// Arrange
			var didEventFire = false;

			ActionEvent += HandleDelegateTest;
			ActionEvent -= HandleDelegateTest;
			void HandleDelegateTest(string message)
			{
				Assert.NotNull(message);
				Assert.NotEmpty(message);

				didEventFire = true;
			}

			// Act
			actionEventManager.RaiseEvent("Test", nameof(ActionEvent));

			// Assert
			Assert.False(didEventFire);
		}

		[Fact]
		public void WeakEventManagerActionT_UnassignedEventManager()
		{
			// Arrange
			var unassignedEventManager = new WeakEventManager<string>();
			var didEventFire = false;

			ActionEvent += HandleDelegateTest;
			void HandleDelegateTest(string message)
			{
				Assert.NotNull(message);
				Assert.NotEmpty(message);

				didEventFire = true;
			}

			// Act
			unassignedEventManager.RaiseEvent(string.Empty, nameof(ActionEvent));

			// Assert
			Assert.False(didEventFire);
			ActionEvent -= HandleDelegateTest;
		}

		[Fact]
		public void WeakEventManagerActionT_HandleEvent_InvalidHandleEvent()
		{
			// Arrange
			ActionEvent += HandleDelegateTest;
			var didEventFire = false;

			void HandleDelegateTest(string message)
			{
				Assert.NotNull(message);
				Assert.NotEmpty(message);

				didEventFire = true;
			}

			// Act

			// Assert
			Assert.Throws<InvalidHandleEventException>(() => actionEventManager.RaiseEvent(this, "Test", nameof(ActionEvent)));
			Assert.False(didEventFire);
			ActionEvent -= HandleDelegateTest;
		}

		[Fact]
		public void WeakEventManagerActionT_AddEventHandler_NullHandler()
		{
			// Arrange
			Action<string> nullAction = null;

			// Act

			// Assert
#pragma warning disable CS8604 //Possible null reference argument for parameter
			Assert.Throws<ArgumentNullException>(() => actionEventManager.AddEventHandler(nullAction, nameof(ActionEvent)));
#pragma warning restore CS8604 //Possible null reference argument for parameter

		}

		[Fact]
		public void WeakEventManagerActionT_AddEventHandler_NullEventName()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			Assert.Throws<ArgumentNullException>(() => actionEventManager.AddEventHandler(s => { var temp = s; }, null));
#pragma warning restore CS8625
		}

		[Fact]
		public void WeakEventManagerActionT_AddEventHandler_EmptyEventName()
		{
			// Arrange
			Action<string> nullAction = null;

			// Act

			// Assert
#pragma warning disable CS8604 //Possible null reference argument for parameter
			Assert.Throws<ArgumentNullException>(() => actionEventManager.AddEventHandler(nullAction, string.Empty));
#pragma warning restore CS8604 //Possible null reference argument for parameter
		}

		[Fact]
		public void WeakEventManagerActionT_AddEventHandler_WhitespaceEventName()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			Assert.Throws<ArgumentNullException>(() => actionEventManager.AddEventHandler(s => { var temp = s; }, " "));
#pragma warning restore CS8625
		}

		[Fact]
		public void WeakEventManagerActionT_RemoveEventHandler_NullHandler()
		{
			// Arrange
			Action<string> nullAction = null;

			// Act

			// Assert
#pragma warning disable CS8604 //Possible null reference argument for parameter
			Assert.Throws<ArgumentNullException>(() => actionEventManager.RemoveEventHandler(nullAction));
#pragma warning restore CS8604
		}

		[Fact]
		public void WeakEventManagerActionT_RemoveEventHandler_NullEventName()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			Assert.Throws<ArgumentNullException>(() => actionEventManager.RemoveEventHandler(s => { var temp = s; }, null));
#pragma warning restore CS8625
		}

		[Fact]
		public void WeakEventManagerActionT_RemoveEventHandler_EmptyEventName()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			Assert.Throws<ArgumentNullException>(() => actionEventManager.RemoveEventHandler(s => { var temp = s; }, string.Empty));
#pragma warning restore CS8625
		}

		[Fact]
		public void WeakEventManagerActionT_RemoveEventHandler_WhiteSpaceEventName()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			Assert.Throws<ArgumentNullException>(() => actionEventManager.RemoveEventHandler(s => { var temp = s; }, " "));
#pragma warning restore CS8625
		}
	}
}