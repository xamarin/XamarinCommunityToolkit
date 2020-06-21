using System;
namespace XamarinCommunityToolkit.Helpers
{
    public struct SafeAreaPadding
    {
        readonly bool isParameterized;

        public bool Left { get; }
        public bool Top { get; }
        public bool Right { get; }
        public bool Bottom { get; }

        public bool IsEmpty
            => Left || Top || Right || Bottom;

        public SafeAreaPadding(bool uniformSafeAreaPadding) : this(uniformSafeAreaPadding, uniformSafeAreaPadding, uniformSafeAreaPadding, uniformSafeAreaPadding)
        {
        }

        public SafeAreaPadding(bool horizontal, bool vertical) : this(horizontal, vertical, horizontal, vertical)
        {
        }

        public SafeAreaPadding(bool left, bool top, bool right, bool bottom)
        {
            isParameterized = true;

            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public static implicit operator SafeAreaPadding(bool uniformSafeAreaPadding)
            => new SafeAreaPadding(uniformSafeAreaPadding);

        bool Equals(SafeAreaPadding other)
            => (!isParameterized &&
            !other.isParameterized) ||
            (Left == other.Left &&
            Top == other.Top &&
            Right == other.Right &&
            Bottom == other.Bottom);

        public override bool Equals(object obj)
            => !ReferenceEquals(null, obj) &&
            obj is SafeAreaPadding safeAreaPadding &&
            Equals(safeAreaPadding);

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

        public static bool operator ==(SafeAreaPadding left, SafeAreaPadding right)
            => left.Equals(right);

        public static bool operator !=(SafeAreaPadding left, SafeAreaPadding right)
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
