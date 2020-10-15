using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views.Options
{
	public class SnackBarOptions : ActionOptions
	{
		public SnackBarOptions(MessageOptions message, int duration, Color backgroundColor, bool isRtl, IEnumerable<SnackBarActionOptions> actions)
			: base(message, duration, backgroundColor, isRtl) =>
			Actions = actions ?? Enumerable.Empty<SnackBarActionOptions>();

		public SnackBarOptions() => Actions = Enumerable.Empty<SnackBarActionOptions>();

		/// <summary>
		///     Gets the text for the action buttons
		/// </summary>
		public IEnumerable<SnackBarActionOptions> Actions { get; }
	}
}