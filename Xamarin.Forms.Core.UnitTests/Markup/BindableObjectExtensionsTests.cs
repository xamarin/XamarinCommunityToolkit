﻿using System;
using System.Reflection;
using NUnit.Framework;

namespace Xamarin.Forms.Markup.UnitTests
{
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Net.Http.Headers;
	using System.Windows.Input;
	using XamarinFormsMarkupUnitTestsBindableObjectViews;

	[TestFixture(true)]
	[TestFixture(false)]
	public class BindableObjectExtensionsTests : MarkupBaseTestFixture
	{
		ViewModel viewModel;

		public BindableObjectExtensionsTests(bool withExperimentalFlag) : base(withExperimentalFlag) { }

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
		public void BindSpecifiedPropertyWithDefaults() => AssertExperimental(() =>
		{
			var label = new Label();
			label.Bind(Label.TextColorProperty, nameof(viewModel.TextColor));
			BindingHelpers.AssertBindingExists(label, Label.TextColorProperty, nameof(viewModel.TextColor));
		});

		// Note that we test positional parameters to catch API parameter order changes (which would be breaking).
		// Testing named parameters is not useful because a parameter rename operation in the API would also rename it in the test
		[Test]
		public void BindSpecifiedPropertyWithPositionalParameters() => AssertExperimental(() =>
		{
			var button = new Button();
			object converterParameter = 1;
			string stringFormat = nameof(BindSpecifiedPropertyWithPositionalParameters) + " {0}";
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
		});

		[Test]
		public void BindSpecifiedPropertyWithInlineOneWayConvertAndDefaults() => AssertExperimental(() =>
		{
			var label = new Label();
			label.Bind(
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				convert: (bool isRed) => isRed ? Color.Red : Color.Transparent
			);

			BindingHelpers.AssertBindingExists<Color>(
				label,
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				assertConverterInstanceIsAnyNotNull: true,
				assertConvert: c => c.AssertConvert(true, Color.Red).AssertConvert(false, Color.Transparent)
			);
		});

		[Test]
		public void BindSpecifiedPropertyWithInlineOneWayParameterizedConvertAndDefaults() => AssertExperimental(() =>
		{
			var label = new Label();
			label.Bind(
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				convert: (bool isRed, double alpha) => (isRed ? Color.Red : Color.Green).MultiplyAlpha(alpha),
				converterParameter: 0.5
			);

			BindingHelpers.AssertBindingExists<Color, double>(
				label,
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				assertConverterInstanceIsAnyNotNull: true,
				converterParameter: 0.5,
				assertConvert: c => c.AssertConvert(true, 0.5, Color.Red.MultiplyAlpha(0.5))
									 .AssertConvert(false, 0.2, Color.Green.MultiplyAlpha(0.2))
			);
		});

