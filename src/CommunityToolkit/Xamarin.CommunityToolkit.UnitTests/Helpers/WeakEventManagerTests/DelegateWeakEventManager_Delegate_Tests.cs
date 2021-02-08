using System;
using System.ComponentModel;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.Helpers;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Helpers.WeakEventManagerTests
{
	public class WeakEventManager_Delegate_Tests : BaseWeakEventManagerTests, INotifyPropertyChanged
	{
		readonly DelegateWeakEventManager propertyChangedWeakEventManager = new DelegateWeakEventManager();

		public event PropertyChangedEventHandler PropertyChanged
		{
			add => propertyChangedWeakEventManager.AddEventHandler(value);
			remove => propertyChangedWeakEventManager.RemoveEventHandler(value);
		}

		[Fact]
		public void WeakEventManagerDelegate_HandleEvent_ValidImplementation()
		{
			// Arrange
			PropertyChanged += HandleDelegateTest;
			var didEventFire = false;

			void HandleDelegateTest(object sender, PropertyChangedEventArgs e)
			{
				Assert.NotNull(sender);
				Assert.Equal(GetType(), sender.GetType());

				Assert.NotNull(e);

				didEventFire = true;
				PropertyChanged -= HandleDelegateTest;
			}

			// Act
			propertyChangedWeakEventManager.RaiseEvent(this, new PropertyChangedEventArgs("Test"), nameof(PropertyChanged));

			// Assert
			Assert.True(didEventFire);
		}

		[Fact]
		public void WeakEventManagerDelegate_HandleEvent_NullSender()
		{
			// Arrange
			PropertyChanged += HandleDelegateTest;
			var didEventFire = false;

			void HandleDelegateTest(object sender, PropertyChangedEventArgs e)
			{
				Assert.Null(sender);
				Assert.NotNull(e);

				didEventFire = true;
				PropertyChanged -= HandleDelegateTest;
			}

			// Act
			propertyChangedWeakEventManager.RaiseEvent(null, new PropertyChangedEventArgs("Test"), nameof(PropertyChanged));

			// Assert
			Assert.True(didEventFire);
		}

		[Fact]
		public void WeakEventManagerDelegate_HandleEvent_InvalidEventArgs()
		{
			// Arrange
			PropertyChanged += HandleDelegateTest;
			var didEventFire = false;

			void HandleDelegateTest(object sender, PropertyChangedEventArgs e) => didEventFire = true;

			// Act

			// Assert
			Assert.Throws<ArgumentException>(() => propertyChangedWeakEventManager.RaiseEvent(this, EventArgs.Empty, nameof(PropertyChanged)));
			Assert.False(didEventFire);
			PropertyChanged -= HandleDelegateTest;
		}

		[Fact]
		public void WeakEventManagerDelegate_HandleEvent_NullEventArgs()
		{
			// Arrange
			PropertyChanged += HandleDelegateTest;
			var didEventFire = false;

			void HandleDelegateTest(object sender, PropertyChangedEventArgs e)
			{
				Assert.NotNull(sender);
				Assert.Equal(GetType(), sender.GetType());

				Assert.Null(e);

				didEventFire = true;
				PropertyChanged -= HandleDelegateTest;
			}

			// Act
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			propertyChangedWeakEventManager.RaiseEvent(this, null, nameof(PropertyChanged));
#pragma warning restore CS8625 //Cannot convert null literal to non-nullable reference type

			// Assert
			Assert.True(didEventFire);
		}

		[Fact]
		public void WeakEventManagerDelegate_HandleEvent_InvalidHandleEventEventName()
		{
			// Arrange
			PropertyChanged += HandleDelegateTest;
			var didEventFire = false;

			void HandleDelegateTest(object sender, PropertyChangedEventArgs e) => didEventFire = true;

			// Act
			propertyChangedWeakEventManager.RaiseEvent(this, new PropertyChangedEventArgs("Test"), nameof(TestStringEvent));

			// Assert
			Assert.False(didEventFire);
			PropertyChanged -= HandleDelegateTest;
		}

		[Fact]
		public void WeakEventManagerDelegate_HandleEvent_DynamicMethod_ValidImplementation()
		{
			// Arrange
			var dynamicMethod = new System.Reflection.Emit.DynamicMethod(string.Empty, typeof(void), new[] { typeof(object), typeof(PropertyChangedEventArgs) });
			var ilGenerator = dynamicMethod.GetILGenerator();
			ilGenerator.Emit(System.Reflection.Emit.OpCodes.Ret);

			var handler = (PropertyChangedEventHandler)dynamicMethod.CreateDelegate(typeof(PropertyChangedEventHandler));
			PropertyChanged += handler;

			// Act

			// Assert
			propertyChangedWeakEventManager.RaiseEvent(this, new PropertyChangedEventArgs("Test"), nameof(PropertyChanged));
			PropertyChanged -= handler;
		}

		[Fact]
		public void WeakEventManagerDelegate_UnassignedEvent()
		{
			// Arrange
			var didEventFire = false;

			PropertyChanged += HandleDelegateTest;
			PropertyChanged -= HandleDelegateTest;
			void HandleDelegateTest(object sender, PropertyChangedEventArgs e) => didEventFire = true;

			// Act
#pragma warning disable CS8625 //Cannot convert null literal to non-nullable reference type
			propertyChangedWeakEventManager.RaiseEvent(null, null, nameof(PropertyChanged));
#pragma warning restore CS8625 //Cannot convert null literal to non-nullable reference type

			// Assert
			Assert.False(didEventFire);
		}

		[Fact]
		public void WeakEventManagerDelegate_UnassignedEventManager()
		{
			// Arrange
			var unassignedEventManager = new DelegateWeakEventManager();
			var didEventFire = false;

			PropertyChanged += HandleDelegateTest;
			void HandleDelegateTest(object sender, PropertyChangedEventArgs e) => didEventFire = true;

			// Act
			unassignedEventManager.RaiseEvent(null, null, nameof(PropertyChanged));

			// Assert
			Assert.False(didEventFire);
			PropertyChanged -= HandleDelegateTest;
		}

		[Fact]
		public void WeakEventManagerDelegate_HandleEvent_InvalidHandleEvent()
		{
			// Arrange
			PropertyChanged += HandleDelegateTest;
			var didEventFire = false;

			void HandleDelegateTest(object sender, PropertyChangedEventArgs e) => didEventFire = true;

			// Act

			// Assert
			Assert.Throws<InvalidHandleEventException>(() => propertyChangedWeakEventManager.RaiseEvent(nameof(PropertyChanged)));
			Assert.False(didEventFire);
			PropertyChanged -= HandleDelegateTest;
		}

		[Fact]
		public void WeakEventManagerDelegate_AddEventHandler_NullHandler()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference
			Assert.Throws<ArgumentNullException>(() => propertyChangedWeakEventManager.AddEventHandler(null));
#pragma warning restore CS8625
		}

		[Fact]
		public void WeakEventManagerDelegate_AddEventHandler_NullEventName()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference
			Assert.Throws<ArgumentNullException>(() => propertyChangedWeakEventManager.AddEventHandler(null, null));
#pragma warning restore CS8625
		}

		[Fact]
		public void WeakEventManagerDelegate_AddEventHandler_EmptyEventName()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference
			Assert.Throws<ArgumentNullException>(() => propertyChangedWeakEventManager.AddEventHandler(null, string.Empty));
