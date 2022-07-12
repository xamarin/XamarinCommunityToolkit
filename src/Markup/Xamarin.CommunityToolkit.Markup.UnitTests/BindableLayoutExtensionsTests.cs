﻿using System;
using NUnit.Framework;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Markup.UnitTests
{
	[TestFixture]
	public class BindableLayoutExtensionsTests : MarkupBaseTestFixture<StackLayout>
	{
		[Test]
		public void EmptyView()
		{
			var view = new BoxView();
			TestPropertiesSet(l => l?.EmptyView(view), (BindableLayout.EmptyViewProperty, view));
		}

		[Test]
		public void EmptyViewTemplate()
		{
			var template = new DataTemplate(() => new BoxView());
			TestPropertiesSet(l => l?.EmptyViewTemplate(template), (BindableLayout.EmptyViewTemplateProperty, template));
		}

		[Test]
		public void EmptyViewTemplateFunction()
		{
			Func<object> loadTemplate = () => new BoxView();
			Bindable?.EmptyViewTemplate(loadTemplate);
			Assert.That(BindableLayout.GetEmptyViewTemplate(Bindable), Is.Not.Null);
		}

		[Test]
		public void ItemsSource()
		{
			var source = new string[] { };
			TestPropertiesSet(l => l?.ItemsSource(source), (BindableLayout.ItemsSourceProperty, source));
		}

		[Test]
		public void ItemTemplate()
		{
			var template = new DataTemplate(() => new BoxView());
			TestPropertiesSet(l => l?.ItemTemplate(template), (BindableLayout.ItemTemplateProperty, template));
		}

		[Test]
		public void ItemTemplateFunction()
		{
			Func<object> loadTemplate = () => new BoxView();
			Bindable?.ItemTemplate(loadTemplate);
			Assert.That(BindableLayout.GetItemTemplate(Bindable), Is.Not.Null);
		}

		[Test]
		public void ItemTemplateSelector()
		{
			var selector = new Selector();
			TestPropertiesSet(l => l?.ItemTemplateSelector(selector), (BindableLayout.ItemTemplateSelectorProperty, selector));
		}

		class Selector : DataTemplateSelector
		{
			protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
			=> new DataTemplate(() => new BoxView());
		}

		[Test]
		public void SupportDerivedFromView()
		{
			_ = new DerivedFromView()
				.EmptyView(new BoxView())
				.EmptyViewTemplate(new DataTemplate(() => new BoxView()))
				.ItemsSource(new string[] { })
				.ItemTemplate(new DataTemplate(() => new BoxView()))
				.ItemTemplateSelector(new Selector());
		}

		class DerivedFromView : StackLayout { }
	}
}