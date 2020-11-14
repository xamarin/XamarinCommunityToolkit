using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views.Options
{
	/// <summary>
	/// Use for any actions: toasts, SnackBars etc.
	/// </summary>
	public class ActionOptions
	{
		public ActionOptions()
		{
			MessageOptions = new MessageOptions();
			Duration = 3000;
			BackgroundColor = Color.Default;
			IsRtl = false;
			Result = new TaskCompletionSource<bool>(false);
		}

		public ActionOptions(MessageOptions messageOptions, int duration, Color backgroundColor, bool isRtl)
		{
			MessageOptions = messageOptions;
			Duration = duration;
			BackgroundColor = backgroundColor;
			IsRtl = isRtl;
			Result = new TaskCompletionSource<bool>(false);
		}

		/// <summary>
		/// Gets message options: Message, color, font
		/// </summary>
		public MessageOptions MessageOptions { get; }

		/// <summary>
		/// Gets background color.
		/// </summary>
		public Color BackgroundColor { get; }

		/// <summary>
		/// Is Right to left
		/// </summary>
		public bool IsRtl { get; }

		/// <summary>
		/// Result is true if ActionButton is clicked.
		/// </summary>
		public TaskCompletionSource<bool> Result { get; }

		/// <summary>
		/// Gets the duration for the SnackBar.
		/// </summary>
		public int Duration { get; }

		public void SetResult(bool result) => Result.TrySetResult(result);
	}
}