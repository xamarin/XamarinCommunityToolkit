﻿using System;
using NUnit.Framework;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Markup.UnitTests
{
	[TestFixture]
	public class StyleTests : MarkupBaseTestFixture
	{
		[Test]
		public void ImplicitCast()
		{
			Style<Label>? style = null;

			Style? formsStyle = style;
			Assert.That(formsStyle, Is.Null);

			style = new Style<Label>();
			formsStyle = style;
			Assert.That(ReferenceEquals(style.FormsStyle, formsStyle));
		}

		[Test]
		public void StyleSingleSetter()
		{
			var style = new Style<Label>(
				(Label.TextColorProperty, Color.Red));
			Style? formsStyle = style;

			Assert.That(formsStyle?.Setters?.Count, Is.EqualTo(1));

			var setter = formsStyle?.Setters?[0];
			Assert.That(setter?.Property, Is.EqualTo(Label.TextColorProperty));
			Assert.That(setter?.Value, Is.EqualTo(Color.Red));
		}

		[Test]
		public void StyleMultipleSetters()
		{
			var style = new Style<Label>(
				(Label.TextColorProperty, Color.Red),
				(Label.TranslationXProperty, 8.0));
			Style? formsStyle = style;

			Assert.That(formsStyle?.Setters?.Count, Is.EqualTo(2));

			var setter1 = formsStyle?.Setters?[0];
			Assert.That(setter1?.Property, Is.EqualTo(Label.TextColorProperty));
			Assert.That(setter1?.Value, Is.EqualTo(Color.Red));

			var setter2 = formsStyle?.Setters?[1];
			Assert.That(setter2?.Property, Is.EqualTo(Label.TranslationXProperty));
			Assert.That(setter2?.Value, Is.EqualTo(8.0));
		}

		[Test]
		public void ApplyToDerivedTypes()
		{
			var style = new Style<Label>();
			Style? formsStyle = style;

			Assert.IsFalse(formsStyle?.ApplyToDerivedTypes);
			style.ApplyToDerivedTypes(true);
			Assert.IsTrue(formsStyle?.ApplyToDerivedTypes);
		}

		[Test]
		public void BasedOn()
		{
			Style? baseStyle = new Style<Label>();
			if (baseStyle is null)
				throw new NullReferenceException();

			var style = new Style<Label>().BasedOn(baseStyle);
			Style? formsStyle = style, formsBaseStyle = baseStyle;

			Assert.That(ReferenceEquals(formsStyle?.BasedOn, formsBaseStyle));
		}

		[Test]
		public void AddSingleSetter()
		{
			var style = new Style<Label>();
			Style? formsStyle = style;

			Assume.That(formsStyle?.Setters?.Count ?? 0, Is.EqualTo(0));

			style.Add((Label.TextColorProperty, Color.Red));

			Assert.That(formsStyle?.Setters?.Count, Is.EqualTo(1));

			var setter = formsStyle?.Setters?[0];
			Assert.That(setter?.Property, Is.EqualTo(Label.TextColorProperty));
			Assert.That(setter?.Value, Is.EqualTo(Color.Red));
		}

		[Test]
		public void AddMultipleSetters()
		{
			var style = new Style<Label>();
			Style? formsStyle = style;
			Assume.That(formsStyle?.Setters?.Count ?? 0, Is.EqualTo(0));

			style.Add(
				(Label.TextColorProperty, Color.Red),
				(Label.TranslationXProperty, 8.0));

			Assert.That(formsStyle?.Setters?.Count, Is.EqualTo(2));

			var setter1 = formsStyle?.Setters?[0];
			Assert.That(setter1?.Property, Is.EqualTo(Label.TextColorProperty));
			Assert.That(setter1?.Value, Is.EqualTo(Color.Red));

			var setter2 = formsStyle?.Setters?[1];
			Assert.That(setter2?.Property, Is.EqualTo(Label.TranslationXProperty));
			Assert.That(setter2?.Value, Is.EqualTo(8.0));
		}

		[Test]
		public void AddSingleBehavior()
		{
			var style = new Style<Label>();
			Style? formsStyle = style;
			Assume.That(formsStyle?.Behaviors?.Count ?? 0, Is.EqualTo(0));
			var behavior = new LabelBehavior();

			style.Add(behavior);

			Assert.That(formsStyle?.Behaviors?.Count, Is.EqualTo(1));
			Assert.That(ReferenceEquals(formsStyle?.Behaviors?[0], behavior));
		}

		[Test]
		public void AddMultipleBehaviors()
		{
			var style = new Style<Label>();
			Style? formsStyle = style;
			Assume.That(formsStyle?.Behaviors?.Count ?? 0, Is.EqualTo(0));
			var behavior1 = new LabelBehavior();
			var behavior2 = new LabelBehavior();

			style.Add(behavior1, behavior2);

			Assert.That(formsStyle?.Behaviors?.Count, Is.EqualTo(2));
			Assert.That(ReferenceEquals(formsStyle?.Behaviors?[0], behavior1));
			Assert.That(ReferenceEquals(formsStyle?.Behaviors?[1], behavior2));
		}

		[Test]
		public void AddSingleTrigger()
		{
			var style = new Style<Label>();
			Style? formsStyle = style;
			Assume.That(formsStyle?.Triggers?.Count ?? 0, Is.EqualTo(0));
			var trigger = new Trigger(typeof(Label));

			style.Add(trigger);

			Assert.That(formsStyle?.Triggers?.Count, Is.EqualTo(1));
			Assert.That(ReferenceEquals(formsStyle?.Triggers?[0], trigger));
		}

		[Test]
		public void AddMultipleTriggers()
		{
			var style = new Style<Label>();
			Style? formsStyle = style;
			Assume.That(formsStyle?.Triggers?.Count ?? 0, Is.EqualTo(0));
			var trigger1 = new Trigger(typeof(Label));
			var trigger2 = new Trigger(typeof(Label));

			style.Add(trigger1, trigger2);

			Assert.That(formsStyle?.Triggers?.Count, Is.EqualTo(2));
			Assert.That(ReferenceEquals(formsStyle?.Triggers?[0], trigger1));
			Assert.That(ReferenceEquals(formsStyle?.Triggers?[1], trigger2));
		}

		[Test]
		public void CanCascade()
		{
			var style = new Style<Label>();
			Style? formsStyle = style;

			Assert.IsFalse(formsStyle?.CanCascade);
			style.CanCascade(true);
			Assert.IsTrue(formsStyle?.CanCascade);
		}

		[Test]
		public void Fluent()
		{
			Style? basedOnStyle = new Style<Label>();
			if (basedOnStyle is null)
				throw new NullReferenceException();

			var style =
				new Style<Label>()
				.ApplyToDerivedTypes(true)
				.BasedOn(basedOnStyle)
				.Add((Label.TextColorProperty, Color.Red))
				.Add(new LabelBehavior())
				.Add(new Trigger(typeof(Label)))
				.CanCascade(true);
		}

		class LabelBehavior : Behavior<Label> { }
	}
}