﻿using System;
using NUnit.Framework;
using Xamarin.Forms;
using FontElement = Xamarin.Forms.Label; // TODO: Get rid of this after we have default interface implementation in Forms for IFontElement

namespace Xamarin.CommunityToolkit.Markup.UnitTests
{
	[TestFixture]
	public class ElementExtensionsTests : MarkupBaseTestFixture<Label>
	{
		Label Label => Bindable ?? throw new NullReferenceException();

		[Test]
		public void DynamicResource()
		{
			var label = new Label { Resources = new ResourceDictionary { { "TextKey", "TextValue" } } };
			Assert.That(label.Text, Is.EqualTo(Label.TextProperty.DefaultValue));

			label.DynamicResource(Label.TextProperty, "TextKey");
			Assert.That(label.Text, Is.EqualTo("TextValue"));
		}

		[Test]
		public void DynamicResources() => AssertDynamicResources();

		[Test]
		public void RemoveDynamicResources()
		{
			var label = AssertDynamicResources();

			label.RemoveDynamicResources(Label.TextProperty, Label.TextColorProperty);
			label.Resources["TextKey"] = "ChangedTextValue";
			label.Resources["ColorKey"] = Color.Red;

			Assert.That(label.Text, Is.EqualTo("TextValue"));
			Assert.That(label.TextColor, Is.EqualTo(Color.Green));
		}

		Label AssertDynamicResources()
		{
			var label = new Label { Resources = new ResourceDictionary { { "TextKey", "TextValue" }, { "ColorKey", Color.Green } } };

			Assert.That(label.Text, Is.EqualTo(Label.TextProperty.DefaultValue));
			Assert.That(label.TextColor, Is.EqualTo(Label.TextColorProperty.DefaultValue));

			label.DynamicResources((Label.TextProperty, "TextKey"),
								   (Label.TextColorProperty, "ColorKey"));

			Assert.That(label.Text, Is.EqualTo("TextValue"));
			Assert.That(label.TextColor, Is.EqualTo(Color.Green));

			return label;
		}

		[Test]
		public void EffectSingle()
		{
			Label.Effects.Clear();
			Assume.That(Label.Effects.Count, Is.EqualTo(0));

			var effect1 = new NullEffect();
			Label.Effects(effect1);

			Assert.That(Label.Effects.Count, Is.EqualTo(1));
			Assert.That(Label.Effects.Contains(effect1));
		}

		[Test]
		public void EffectsMultiple()
		{
			Label.Effects.Clear();
			Assume.That(Label.Effects.Count, Is.EqualTo(0));

			NullEffect effect1 = new NullEffect(), effect2 = new NullEffect();
			Label.Effects(effect1, effect2);

			Assert.That(Label.Effects.Count, Is.EqualTo(2));
			Assert.That(Label.Effects.Contains(effect1));
			Assert.That(Label.Effects.Contains(effect2));
		}

		[Test]
		public void FontSize()
			=> TestPropertiesSet(l => l?.FontSize(8), (FontElement.FontSizeProperty, 6.0, 8.0));

		[Test]
		public void Bold()
			=> TestPropertiesSet(l => l?.Bold(), (FontElement.FontAttributesProperty, FontAttributes.None, FontAttributes.Bold));

		[Test]
		public void Italic()
			=> TestPropertiesSet(l => l?.Italic(), (FontElement.FontAttributesProperty, FontAttributes.None, FontAttributes.Italic));

		[Test]
		public void FontWithPositionalParameters()
			=> TestPropertiesSet(
					l => l?.Font("AFontName", 8, true, true),
					(FontElement.FontSizeProperty, 6.0, 8.0),
					(FontElement.FontAttributesProperty, FontAttributes.None, FontAttributes.Bold | FontAttributes.Italic),
					(FontElement.FontFamilyProperty, string.Empty, "AFontName"));

		[Test]
		public void FontWithSizeNamedParameter()
			=> TestPropertiesSet(l => l?.Font(size: 8), (FontElement.FontSizeProperty, 6.0, 8.0));

		[Test]
		public void FontWithBoldNamedParameter()
			=> TestPropertiesSet(l => l?.Font(bold: true), (FontElement.FontAttributesProperty, FontAttributes.None, FontAttributes.Bold));

		[Test]
		public void FontWithItalicNamedParameter()
			=> TestPropertiesSet(l => l?.Font(italic: true), (FontElement.FontAttributesProperty, FontAttributes.None, FontAttributes.Italic));

		[Test]
		public void FontWithFamilyNamedParameter()
			=> TestPropertiesSet(l => l?.Font(family: "AFontName"), (FontElement.FontFamilyProperty, string.Empty, "AFontName"));

		[Test]
		public void SupportDerivedFromLabel()
		{
			Assert.IsInstanceOf<DerivedFromLabel>(
				new DerivedFromLabel()
				.Effects(new NullEffect())
				.FontSize(8)
				.Bold()
				.Italic()
				.Font("AFontName", 8, true, true));
		}

		class DerivedFromLabel : Label
		{
		}
	}
}