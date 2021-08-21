using Xamarin.CommunityToolkit.PlatformConfiguration.AndroidSpecific;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

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

		/// <summary>
		/// Effect Id for <see cref="TouchEffect"/>
		/// </summary>
		public static string TouchEffect => $"{effectResolutionGroupName}.{nameof(TouchEffect)}";

		/// <summary>
		/// Effect Id for <see cref="LifeCycleEffect"/>
		/// </summary>
		public static string LifeCycleEffect => $"{effectResolutionGroupName}.{nameof(LifecycleEffect)}";

		/// <summary>
		/// Effect Id for <see cref="ShadowEffect"/>
		/// </summary>
		public static string ShadowEffect => $"{effectResolutionGroupName}.{nameof(ShadowEffect)}";

		/// <summary>
		/// Effect Id for <see cref="SemanticEffect"/>
		/// </summary>
		public static string Semantic => $"{effectResolutionGroupName}.{nameof(SemanticEffectRouter)}";

		/// Effect Id for <see cref="StatusBarEffect"/>
		/// </summary>
		public static string StatusBar => $"{effectResolutionGroupName}.{nameof(StatusBarEffect)}";

		/// <summary>
		/// Effect Id for <see cref="NavigationBarEffect"/>
		/// </summary>
		public static string NavigationBar => $"{effectResolutionGroupName}.{nameof(NavigationBarEffect)}";
	}
}