		[Test]
		public void BindSpecifiedPropertyWithInlineTwoWayConvertAndDefaults() => AssertExperimental(() =>
		{
			var label = new Label();
			label.Bind(
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				BindingMode.TwoWay,
				(bool isRed) => isRed ? Color.Red : Color.Transparent,
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
		});

		[Test]
		public void BindSpecifiedPropertyWithInlineTwoWayParameterizedConvertAndDefaults() => AssertExperimental(() =>
		{
			var label = new Label();
			label.Bind(
				Label.TextColorProperty,
				nameof(viewModel.IsRed),
				BindingMode.TwoWay,
				(bool isRed, double alpha) => (isRed ? Color.Red : Color.Green).MultiplyAlpha(alpha),
				(color, alpha) => color == Color.Red.MultiplyAlpha(alpha),
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
		});

		[Test]
		public void BindSpecifiedPropertyWithInlineOneWayConvertAndPositionalParameters() => AssertExperimental(() =>
		{
			var button = new Button();
			string stringFormat = nameof(BindSpecifiedPropertyWithInlineOneWayConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			string targetNullValue = nameof(BindSpecifiedPropertyWithInlineOneWayConvertAndPositionalParameters) + " null";
			string fallbackValue = nameof(BindSpecifiedPropertyWithInlineOneWayConvertAndPositionalParameters) + " fallback";

			button.Bind(
				Button.TextProperty,
				nameof(viewModel.Text),
				BindingMode.OneWay,
				(string text) => $"'{text?.Trim('\'')}'",
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
		});

		[Test]
		public void BindSpecifiedPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters() => AssertExperimental(() =>
		{
			var button = new Button();
			int converterParameter = 1;
			string stringFormat = nameof(BindSpecifiedPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			string targetNullValue = nameof(BindSpecifiedPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters) + " null";
			string fallbackValue = nameof(BindSpecifiedPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters) + " fallback";

			button.Bind(
				Button.TextProperty,
				nameof(viewModel.Text),
				BindingMode.OneWay,
				(string text, int repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat)),
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
		});

		[Test]
		public void BindSpecifiedPropertyWithInlineTwoWayConvertAndPositionalParameters() => AssertExperimental(() =>
		{
			var button = new Button();
			string stringFormat = nameof(BindSpecifiedPropertyWithInlineTwoWayConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			string targetNullValue = nameof(BindSpecifiedPropertyWithInlineTwoWayConvertAndPositionalParameters) + " null";
			string fallbackValue = nameof(BindSpecifiedPropertyWithInlineTwoWayConvertAndPositionalParameters) + " fallback";

			button.Bind(
				Button.TextProperty,
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				(string text) => $"'{text?.Trim('\'')}'",
				text => text?.Trim('\''),
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
		});

		[Test]
		public void BindSpecifiedPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters() => AssertExperimental(() =>
		{
			var button = new Button();
			int converterParameter = 1;
			string stringFormat = nameof(BindSpecifiedPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			string targetNullValue = nameof(BindSpecifiedPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters) + " null";
			string fallbackValue = nameof(BindSpecifiedPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters) + " fallback";

			button.Bind(
				Button.TextProperty,
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				(string text, int repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat)),
				(text, repeat) => text?.Substring(0, text.Length / repeat).Trim('\''),
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
		});

		[Test]
		public void BindDefaultPropertyWithDefaults() => AssertExperimental(() =>
		{
			var label = new Label();
			label.Bind(nameof(viewModel.Text));
			BindingHelpers.AssertBindingExists(label, Label.TextProperty, nameof(viewModel.Text));
		});

		[Test]
		public void BindDefaultPropertyWithPositionalParameters() => AssertExperimental(() =>
		{
			var label = new Label();
			object converterParameter = 1;
			string stringFormat = nameof(BindDefaultPropertyWithPositionalParameters) + " {0}";
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
		});

		[Test]
		public void BindDefaultPropertyWithInlineOneWayConvertAndDefaults() => AssertExperimental(() =>
		{
			var label = new Label();
			label.Bind(
				nameof(viewModel.Text),
				convert: (string text) => $"'{text}'"
			);

			BindingHelpers.AssertBindingExists(
				label,
				Label.TextProperty,
				nameof(viewModel.Text),
				assertConverterInstanceIsAnyNotNull: true,
				assertConvert: c => c.AssertConvert("test", "'test'")
			);
		});

		[Test]
		public void BindDefaultPropertyWithInlineOneWayParameterizedConvertAndDefaults() => AssertExperimental(() =>
		{
			var label = new Label();
			label.Bind(
				nameof(viewModel.Text),
				convert: (string text, int repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat)),
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
		});

		[Test]
		public void BindDefaultPropertyWithInlineTwoWayConvertAndDefaults() => AssertExperimental(() =>
		{
			var label = new Label();
			label.Bind(
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				(string text) => $"'{text?.Trim('\'')}'",
				text => text?.Trim('\'')
			);

			BindingHelpers.AssertBindingExists(
				label,
				Label.TextProperty,
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				assertConverterInstanceIsAnyNotNull: true,
				assertConvert: c => c.AssertConvert("test", "'test'", twoWay: true)
			);
		});

		[Test]
		public void BindDefaultPropertyWithInlineTwoWayParameterizedConvertAndDefaults() => AssertExperimental(() =>
		{
			var label = new Label();
			label.Bind(
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				(string text, int repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat)),
				(text, repeat) => text?.Substring(0, text.Length / repeat).Trim('\''),
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
		});

		[Test]
		public void BindDefaultPropertyWithInlineOneWayConvertAndPositionalParameters() => AssertExperimental(() =>
		{
			var label = new Label();
			string stringFormat = nameof(BindDefaultPropertyWithInlineOneWayConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			string targetNullValue = nameof(BindDefaultPropertyWithInlineOneWayConvertAndPositionalParameters) + " null";
			string fallbackValue = nameof(BindDefaultPropertyWithInlineOneWayConvertAndPositionalParameters) + " fallback";

			label.Bind(
				nameof(viewModel.Text),
				BindingMode.OneWay,
				(string text) => $"'{text?.Trim('\'')}'",
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
		});

		[Test]
		public void BindDefaultPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters() => AssertExperimental(() =>
		{
			var label = new Label();
			int converterParameter = 1;
			string stringFormat = nameof(BindDefaultPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			string targetNullValue = nameof(BindDefaultPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters) + " null";
			string fallbackValue = nameof(BindDefaultPropertyWithInlineOneWayParameterizedConvertAndPositionalParameters) + " fallback";

			label.Bind(
				nameof(viewModel.Text),
				BindingMode.OneWay,
				(string text, int repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat)),
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
		});

		[Test]
		public void BindDefaultPropertyWithInlineTwoWayConvertAndPositionalParameters() => AssertExperimental(() =>
		{
			var label = new Label();
			string stringFormat = nameof(BindDefaultPropertyWithInlineTwoWayConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			string targetNullValue = nameof(BindDefaultPropertyWithInlineTwoWayConvertAndPositionalParameters) + " null";
			string fallbackValue = nameof(BindDefaultPropertyWithInlineTwoWayConvertAndPositionalParameters) + " fallback";

			label.Bind(
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				(string text) => $"'{text?.Trim('\'')}'",
				text => text?.Trim('\''),
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
		});

		[Test]
		public void BindDefaultPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters() => AssertExperimental(() =>
		{
			var label = new Label();
			int converterParameter = 1;
			string stringFormat = nameof(BindDefaultPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters) + " {0}";
			object source = new ViewModel();
			string targetNullValue = nameof(BindDefaultPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters) + " null";
			string fallbackValue = nameof(BindDefaultPropertyWithInlineTwoWayParameterizedConvertAndPositionalParameters) + " fallback";

			label.Bind(
				nameof(viewModel.Text),
				BindingMode.TwoWay,
				(string text, int repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat)),
				(text, repeat) => text?.Substring(0, text.Length / repeat).Trim('\''),
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
		});


		// TODO: Complete unit tests for multibind

		[Test]
		[TestCase(true, false)]
		[TestCase(false, true)]
		[TestCase(true, true)]
		public void BindSpecifiedPropertyWith4BindingsAndInlineConvert(bool testConvert, bool testConvertBack)
		{
			var label = new Label();

			var bindings = new List<BindingBase> {
				new Binding(nameof(viewModel.Text)),
				new Binding(nameof(viewModel.Id)),
				new Binding(nameof(viewModel.IsRed)),
				new Binding(nameof(viewModel.Fraction))
			};

			// Repeat inline converter code for each variantion to test that the Bind overloads allow inferring the generic conversion parameter types
			// The developer should not have to specify generic type parameters
			if (testConvert && testConvertBack)
			{
				label.Bind(
					Label.TextProperty,
					bindings[0], bindings[1], bindings[2], bindings[3],
					((string text, Guid id, bool isRed, double fraction) values) => $"Text = '{ values.text }', Id = '{ values.id }', IsRed = '{ values.isRed }', Fraction = '{ values.fraction }'",
					(string formatted) =>
					{
						var split = formatted.Split('\'');
						return (
							split[1],
							Guid.Parse(split[3]),
							bool.Parse(split[5]),
							double.Parse(split[7])
						);
					}
				);
			} else if (testConvert && !testConvertBack) {
				label.Bind(
					Label.TextProperty,
					bindings[0], bindings[1], bindings[2], bindings[3],
					((string text, Guid id, bool isRed, double fraction) values) => $"Text = '{ values.text }', Id = '{ values.id }', IsRed = '{ values.isRed }', Fraction = '{ values.fraction }'"
				);
			}
			else if (!testConvert && testConvertBack) {
				label.Bind(
					Label.TextProperty,
					bindings[0], bindings[1], bindings[2], bindings[3],
					convertBack: (string formatted) =>
					{
						var split = formatted.Split('\'');
						return (
							split[1],
							Guid.Parse(split[3]),
							bool.Parse(split[5]),
							double.Parse(split[7])
						);
					}
				);
			}

			var avalues = new object[] {
				"Hi",
				Guid.Parse("{272383A4-92E3-46BA-99DC-438D81E039AB}"),
				true,
				0.5
			};

			BindingHelpers.AssertBindingExists<string>(
				label,
				targetProperty: Label.TextProperty,
				bindings,
				assertConverterInstanceIsAnyNotNull: true,
				assertConvert: c => { c.AssertConvert(
					avalues, 
					$"Text = '{ avalues[0] }', Id = '{ avalues[1] }', IsRed = '{ avalues[2] }', Fraction = '{ avalues[3] }'",
					twoWay: testConvert && testConvertBack,
					backOnly: !testConvert && testConvertBack
				); }
			);
		}

		[Test]
		[TestCase(true, false)]
		[TestCase(false, true)]
		[TestCase(true, true)]
		public void BindSpecifiedPropertyWithMultipleBindings(bool testConvert, bool testConvertBack)
		{
			var label = new Label();
			var bindings = new List<BindingBase> {
				new Binding(nameof(viewModel.Text)),
				new Binding(nameof(viewModel.Id)),
				new Binding(nameof(viewModel.IsRed)),
				new Binding(nameof(viewModel.Fraction)),
				new Binding(nameof(viewModel.Count)),
			};

			Func<object[], string> convert = null;
			if (testConvert) convert = (object[] values) => $"Text = '{ values[0] }', Id = '{ values[1] }', IsRed = '{ values[2] }', Fraction = '{ values[3] }', Count = '{ values[4] }'";

			Func<string, object[]> convertBack = null;
			if (testConvertBack)
				convertBack = (string text) => {
					var split = text.Split('\'');
					return new object[]
					{
						split[1],
						Guid.Parse(split[3]),
						bool.Parse(split[5]),
						double.Parse(split[7]),
						int.Parse(split[9])
					};
				};

			var converter = new FuncMultiConverter<string, object>(convert, convertBack);

			label.Bind(
				Label.TextProperty,
				bindings,
				converter
			);

			var avalues = new object[] {
				"Hi",
				Guid.Parse("{272383A4-92E3-46BA-99DC-438D81E039AB}"),
				true,
				0.5,
				3
			};

			BindingHelpers.AssertBindingExists<string>(
				label,
				targetProperty: Label.TextProperty,
				bindings,
				converter,
				assertConvert: c => { c.AssertConvert(
					avalues, 
					$"Text = '{ avalues[0] }', Id = '{ avalues[1] }', IsRed = '{ avalues[2] }', Fraction = '{ avalues[3] }', Count = '{ avalues[4] }'",
					twoWay: testConvert && testConvertBack,
					backOnly: !testConvert && testConvertBack
				); }
			);
		}

		List<BindingBase> GetTestBindings(int count) => new List<BindingBase> {
			new Binding(nameof(viewModel.Text)),
			new Binding(nameof(viewModel.Id)),
			new Binding(nameof(viewModel.IsRed)),
			new Binding(nameof(viewModel.Fraction)),
			new Binding(nameof(viewModel.Count))
		}.Take(count).ToList();

		object[] GetTestConvertValues(int count) => new List<object> {
			"Hi",
			Guid.Parse("{272383A4-92E3-46BA-99DC-438D81E039AB}"),
			true,
			0.5,
			3
		}.Take(count).ToArray();

		void AssertLabelTextMultiBound(Label label, int nBindings, bool testConvert, bool testConvertBack, IMultiValueConverter converter = null)
		{
			var values = GetTestConvertValues(nBindings);
			string expected               = $"Text = '{ values[0] }', Id = '{ values[1] }'";
			if (nBindings >= 3) expected += $", IsRed = '{ values[2] }'";
			if (nBindings >= 4) expected += $", Fraction = '{ values[3] }'";
			if (nBindings >= 5) expected += $", Count = '{ values[4] }'";

			BindingHelpers.AssertBindingExists<string>(
				label,
				targetProperty: Label.TextProperty,
				GetTestBindings(nBindings),
				converter,
				assertConverterInstanceIsAnyNotNull: converter == null,
				assertConvert: c => c.AssertConvert(values, expected, twoWay: testConvert && testConvertBack, backOnly: !testConvert && testConvertBack)
			);
		}

		[Test]
		public void BindCommandWithDefaults() => AssertExperimental(() =>
		{
			var textCell = new TextCell();
			string path = nameof(viewModel.Command);

			textCell.BindCommand(path);

			BindingHelpers.AssertBindingExists(textCell, TextCell.CommandProperty, path);
			BindingHelpers.AssertBindingExists(textCell, TextCell.CommandParameterProperty);
		});

		[Test]
		public void BindCommandWithoutParameter() => AssertExperimental(() =>
		{
			var textCell = new TextCell();
			string path = nameof(viewModel.Command);

			textCell.BindCommand(path, parameterPath: null);

			BindingHelpers.AssertBindingExists(textCell, TextCell.CommandProperty, path);
			Assert.That(BindingHelpers.GetBinding(textCell, TextCell.CommandParameterProperty), Is.Null);
		});

		[Test]
		public void BindCommandWithPositionalParameters() => AssertExperimental(() =>
		{
			var textCell = new TextCell();
			object source = new ViewModel();
			string path = nameof(viewModel.Command);
			string parameterPath = nameof(viewModel.Id);
			object parameterSource = new ViewModel();

			textCell.BindCommand(path, source, parameterPath, parameterSource);

			BindingHelpers.AssertBindingExists(textCell, TextCell.CommandProperty, path, source: source);
			BindingHelpers.AssertBindingExists(textCell, TextCell.CommandParameterProperty, parameterPath, source: parameterSource);
		});

		[Test]
		public void Assign() => AssertExperimental(() =>
		{
			var createdLabel = new Label().Assign(out Label assignLabel);
			Assert.That(Object.ReferenceEquals(createdLabel, assignLabel));
		});

		[Test]
		public void Invoke() => AssertExperimental(() =>
		{
			var createdLabel = new Label().Invoke(null).Invoke(l => l.Text = nameof(Invoke));
			Assert.That(createdLabel.Text, Is.EqualTo(nameof(Invoke)));
		});

		[Test]
		public void SupportDerivedElements() => AssertExperimental(() =>
		{
			DerivedFromLabel _ =
				new DerivedFromLabel()
				.Bind(nameof(viewModel.Text))
				.Bind(
					nameof(viewModel.Text),
					convert: (string text) => $"'{text}'")
				.Bind(
					nameof(viewModel.Text),
					convert: (string text, int repeat) => string.Concat(Enumerable.Repeat($"'{text?.Trim('\'')}'", repeat)))
				.Bind(
					DerivedFromLabel.TextColorProperty,
					nameof(viewModel.TextColor))
				.Bind(
					DerivedFromLabel.BackgroundColorProperty,
					nameof(viewModel.IsRed),
					convert: (bool isRed) => isRed ? Color.Black : Color.Transparent)
				.Bind(
					Label.TextColorProperty,
					nameof(viewModel.IsRed),
					convert: (bool isRed, double alpha) => (isRed ? Color.Red : Color.Green).MultiplyAlpha(alpha))
				.Invoke(l => l.Text = nameof(SupportDerivedElements))
				.Assign(out DerivedFromLabel assignDerivedFromLabel);

			DerivedFromTextCell __ =
				new DerivedFromTextCell()
				.BindCommand(nameof(viewModel.Command));
		});

		class ViewModel
		{
			public Guid Id { get; set; }
			public ICommand Command { get; set; }
			public string Text { get; set; }
			public Color TextColor { get; set; }
			public bool IsRed { get; set; }
			public double Fraction { get; set; }
			public int Count { get; set; }
		}
	}

	internal static class BindingHelpers
	{
		static MethodInfo getContextMethodInfo;
		static FieldInfo bindingFieldInfo;

		internal static void AssertBindingExists(
			BindableObject bindable,
			BindableProperty targetProperty,
			string path = ".",
			BindingMode mode = BindingMode.Default,
			bool assertConverterInstanceIsAnyNotNull = false,
			IValueConverter converter = null,
			object converterParameter = null,
			string stringFormat = null,
			object source = null,
			object targetNullValue = default,
			object fallbackValue = default,
			Action<IValueConverter> assertConvert = null
		) => AssertBindingExists<object, object>(
				bindable, targetProperty, path, mode, assertConverterInstanceIsAnyNotNull, converter, converterParameter,
				stringFormat, source, targetNullValue, fallbackValue, assertConvert
		);

		internal static void AssertBindingExists<TDest>(
			BindableObject bindable,
			BindableProperty targetProperty,
			string path = ".",
			BindingMode mode = BindingMode.Default,
			bool assertConverterInstanceIsAnyNotNull = false,
			IValueConverter converter = null,
			string stringFormat = null,
			object source = null,
			TDest targetNullValue = default,
			TDest fallbackValue = default,
			Action<IValueConverter> assertConvert = null
		) => AssertBindingExists<TDest, object>(
				bindable, targetProperty, path, mode, assertConverterInstanceIsAnyNotNull, converter, null,
				stringFormat, source, targetNullValue, fallbackValue, assertConvert
		);

		internal static void AssertBindingExists<TDest, TParam>(
			BindableObject bindable,
			BindableProperty targetProperty,
			string path = ".",
			BindingMode mode = BindingMode.Default,
			bool assertConverterInstanceIsAnyNotNull = false,
			IValueConverter converter = null,
			TParam converterParameter = default,
			string stringFormat = null,
			object source = null,
			TDest targetNullValue = default,
			TDest fallbackValue = default,
			Action<IValueConverter> assertConvert = null
		)
		{
			var binding = BindingHelpers.GetBinding(bindable, targetProperty);
			Assert.That(binding, Is.Not.Null);
			Assert.That(binding.Path, Is.EqualTo(path));
			Assert.That(binding.Mode, Is.EqualTo(mode));
			if (assertConverterInstanceIsAnyNotNull)
				Assert.That(binding.Converter, Is.Not.Null);
			else
				Assert.That(binding.Converter, Is.EqualTo(converter));
			Assert.That(binding.ConverterParameter, Is.EqualTo(converterParameter));
			Assert.That(binding.StringFormat, Is.EqualTo(stringFormat));
			Assert.That(binding.Source, Is.EqualTo(source));
			Assert.That(binding.TargetNullValue, Is.EqualTo(targetNullValue));
			Assert.That(binding.FallbackValue, Is.EqualTo(fallbackValue));

			assertConvert?.Invoke(binding.Converter);
		}

		internal static void AssertBindingExists<TDest>(
			BindableObject bindable,
			BindableProperty targetProperty,
			IList<BindingBase> bindings,
			IMultiValueConverter converter = null,
			BindingMode mode = BindingMode.Default,
			bool assertConverterInstanceIsAnyNotNull = false,
			string stringFormat = null,
			TDest targetNullValue = default,
			TDest fallbackValue = default,
			Action<IMultiValueConverter> assertConvert = null
		) => AssertBindingExists<TDest, object>(
			bindable, targetProperty, bindings, converter, null, mode, assertConverterInstanceIsAnyNotNull,
			stringFormat, targetNullValue, fallbackValue, assertConvert);


		internal static void AssertBindingExists<TDest, TParam>(
			BindableObject bindable,
			BindableProperty targetProperty,
			IList<BindingBase> bindings,
			IMultiValueConverter converter = null,
			TParam converterParameter = default,
			BindingMode mode = BindingMode.Default,
			bool assertConverterInstanceIsAnyNotNull = false,
			string stringFormat = null,
			TDest targetNullValue = default,
			TDest fallbackValue = default,
			Action<IMultiValueConverter> assertConvert = null
		)
		{
			var binding = BindingHelpers.GetMultiBinding(bindable, targetProperty);
			Assert.That(binding, Is.Not.Null);
			Assert.That(binding.Bindings.SequenceEqual(bindings), Is.True);
			Assert.That(binding.Mode, Is.EqualTo(mode));
			if (assertConverterInstanceIsAnyNotNull)
				Assert.That(binding.Converter, Is.Not.Null);
			else
				Assert.That(binding.Converter, Is.EqualTo(converter));
			Assert.That(binding.ConverterParameter, Is.EqualTo(converterParameter));
			Assert.That(binding.StringFormat, Is.EqualTo(stringFormat));
			Assert.That(binding.TargetNullValue, Is.EqualTo(targetNullValue));
			Assert.That(binding.FallbackValue, Is.EqualTo(fallbackValue));

			assertConvert?.Invoke(binding.Converter);
		}

		internal static Binding GetBinding(BindableObject bindable, BindableProperty property) => GetBindingBase(bindable, property) as Binding;

		internal static MultiBinding GetMultiBinding(BindableObject bindable, BindableProperty property) => GetBindingBase(bindable, property) as MultiBinding;

		/// <remarks>
		/// Note that we are only testing whether the Markup helpers create the correct bindings,
		/// we are not testing the binding mechanism itself; this is why it is justified to access
		/// private binding API's here for testing.
		/// </remarks>
		internal static BindingBase GetBindingBase(BindableObject bindable, BindableProperty property)
		{
			// return bindable.GetContext(property)?.Binding as Binding;
			// Both BindableObject.GetContext and BindableObject.BindablePropertyContext are private; 
			// use reflection instead of above line.

			if (getContextMethodInfo == null)
				getContextMethodInfo = typeof(BindableObject).GetMethod("GetContext", BindingFlags.NonPublic | BindingFlags.Instance);

			var context = getContextMethodInfo?.Invoke(bindable, new object[] { property });
			if (context == null)
				return null;

			if (bindingFieldInfo == null)
				bindingFieldInfo = context?.GetType().GetField("Binding");

			return bindingFieldInfo?.GetValue(context) as BindingBase;
		}

		internal static IValueConverter AssertConvert<TValue, TConvertedValue>(this IValueConverter converter, TValue value, object parameter, TConvertedValue expectedConvertedValue, bool twoWay = false, bool backOnly = false, CultureInfo culture = null)
		{
			Assert.That(converter?.Convert(value, typeof(object), parameter, culture), Is.EqualTo(backOnly ? default(TConvertedValue) : expectedConvertedValue));
			Assert.That(converter?.ConvertBack(expectedConvertedValue, typeof(object), parameter, culture), Is.EqualTo(twoWay || backOnly ? value : default(TValue)));
			return converter;
		}

		internal static IValueConverter AssertConvert<TValue, TConvertedValue>(this IValueConverter converter, TValue value, TConvertedValue expectedConvertedValue, bool twoWay = false, bool backOnly = false, CultureInfo culture = null)
			=> AssertConvert(converter, value, null, expectedConvertedValue, twoWay: twoWay, backOnly: backOnly, culture: culture);

		internal static IMultiValueConverter AssertConvert<TConvertedValue>(this IMultiValueConverter converter, object[] values, object parameter, TConvertedValue expectedConvertedValue, bool twoWay = false, bool backOnly = false, CultureInfo culture = null)
		{
			Assert.That(converter?.Convert(values, typeof(TConvertedValue), parameter, culture), Is.EqualTo(backOnly ? BindableProperty.UnsetValue : expectedConvertedValue));
			var convertedBackValues = converter?.ConvertBack(expectedConvertedValue, null, parameter, culture);
			if (twoWay || backOnly)
				Assert.That(convertedBackValues.SequenceEqual(values), Is.True);
			else
				Assert.That(convertedBackValues, Is.Null);
			return converter;
		}

		internal static IMultiValueConverter AssertConvert<TConvertedValue>(this IMultiValueConverter converter, object[] values, TConvertedValue expectedConvertedValue, bool twoWay = false, bool backOnly = false, CultureInfo culture = null)
		=> AssertConvert<TConvertedValue>(converter, values, null, expectedConvertedValue, twoWay, backOnly, culture);
	}
}

namespace XamarinFormsMarkupUnitTestsBindableObjectViews
{
	using Xamarin.Forms;

	class DerivedFromLabel : Label { }
	class DerivedFromTextCell : TextCell { }
}