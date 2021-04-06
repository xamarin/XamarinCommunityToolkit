using System;
using System.Globalization;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Converters
{
	/// <summary>
	/// Converts an object that implements IComparable to an object or a boolean based on a comparaison.
	/// </summary>
	public class CompareConverter : CompareConverter<object>
	{
	}

	/// <summary>
	/// Converts an object that implements IComparable to an object or a boolean based on a comparaison.
	/// </summary>
	public class CompareConverter<TObject> : ValueConverterExtension, IValueConverter
	{
		[Flags]
		public enum Operator
		{
			Greater = 2,
			GreaterOrEqual = 1,
			Equal = 0,
			SmallerOrEqual = -1,
			Smaller = -2,
			NotEqual = -3,
		}

		enum Modes
		{
			Boolean,
			Object
		}

		Modes mode;

		/// <summary>
		/// The comparing value.
		/// </summary>
		public IComparable? ComparingValue { get; set; }

		/// <summary>
		/// The comparison operator.
		/// </summary>
		public Operator ComparisonOperator { get; set; }

		/// <summary>
		/// The object that corresponds to True value.
		/// </summary>
		public TObject? TrueObject { get; set; }

		/// <summary>
		/// The object that corresponds to False value.
		/// </summary>
		public TObject? FalseObject { get; set; }

		/// <summary>
		/// Converts an object that implements IComparable to a specified object or a boolean based on a comparaison result.
		/// </summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="targetType">The type of the binding target property. This is not implemented.</param>
		/// <param name="parameter">Additional parameter for the converter to handle. This is not implemented.</param>
		/// <param name="culture">The culture to use in the converter.  This is not implemented.</param>
		/// <returns>The object assigned to <see cref="TrueObject"/> if (value <see cref="ComparisonOperator"/> <see cref="ComparingValue"/>) equals True and <see cref="TrueObject"/> is not null, if <see cref="TrueObject"/> is null it returns true, otherwise the value assigned to <see cref="FalseObject"/>, if no value is assigned then it returns false.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (ComparingValue == null)
			{
				throw new ArgumentNullException(nameof(ComparingValue), $"{nameof(ComparingValue)} and {nameof(ComparisonOperator)} parameters shouldn't be null");
			}

			if (value is not IComparable)
			{
				throw new ArgumentException("is expected to implement IComparable interface.", nameof(value));
			}

			if (!Enum.IsDefined(typeof(Operator), ComparisonOperator))
			{
				throw new ArgumentOutOfRangeException($"is expected to be of type {nameof(Operator)}", nameof(ComparisonOperator));
			}

			if (!(TrueObject == null ^ FalseObject != null))
			{
				throw new ArgumentNullException(nameof(TrueObject), $"{nameof(TrueObject)} and {nameof(FalseObject)} should be either defined both or omitted both.");
			}

			if (TrueObject != null)
			{
				mode = Modes.Object;
			}

			var valueIComparable = (IComparable)value;
			var result = valueIComparable.CompareTo(ComparingValue);

			return ComparisonOperator switch
			{
				Operator.Smaller => EvaluateCondition(result < 0),
				Operator.SmallerOrEqual => EvaluateCondition(result <= 0),
				Operator.Equal => EvaluateCondition(result == 0),
				Operator.NotEqual => EvaluateCondition(result != 0),
				Operator.GreaterOrEqual => EvaluateCondition(result >= 0),
				Operator.Greater => EvaluateCondition(result > 0),
				_ => throw new ArgumentNullException(nameof(ComparisonOperator), $"\"{ComparisonOperator}\" is not supported."),
			};
		}

		object EvaluateCondition(bool comparaisonResult)
		{
			if (comparaisonResult)
			{
				if (mode == Modes.Object)
				{
					return TrueObject!;
				}
				else
				{
					return true;
				}
			}
			else if (mode == Modes.Object)
			{
				return FalseObject!;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// This method is not implemented and will throw a <see cref="NotImplementedException"/>.
		/// </summary>
		/// <param name="value">N/A</param>
		/// <param name="targetType">N/A</param>
		/// <param name="parameter">N/A</param>
		/// <param name="culture">N/A</param>
		/// <returns>N/A</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}
}
