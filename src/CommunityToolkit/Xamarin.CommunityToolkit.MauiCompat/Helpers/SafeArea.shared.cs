using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Helpers
{
	[System.ComponentModel.TypeConverter(typeof(SafeAreaTypeConverter))]
	public struct SafeArea
	{
		readonly bool isParameterized;

		public bool Left { get; }

		public bool Top { get; }

		public bool Right { get; }

		public bool Bottom { get; }

		public bool IsEmpty
			=> !Left && !Top && !Right && !Bottom;

		public SafeArea(bool uniformSafeArea)
			: this(uniformSafeArea, uniformSafeArea, uniformSafeArea, uniformSafeArea)
		{
		}

		public SafeArea(bool horizontal, bool vertical)
			: this(horizontal, vertical, horizontal, vertical)
		{
		}

		public SafeArea(bool left, bool top, bool right, bool bottom)
		{
			isParameterized = true;

			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public static implicit operator SafeArea(bool uniformSafeArea)
			=> new SafeArea(uniformSafeArea);

		bool Equals(SafeArea other)
			=> (!isParameterized &&
			!other.isParameterized) ||
			(Left == other.Left &&
			Top == other.Top &&
			Right == other.Right &&
			Bottom == other.Bottom);

		public override bool Equals(object? obj)
			=> obj is not null
			&& obj is SafeArea safeArea
			&& Equals(safeArea);

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Left.GetHashCode();
				hashCode = (hashCode * 397) ^ Top.GetHashCode();
				hashCode = (hashCode * 397) ^ Right.GetHashCode();
				hashCode = (hashCode * 397) ^ Bottom.GetHashCode();
				return hashCode;
			}
		}

		public static bool operator ==(SafeArea left, SafeArea right)
			=> left.Equals(right);

		public static bool operator !=(SafeArea left, SafeArea right)
			=> !left.Equals(right);

		public void Deconstruct(out bool left, out bool top, out bool right, out bool bottom)
		{
			left = Left;
			top = Top;
			right = Right;
			bottom = Bottom;
		}
	}
}