using System.Collections.Generic;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.CommunityToolkit.Sample.Models;
using Xamarin.CommunityToolkit.Sample.Pages.Effects;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Effects
{
	public class EffectsGalleryViewModel : BaseGalleryViewModel
	{
		protected override IEnumerable<SectionModel> CreateItems() => new[]
		{
			new SectionModel(
				typeof(SafeAreaEffectPage),
				nameof(SafeAreaEffect),
				"The SafeAreaEffect is an effect that will help to make sure that content isn't clipped by rounded device corners, the home indicator, or the sensor housing on an iPhone X (or alike)"),

			new SectionModel(
				typeof(RemoveBorderEffectPage),
				nameof(RemoveBorderEffect),
				"The RemoveBorderEffect is an effect that will remove the border from an Entry on iOS and the underline from an entry on Android"),

			new SectionModel(
				typeof(SelectAllTextEffectPage),
				nameof(SelectAllTextEffect),
				"The SelectAllTextEffect is an effect that will select all text in an Entry / Editor when it becomes focussed"),

			new SectionModel(
				typeof(IconTintColorEffectPage),
				nameof(IconTintColorEffect),
				"With the IconTintColorEffect you set the tint color of an Image or ImageButton."),

			new SectionModel(
				typeof(TouchEffectPage),
				nameof(TouchEffect),
				"The TouchEffect is an effect that allows changing the view's appearance depending on the touch state (normal, pressed, hovered). Also, it allows to handle long presses."),

			new SectionModel(
				typeof(LifeCycleEffectPage),
				nameof(LifecycleEffect),
				"The LifeCycle is an effect that allows you to know when a control or layout is loaded or/and unloaded in the screen and perform actions based on that."),

			new SectionModel(
				typeof(ShadowEffectPage),
				nameof(ShadowEffect),
				"The ShadowEffect allows all views to display shadow."),

			new SectionModel(
				typeof(SemanticEffectPage),
				nameof(SemanticEffect),
				"The SemanticEffect allows you to set semantic properties for accessibility."),

			new SectionModel(
				typeof(CornerRadiusEffectPage),
				nameof(CornerRadiusEffect),
				"The CornerRadius allows rounded corners everywhere."),
        
      new SectionModel(
				typeof(StatusBarEffectPage),
				nameof(StatusBarEffect),
				"The StatusBar allows to change status bar color and style. This page also demonstrates Android only NavigationBar changes.")
		};
	}
}