﻿using System;
using System.Linq;
using System.Windows.Input;
using NUnit.Framework;
using Xamarin.CommunityToolkit.Markup.UnitTests.BindableObjectViews;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Markup.UnitTests
{
	[TestFixture]
	public class BindableObjectExtensionsTests : MarkupBaseTestFixture
	{
		ViewModel? viewModel;

		[SetUp]
		public override void Setup()
		{
			base.Setup();
			viewModel = new ViewModel();
		}

		[TearDown]
		public override void TearDown()
		{
			viewModel = null;
			base.TearDown();
		}

		[Test]
		public void BindSpecifiedPropertyWithDefaults()
		{
			var label = new Label();
			label.Bind(Label.TextColorProperty, nameof(viewModel.TextColor));
			BindingHelpers.AssertBindingExists(label, Label.TextColorProperty, nameof(viewModel.TextColor));
		}

		// Note that we test positional parameters to catch API parameter order changes (which would be breaking).
		// Testing named parameters is not useful because a parameter rename operation in the API would also rename it in the test
		[Test]
		public void BindSpecifiedPropertyWithPositionalParameters()
		{
			var button = new Button();
			object converterParameter = 1;
			var stringFormat = nameof(BindSpecifiedPropertyWithPositionalParameters) + " {0}";
			IValueConverter converter = new ToStringConverter();
			object source = new ViewModel();
			object targetNullValue = nameof(BindSpecifiedPropertyWithPositionalParameters) + " null";
			object fallbackValue = nameof(BindSpecifiedPropertyWithPositionalParameters) + " fallback";

			button.Bind(
				Button.TextProperty,
				nameof(viewModel.Text),
				BindingMode.OneWay,
				converter,
				converterParameter,
				stringFormat,
				source,
				targetNullValue,
				fallbackValue
			);

			BindingHelpers.AssertBindingExists(
				button,
				targetProperty: Button.TextProperty,
				path: nameof(viewModel.Text),
				mode: BindingMode.OneWay,
				converter: converter,
				converterParameter: converterParameter,
				stringFormat: stringFormat,
				source: source,
				targetNullValue: targetNullValue,
				fallbackValue: fallbackValue
			);
		}

		[Test]
		public void BindSpecifiedPropertyWithInlineOneWayConvertAndDefaults()
		{
			var label = new Label();
			label.Bind(
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				convert: (bool? isRed) => isRed.HasValue && isRed.Value ? Color.Red : Color.Transparent
			);

			BindingHelpers.AssertBindingExists<Color>(
				label,
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				assertConverterInstanceIsAnyNotNull: true,
				assertConvert: c => c.AssertConvert<bool?, Color>(true, Color.Red).AssertConvert<bool?, Color>(false, Color.Transparent)
			);
		}

		[Test]
		public void BindSpecifiedPropertyWithInlineOneWayParameterizedConvertAndDefaults()
		{
			var label = new Label();
			label.Bind(
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				convert: (bool? isRed, double? alpha) => (isRed.HasValue && isRed.Value ? Color.Red : Color.Green).MultiplyAlpha(alpha ?? throw new NullReferenceException()),
				converterParameter: 0.5
			);

			BindingHelpers.AssertBindingExists<Color, double>(
				label,
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				assertConverterInstanceIsAnyNotNull: true,
				converterParameter: 0.5,
				assertConvert: c => c.AssertConvert<bool?, Color>(true, 0.5, Color.Red.MultiplyAlpha(0.5))
									 .AssertConvert<bool?, Color>(false, 0.2, Color.Green.MultiplyAlpha(0.2))
			);
		}

		[Test]
		public void BindSpecifiedPropertyWithInlineTwoWayConvertAndDefaults()
		{
			var label = new Label();
			label.Bind(
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				BindingMode.TwoWay,
				(bool? isRed) => isRed.HasValue && isRed.Value ? Color.Red : Color.Transparent,
				color => color == Color.Red
			);

			BindingHelpers.AssertBindingExists<Color>(
				label,
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				BindingMode.TwoWay,
				assertConverterInstanceIsAnyNotNull: true,
				assertConvert: c => c.AssertConvert(true, Color.Red, twoWay: true)
									 .AssertConvert(false, Color.Transparent, twoWay: true)
			);
		}

		[Test]
		public void BindSpecifiedPropertyWithInlineTwoWayParameterizedConvertAndDefaults()
		{
			var label = new Label();
			label.Bind(
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				BindingMode.TwoWay,
				(bool? isRed, double? alpha) => (isRed.HasValue && isRed.Value ? Color.Red : Color.Green).MultiplyAlpha(alpha ?? throw new NullReferenceException()),
				(color, alpha) => color == Color.Red.MultiplyAlpha(alpha ?? throw new NullReferenceException()),
				0.5
			);

			BindingHelpers.AssertBindingExists<Color, double>(
				label,
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				BindingMode.TwoWay,
				assertConverterInstanceIsAnyNotNull: true,
				converterParameter: 0.5,
				assertConvert: c => c.AssertConvert(true, 0.5, Color.Red.MultiplyAlpha(0.5), twoWay: true)
									 .AssertConvert(false, 0.2, Color.Green.MultiplyAlpha(0.2), twoWay: true)
			);
		}

		[Test]
		public void BindSpecifiedPropertyWithInlineOneWayConvertAndPositionalParameters()
		{
			var button = new Button();
			var stringFormat = nameof(BindSpecifiedPropertyWithInlineOneWayConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			var targetNullValue = nameof(BindSpecifiedPropertyWithInlineOneWayConvertAndPositionalParameters) + " null";
			var fallbackValue = nameof(BindSpecifiedPropertyWithInlineOneWayConvertAndPositionalParameters) + " fallback";

			button.Bind(
				Button.TextProperty,
				nameof(viewModel.Text),
				BindingMode.OneWay,
				(string? text) => $"'{text?.Trim('\'')}'",
				null,
				stringFormat,
				source,
				targetNullValue,
				fallbackValue
			);

			BindingHelpers.AssertBindingExists<string>(
				button,
				targetProperty: Button.TextProperty,
				path: nameof(viewModel.Text),
				mode: BindingMode.OneWay,
				assertConverterInstanceIsAnyNotNull: true,
				stringFormat: stringFormat,
				source: source,
				targetNullValue: targetNullValue,
				fallbackValue: fallbackValue,
				assertConvert: c => c.AssertConvert("test", "'test'")
			);
		}

		[Test]
		public void BindSpecifiedPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters()
		{
			var button = new Button();
			var converterParameter = 1;
			var stringFormat = nameof(BindSpecifiedPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			var targetNullValue = nameof(BindSpecifiedPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters) + " null";
			var fallbackValue = nameof(BindSpecifiedPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters) + " fallback";

			button.Bind(
				Button.TextProperty,
				nameof(viewModel.Text),
				BindingMode.OneWay,
				(string? text, int? repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat ?? throw new NullReferenceException())),
				null,
				converterParameter,
				stringFormat,
				source,
				targetNullValue,
				fallbackValue
			);

			BindingHelpers.AssertBindingExists<string, int>(
				button,
				targetProperty: Button.TextProperty,
				path: nameof(viewModel.Text),
				mode: BindingMode.OneWay,
				assertConverterInstanceIsAnyNotNull: true,
				converterParameter: converterParameter,
				stringFormat: stringFormat,
				source: source,
				targetNullValue: targetNullValue,
				fallbackValue: fallbackValue,
				assertConvert: c => c.AssertConvert("test", 2, "'test''test'")
			);
		}

		[Test]
		public void BindSpecifiedPropertyWithInlineTwoWayConvertAndPositionalParameters()
		{
			var button = new Button();
			var stringFormat = nameof(BindSpecifiedPropertyWithInlineTwoWayConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			var targetNullValue = nameof(BindSpecifiedPropertyWithInlineTwoWayConvertAndPositionalParameters) + " null";
			var fallbackValue = nameof(BindSpecifiedPropertyWithInlineTwoWayConvertAndPositionalParameters) + " fallback";

			button.Bind(
				Button.TextProperty,
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				(string? text) => $"'{text?.Trim('\'')}'",
				text => text?.Trim('\'') ?? throw new NullReferenceException(),
				stringFormat,
				source,
				targetNullValue,
				fallbackValue
			);

			BindingHelpers.AssertBindingExists(
				button,
				targetProperty: Button.TextProperty,
				path: nameof(viewModel.Text),
				mode: BindingMode.TwoWay,
				assertConverterInstanceIsAnyNotNull: true,
				stringFormat: stringFormat,
				source: source,
				targetNullValue: targetNullValue,
				fallbackValue: fallbackValue,
				assertConvert: c => c.AssertConvert("test", "'test'", twoWay: true)
			);
		}

		[Test]
		public void BindSpecifiedPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters()
		{
			var button = new Button();
			var converterParameter = 1;
			var stringFormat = nameof(BindSpecifiedPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			var targetNullValue = nameof(BindSpecifiedPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters) + " null";
			var fallbackValue = nameof(BindSpecifiedPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters) + " fallback";

			button.Bind(
				Button.TextProperty,
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				(string? text, int? repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat ?? throw new NullReferenceException())),
				(text, repeat) => text?.Substring(0, text.Length / repeat ?? throw new NullReferenceException())?.Trim('\'') ?? throw new NullReferenceException(),
				converterParameter,
				stringFormat,
				source,
				targetNullValue,
				fallbackValue
			);

			BindingHelpers.AssertBindingExists<string, int>(
				button,
				targetProperty: Button.TextProperty,
				path: nameof(viewModel.Text),
				mode: BindingMode.TwoWay,
				assertConverterInstanceIsAnyNotNull: true,
				converterParameter: converterParameter,
				stringFormat: stringFormat,
				source: source,
				targetNullValue: targetNullValue,
				fallbackValue: fallbackValue,
				assertConvert: c => c.AssertConvert("test", 2, "'test''test'", twoWay: true)
			);
		}

		[Test]
		public void BindDefaultPropertyWithDefaults()
		{
			var label = new Label();
			label.Bind(nameof(viewModel.Text));
			BindingHelpers.AssertBindingExists(label, Label.TextProperty, nameof(viewModel.Text));
		}

		[Test]
		public void BindDefaultPropertyWithPositionalParameters()
		{
			var label = new Label();
			object converterParameter = 1;
			var stringFormat = nameof(BindDefaultPropertyWithPositionalParameters) + " {0}";
			IValueConverter converter = new ToStringConverter();
			object source = new ViewModel();
			object targetNullValue = nameof(BindDefaultPropertyWithPositionalParameters) + " null";
			object fallbackValue = nameof(BindDefaultPropertyWithPositionalParameters) + " fallback";

			label.Bind(
				nameof(viewModel.Text),
				BindingMode.OneWay,
				converter,
				converterParameter,
				stringFormat,
				source,
				targetNullValue,
				fallbackValue
			);

			BindingHelpers.AssertBindingExists(
				label,
				targetProperty: Label.TextProperty,
				path: nameof(viewModel.Text),
				mode: BindingMode.OneWay,
				converter: converter,
				converterParameter: converterParameter,
				stringFormat: stringFormat,
				source: source,
				targetNullValue: targetNullValue,
				fallbackValue: fallbackValue
			);
		}

		[Test]
		public void BindDefaultPropertyWithInlineOneWayConvertAndDefaults()
		{
			var label = new Label();
			label.Bind(
				nameof(viewModel.Text),
				convert: (string? text) => $"'{text}'"
			);

			BindingHelpers.AssertBindingExists(
				label,
				Label.TextProperty,
				nameof(viewModel.Text),
				assertConverterInstanceIsAnyNotNull: true,
				assertConvert: c => c.AssertConvert("test", "'test'")
			);
		}

		[Test]
		public void BindDefaultPropertyWithInlineOneWayParameterizedConvertAndDefaults()
		{
			var label = new Label();
			label.Bind(
				nameof(viewModel.Text),
				convert: (string? text, int? repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat ?? throw new NullReferenceException())),
				converterParameter: 1
			);

			BindingHelpers.AssertBindingExists<string, int>(
				label,
				Label.TextProperty,
				nameof(viewModel.Text),
				assertConverterInstanceIsAnyNotNull: true,
				converterParameter: 1,
				assertConvert: c => c.AssertConvert("test", 2, "'test''test'")
			);
		}

		[Test]
		public void BindDefaultPropertyWithInlineTwoWayConvertAndDefaults()
		{
			var label = new Label();
			label.Bind(
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				(string? text) => $"'{text?.Trim('\'')}'",
				text => text?.Trim('\'') ?? throw new NullReferenceException()
			);

			BindingHelpers.AssertBindingExists(
				label,
				Label.TextProperty,
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				assertConverterInstanceIsAnyNotNull: true,
				assertConvert: c => c.AssertConvert("test", "'test'", twoWay: true)
			);
		}

		[Test]
		public void BindDefaultPropertyWithInlineTwoWayParameterizedConvertAndDefaults()
		{
			var label = new Label();
			label.Bind(
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				(string? text, int? repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat ?? throw new NullReferenceException())),
				(text, repeat) => text?.Substring(0, text.Length / repeat ?? throw new NullReferenceException()).Trim('\'') ?? throw new NullReferenceException(),
				2
			);

			BindingHelpers.AssertBindingExists<string, int>(
				label,
				Label.TextProperty,
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				assertConverterInstanceIsAnyNotNull: true,
				converterParameter: 2,
				assertConvert: c => c.AssertConvert("test", 2, "'test''test'", twoWay: true)
			);
		}

		[Test]
		public void BindDefaultPropertyWithInlineOneWayConvertAndPositionalParameters()
		{
			var label = new Label();
			var stringFormat = nameof(BindDefaultPropertyWithInlineOneWayConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			var targetNullValue = nameof(BindDefaultPropertyWithInlineOneWayConvertAndPositionalParameters) + " null";
			var fallbackValue = nameof(BindDefaultPropertyWithInlineOneWayConvertAndPositionalParameters) + " fallback";

			label.Bind(
				nameof(viewModel.Text),
				BindingMode.OneWay,
				(string? text) => $"'{text?.Trim('\'')}'",
				null,
				stringFormat,
				source,
				targetNullValue,
				fallbackValue
			);

			BindingHelpers.AssertBindingExists(
				label,
				targetProperty: Label.TextProperty,
				path: nameof(viewModel.Text),
				mode: BindingMode.OneWay,
				assertConverterInstanceIsAnyNotNull: true,
				stringFormat: stringFormat,
				source: source,
				targetNullValue: targetNullValue,
				fallbackValue: fallbackValue,
				assertConvert: c => c.AssertConvert("test", "'test'")
			);
		}

		[Test]
		public void BindDefaultPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters()
		{
			var label = new Label();
			var converterParameter = 1;
			var stringFormat = nameof(BindDefaultPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			var targetNullValue = nameof(BindDefaultPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters) + " null";
			var fallbackValue = nameof(BindDefaultPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters) + " fallback";

			label.Bind(
				nameof(viewModel.Text),
				BindingMode.OneWay,
				(string? text, int? repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat ?? throw new NullReferenceException())),
				null,
				converterParameter,
				stringFormat,
				source,
				targetNullValue,
				fallbackValue
			);

			BindingHelpers.AssertBindingExists<string, int>(
				label,
				targetProperty: Label.TextProperty,
				path: nameof(viewModel.Text),
				mode: BindingMode.OneWay,
				assertConverterInstanceIsAnyNotNull: true,
				converterParameter: converterParameter,
				stringFormat: stringFormat,
				source: source,
				targetNullValue: targetNullValue,
				fallbackValue: fallbackValue,
				assertConvert: c => c.AssertConvert("test", 2, "'test''test'")
			);
		}

		[Test]
		public void BindDefaultPropertyWithInlineTwoWayConvertAndPositionalParameters()
		{
			var label = new Label();
			var stringFormat = nameof(BindDefaultPropertyWithInlineTwoWayConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			var targetNullValue = nameof(BindDefaultPropertyWithInlineTwoWayConvertAndPositionalParameters) + " null";
			var fallbackValue = nameof(BindDefaultPropertyWithInlineTwoWayConvertAndPositionalParameters) + " fallback";

			label.Bind(
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				(string? text) => $"'{text?.Trim('\'')}'",
				text => text?.Trim('\'') ?? throw new NullReferenceException(),
				stringFormat,
				source,
				targetNullValue,
				fallbackValue
			);

			BindingHelpers.AssertBindingExists(
				label,
				targetProperty: Label.TextProperty,
				path: nameof(viewModel.Text),
				mode: BindingMode.TwoWay,
				assertConverterInstanceIsAnyNotNull: true,
				stringFormat: stringFormat,
				source: source,
				targetNullValue: targetNullValue,
				fallbackValue: fallbackValue,
				assertConvert: c => c.AssertConvert("test", "'test'", twoWay: true)
			);
		}

		[Test]
		public void BindDefaultPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters()
		{
			var label = new Label();
			var converterParameter = 1;
			var stringFormat = nameof(BindDefaultPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			var targetNullValue = nameof(BindDefaultPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters) + " null";
			var fallbackValue = nameof(BindDefaultPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters) + " fallback";

			label.Bind(
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				(string? text, int? repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat ?? throw new NullReferenceException())),
				(text, repeat) => text?.Substring(0, text.Length / repeat ?? throw new NullReferenceException()).Trim('\'') ?? throw new NullReferenceException(),
				converterParameter,
				stringFormat,
				source,
				targetNullValue,
				fallbackValue
			);

			BindingHelpers.AssertBindingExists<string, int>(
				label,
				targetProperty: Label.TextProperty,
				path: nameof(viewModel.Text),
				mode: BindingMode.TwoWay,
				assertConverterInstanceIsAnyNotNull: true,
				converterParameter: converterParameter,
				stringFormat: stringFormat,
				source: source,
				targetNullValue: targetNullValue,
				fallbackValue: fallbackValue,
				assertConvert: c => c.AssertConvert("test", 2, "'test''test'", twoWay: true)
			);
		}

		[Test]
		public void BindCommandWithDefaults()
		{
			var textCell = new TextCell();
			var path = nameof(viewModel.Command);

			textCell.BindCommand(path);

			BindingHelpers.AssertBindingExists(textCell, TextCell.CommandProperty, path);
			BindingHelpers.AssertBindingExists(textCell, TextCell.CommandParameterProperty);
		}

		[Test]
		public void BindCommandWithoutParameter()
		{
			var textCell = new TextCell();
			var path = nameof(viewModel.Command);

			textCell.BindCommand(path, parameterPath: null);

			BindingHelpers.AssertBindingExists(textCell, TextCell.CommandProperty, path);
			Assert.That(BindingHelpers.GetBinding(textCell, TextCell.CommandParameterProperty), Is.Null);
		}

		[Test]
		public void BindCommandWithPositionalParameters()
		{
			var textCell = new TextCell();
			object source = new ViewModel();
			var path = nameof(viewModel.Command);
			var parameterPath = nameof(viewModel.Id);
			object parameterSource = new ViewModel();

			textCell.BindCommand(path, source, parameterPath, parameterSource);

			BindingHelpers.AssertBindingExists(textCell, TextCell.CommandProperty, path, source: source);
			BindingHelpers.AssertBindingExists(textCell, TextCell.CommandParameterProperty, parameterPath, source: parameterSource);
		}

		[Test]
		public void Assign()
		{
			var createdLabel = new Label().Assign(out Label assignLabel);
			Assert.That(ReferenceEquals(createdLabel, assignLabel));
		}

		[Test]
		public void Invoke()
		{
			var createdLabel = new Label().Invoke(null).Invoke(l => l.Text = nameof(Invoke));
			Assert.That(createdLabel.Text, Is.EqualTo(nameof(Invoke)));
		}

		[Test]
		public void SupportDerivedElements()
		{
			Assert.IsInstanceOf<DerivedFromLabel>(
				new DerivedFromLabel()
				.Bind(nameof(viewModel.Text))
				.Bind(
					nameof(viewModel.Text),
					convert: (string? text) => $"'{text}'")
				.Bind(
					nameof(viewModel.Text),
					convert: (string? text, int? repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat ?? throw new NullReferenceException())))
				.Bind(
					DerivedFromLabel.TextColorProperty,
					nameof(viewModel.TextColor))
				.Bind(
					DerivedFromLabel.BackgroundColorProperty,
					nameof(viewModel.IsRed),
					convert: (bool? isRed) => isRed.HasValue && isRed.Value ? Color.Black : Color.Transparent)
				.Bind(
					Label.TextColorProperty,
					nameof(viewModel.IsRed),
					convert: (bool? isRed, double? alpha) => (isRed.HasValue && isRed.Value ? Color.Red : Color.Green).MultiplyAlpha(alpha ?? throw new NullReferenceException()))
				.Invoke(l => l.Text = nameof(SupportDerivedElements))
				.Assign(out DerivedFromLabel assignDerivedFromLabel));

			Assert.IsInstanceOf<DerivedFromTextCell>(
				new DerivedFromTextCell()
				.BindCommand(nameof(viewModel.Command)));
		}

		class ViewModel
		{
			public Guid Id { get; set; }

			public ICommand? Command { get; set; }

			public string? Text { get; set; }

			public Color TextColor { get; set; }

			public bool IsRed { get; set; }
		}
	}
}

#pragma warning disable SA1403 // File may only contain a single namespace
namespace Xamarin.CommunityToolkit.Markup.UnitTests.BindableObjectViews // This namespace simulates derived controls defined in a separate app, for use in the tests in this file only
#pragma warning restore SA1403 // File may only contain a single namespace
{
	class DerivedFromLabel : Label { }

	class DerivedFromTextCell : TextCell { }
}