#pragma warning restore CS8625
		}

		[Fact]
		public void WeakEventManagerDelegate_AddEventHandler_WhitespaceEventName()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference
			Assert.Throws<ArgumentNullException>(() => propertyChangedWeakEventManager.AddEventHandler(null, " "));
#pragma warning restore CS8625
		}

		[Fact]
		public void WeakEventManagerDelegate_RemoveEventHandler_NullHandler()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference
			Assert.Throws<ArgumentNullException>(() => propertyChangedWeakEventManager.RemoveEventHandler(null));
#pragma warning restore CS8625
		}

		[Fact]
		public void WeakEventManagerDelegate_RemoveEventHandler_NullEventName()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference
			Assert.Throws<ArgumentNullException>(() => propertyChangedWeakEventManager.RemoveEventHandler(null, null));
#pragma warning restore CS8625
		}

		[Fact]
		public void WeakEventManagerDelegate_RemoveEventHandler_EmptyEventName()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference
			Assert.Throws<ArgumentNullException>(() => propertyChangedWeakEventManager.RemoveEventHandler(null, string.Empty));
#pragma warning restore CS8625
		}

		[Fact]
		public void WeakEventManagerDelegate_RemoveEventHandler_WhiteSpaceEventName()
		{
			// Arrange

			// Act

			// Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference
			Assert.Throws<ArgumentNullException>(() => propertyChangedWeakEventManager.RemoveEventHandler(null, " "));
#pragma warning restore CS8625
		}
	}
}