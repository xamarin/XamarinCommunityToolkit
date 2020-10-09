using System.Collections.Generic;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views.Options
{
	public class SnackBarOptions : ActionOptions
	{
		public SnackBarOptions(MessageOptions message, int duration, Color backgroundColor, bool isRtl, IEnumerable<SnackBarActionOptions> actions)
			: base(message, duration, backgroundColor, isRtl) =>
			Actions = actions ?? new List<SnackBarActionOptions>();

		/// <summary>
		///     Gets the text for the action buttons
		/// </summary>
		public IEnumerable<SnackBarActionOptions> Actions { get; }
	}
}