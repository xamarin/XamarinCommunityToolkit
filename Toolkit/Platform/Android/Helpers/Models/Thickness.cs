namespace Xamarin.Toolkit.Droid.Helpers.Models
{
    /// <summary>
    /// Describes the thickness of a frame around a rectangle. Four Double values describe the Left, Top, Right, and Bottom sides of the rectangle, respectively.
    /// </summary>
    public struct Thickness
    {
        /// <summary>
        /// Constructor for <see cref="Thickness"/>
        /// </summary>
        /// <param name="uniform">Uniform thickness</param>
        public Thickness(int uniform)
        {
            Left = uniform;
            Top = uniform;
            Right = uniform;
            Bottom = uniform;
        }

        /// <summary>
        /// Constructor for <see cref="Thickness"/>
        /// </summary>
        /// <param name="left">The left edge measure of the Thickness.</param>
        /// <param name="top">The top edge measure of the Thickness.</param>
        /// <param name="right">The right edge measure of the Thickness.</param>
        /// <param name="bottom">The bottom edge measure of the Thickness.</param>
        public Thickness(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// The left edge measure of the Thickness.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// The top edge measure of the Thickness.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// The right edge measure of the Thickness.
        /// </summary>
        public int Right { get; set; }

        /// <summary>
        /// The bottom edge measure of the Thickness.
        /// </summary>
        public int Bottom { get; set; }
    }
}
