using System;
using NUnit.Framework;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Markup.UnitTests
{
	public class MarkupBaseTestFixture<TBindable> : MarkupBaseTestFixture where TBindable : BindableObject, new()
	{
		protected TBindable? Bindable { get; private set; }

		[SetUp]
		public override void Setup()
		{
			base.Setup();
			Bindable = new TBindable();
		}

		[TearDown]
		public override void TearDown()
		{
			Bindable = null;
			base.TearDown();
		}

		protected void TestPropertiesSet<TPropertyValue>(
			Action<TBindable?> modify,
			params (BindableProperty property, TPropertyValue beforeValue, TPropertyValue expectedValue)[] propertyChanges)
			=> TestPropertiesSet(Bindable, modify, propertyChanges);

		protected void TestPropertiesSet(
			Action<TBindable?> modify,
			params (BindableProperty property, object beforeValue, object expectedValue)[] propertyChanges)
			=> TestPropertiesSet(Bindable, modify, propertyChanges);

		protected void TestPropertiesSet(
			Action<TBindable?> modify,
			params (BindableProperty property, object expectedValue)[] propertyChanges)
			=> TestPropertiesSet(Bindable, modify, propertyChanges);
	}

	public class MarkupBaseTestFixture : BaseTestFixture
	{
		protected void TestPropertiesSet<TBindable, TPropertyValue>(
			TBindable? bindable,
			Action<TBindable?> modify,
			params (BindableProperty property, TPropertyValue beforeValue, TPropertyValue expectedValue)[] propertyChanges) where TBindable : BindableObject
		{
			foreach (var (property, beforeValue, expectedValue) in propertyChanges)
			{
				bindable?.SetValue(property, beforeValue);
				Assume.That(bindable.GetPropertyIfSet(property, expectedValue), Is.Not.EqualTo(expectedValue));
			}

			modify(bindable);

			foreach (var change in propertyChanges)
				Assert.That(bindable.GetPropertyIfSet(change.property, change.beforeValue), Is.EqualTo(change.expectedValue));
		}

		protected void TestPropertiesSet<TBindable, TPropertyValue>(
			TBindable? bindable,
			Action<TBindable?> modify,
			params (BindableProperty property, TPropertyValue expectedValue)[] propertyChanges) where TBindable : BindableObject
		{
			foreach (var (property, expectedValue) in propertyChanges)
			{
				bindable?.SetValue(property, property.DefaultValue);
				Assume.That(bindable.GetPropertyIfSet(property, expectedValue), Is.Not.EqualTo(expectedValue));
			}

			modify(bindable);

			foreach (var (property, expectedValue) in propertyChanges)
				Assert.That(bindable.GetPropertyIfSet(property, property.DefaultValue), Is.EqualTo(expectedValue));
		}
	}
}
