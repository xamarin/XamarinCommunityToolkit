using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Xamarin.CommunityToolkit.Core
{
	internal enum MathOperatorPriority
	{
		Lowest,
		Low,
		Medium,
		High,
		Constant,
	}

	internal class MathOperator
	{
		public string Name { get; }
		public int NumericCount { get; }
		public MathOperatorPriority Priority { get; }
		public Func<double[], double> CalculateFunc { get; }

		public MathOperator(
			string name,
			int numericCount,
			MathOperatorPriority priority,
			Func<double[], double> calculateFunc)
		{
			Name = name;
			CalculateFunc = calculateFunc;
			Priority = priority;
			NumericCount = numericCount;
		}
	}

	internal class MathExpression
	{
		const string regexPattern = @"\d+\.\d+|\d+|\+|\-|\/|\*|\(|\)|\^|\w+";

		public string Expression { get; }

		readonly IList<MathOperator> operators;

		public MathExpression(string expression)
		{
			if (string.IsNullOrEmpty(expression))
				throw new ArgumentNullException(nameof(expression), "Expression can't be null or empty");

			Expression = expression.ToLower();

			operators = new List<MathOperator>
			{
				new ("+", 2, MathOperatorPriority.Low, x => x[0] + x[1]),
				new ("-", 2, MathOperatorPriority.Low, x => x[0] - x[1]),
				new ("*", 2, MathOperatorPriority.Medium, x => x[0] * x[1]),
				new ("/", 2, MathOperatorPriority.Medium, x => x[0] / x[1]),
				new ("%", 2, MathOperatorPriority.Medium, x => x[0] % x[1]),
				new ("abs", 1, MathOperatorPriority.Medium, x => Math.Abs(x[0])),
				new ("acos", 1, MathOperatorPriority.Medium, x => Math.Acos(x[0])),
				new ("asin", 1, MathOperatorPriority.Medium, x => Math.Asin(x[0])),
				new ("atan", 1, MathOperatorPriority.Medium, x => Math.Atan(x[0])),
				new ("atan2", 2, MathOperatorPriority.Medium, x => Math.Atan2(x[0], x[1])),
				new ("ceiling", 1, MathOperatorPriority.Medium, x => Math.Ceiling(x[0])),
				new ("cos", 1, MathOperatorPriority.Medium, x => Math.Cos(x[0])),
				new ("cosh", 1, MathOperatorPriority.Medium, x => Math.Cosh(x[0])),
				new ("exp", 1, MathOperatorPriority.Medium, x => Math.Exp(x[0])),
				new ("floor", 1, MathOperatorPriority.Medium, x => Math.Floor(x[0])),
				new ("ieeeremainder", 2, MathOperatorPriority.Medium, x => Math.IEEERemainder(x[0], x[1])),
				new ("log", 2, MathOperatorPriority.Medium, x => Math.Log(x[0], x[1])),
				new ("log10", 1, MathOperatorPriority.Medium, x => Math.Log10(x[0])),
				new ("max", 2, MathOperatorPriority.Medium, x => Math.Max(x[0], x[1])),
				new ("min", 2, MathOperatorPriority.Medium, x => Math.Min(x[0], x[1])),
				new ("pow", 2, MathOperatorPriority.Medium, x => Math.Pow(x[0], x[1])),
				new ("round", 2, MathOperatorPriority.Medium, x => Math.Round(x[0], Convert.ToInt32(x[0]))),
				new ("sign", 1, MathOperatorPriority.Medium, x => Math.Sign(x[0])),
				new ("sin", 1, MathOperatorPriority.Medium, x => Math.Sin(x[0])),
				new ("sinh", 1, MathOperatorPriority.Medium, x => Math.Sinh(x[0])),
				new ("sqrt", 1, MathOperatorPriority.Medium, x => Math.Sqrt(x[0])),
				new ("tan", 1, MathOperatorPriority.Medium, x => Math.Tan(x[0])),
				new ("tanh", 1, MathOperatorPriority.Medium, x => Math.Tanh(x[0])),
				new ("truncate", 1, MathOperatorPriority.Medium, x => Math.Truncate(x[0])),
				new ("^", 2, MathOperatorPriority.High, x => Math.Pow(x[0], x[1])),
				new ("pi", 0, MathOperatorPriority.Constant, _ => Math.PI),
				new ("e", 0, MathOperatorPriority.Constant, _ => Math.E),
			};
		}

		public double Calculate()
		{
			var rpn = GetReversePolishNotation(Expression);

			return 2d;
		}

		IEnumerable<string> GetReversePolishNotation(string expression)
		{
			var regex = new Regex(regexPattern);
			var matches = regex.Matches(expression);

			if (matches == null)
				throw new ArgumentException("Invalid math expression");

			var output = new List<string>();
			var stack = new Stack<(string Name, MathOperatorPriority Priority)>();

			foreach (Match match in matches)
			{
				if (string.IsNullOrEmpty(match.Value))
					continue;

				var value = match.Value;

				if (double.TryParse(value, out _))
				{
					output.Add(value);
					continue;
				}

				var @operator = operators.FirstOrDefault(x => x.Name == value);
				if (@operator != null)
				{
					if (@operator.Priority == MathOperatorPriority.Constant)
					{
						output.Add(value);
						continue;
					}

					while (stack.Count > 0)
					{
						var stackValue = stack.Peek();
						if (stackValue.Priority >= @operator.Priority)
						{
							output.Add(stack.Pop().Name);
						}
						else
						{
							break;
						}
					}

					stack.Push((value, @operator.Priority));
				}
				else if (value == "(")
				{
					stack.Push((value, MathOperatorPriority.Lowest));
				}
				else if (value == ")")
				{
					for (var i = stack.Count - 1; i >= 0; i--)
					{
						if (stack.Count == 0)
							throw new ArgumentException("Invalid math expression");

						var stackValue = stack.Pop().Name;
						if (stackValue == "(")
							break;

						output.Add(stackValue);
					}
				}
			}

			for (var i = stack.Count - 1; i >= 0; i--)
			{
				output.Add(stack.Pop().Name);
			}

			return output;
		}
	}
}