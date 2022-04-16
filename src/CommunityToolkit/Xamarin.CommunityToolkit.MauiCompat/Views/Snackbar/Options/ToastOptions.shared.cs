using System;using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views.Options
{
	/// <summary>
	/// Toast options
	/// </summary>
	public class ToastOptions
	{
		public ToastOptions() => Result = new TaskCompletionSource<bool>(false);

		/// <summary>
		/// Message options: Message, color, font
		/// </summary>
		public MessageOptions MessageOptions { get; set; } = DefaultMessageOptions;

		public static MessageOptions DefaultMessageOptions { get; set; } = new();

		/// <summary>
		/// Background color.
		/// </summary>
		public Color BackgroundColor { get; set; } = DefaultBackgroundColor;

		public static Color DefaultBackgroundColor { get; set; } = default(Microsoft.Maui.Graphics.Color);

		/// <summary>
		/// Corner radius (in dp)
		/// </summary>
		public Thickness CornerRadius { get; set; } = DefaultCornerRadius;

		public static Thickness DefaultCornerRadius { get; set; } = new(4, 4, 4, 4);

		/// <summary>
		/// Is Right to left
		/// </summary>
		public bool IsRtl { get; set; } = DefaultIsRtl;

		public static bool DefaultIsRtl { get; set; } = false;

		/// <summary>
		/// The duration for the SnackBar.
		/// </summary>
		public TimeSpan Duration { get; set; } = DefaultDuration;

		public static TimeSpan DefaultDuration { get; set; } = TimeSpan.FromMilliseconds(3000);

		/// <summary>
		/// Result is true if ActionButton is clicked.
		/// </summary>
		public TaskCompletionSource<bool> Result { get; }

		internal bool SetResult(bool result) => Result.TrySetResult(result);

		internal bool SetException(Exception exception) => Result.TrySetException(exception);
	}
}