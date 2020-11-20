using Xamarin.Forms;

[assembly: ResolutionGroupName("Xamarin.CommunityToolkit.Effects")]

namespace Xamarin.CommunityToolkit.Effects
{
	sealed class EffectIds
	{
		/// <summary>
		/// The Base Resolution Group Name For Effects
		/// </summary>
		static string effectResolutionGroupName = $"{nameof(Xamarin)}.{nameof(CommunityToolkit)}.{nameof(Effects)}";

		/// <summary>
		/// Effect Id for <see cref="RemoveBorderEffect"/>
		/// </summary>
		public static string RemoveBorder => $"{effectResolutionGroupName}.{nameof(RemoveBorderEffect)}";

		/// <summary>
		/// Effect Id for <see cref="SelectAllTextEffect"/>
		/// </summary>
		public static string SelectAllText => $"{effectResolutionGroupName}.{nameof(SelectAllTextEffect)}";

		/// <summary>
		/// Effect Id for <see cref="SafeAreaEffect"/>
		/// </summary>
		public static string SafeArea => $"{effectResolutionGroupName}.{nameof(SafeAreaEffectRouter)}";

		/// <summary>
		/// Effect Id for <see cref="IconTintColorEffect"/>
		/// </summary>
		public static string IconTintColor => $"{effectResolutionGroupName}.{nameof(IconTintColorEffectRouter)}";

		/// <summary>
		/// Effect Id for <see cref="VisualFeedbackEffect"/>
		/// </summary>
		public static string VisualFeedback => $"{effectResolutionGroupName}.{nameof(VisualFeedbackEffect)}";
	}
}