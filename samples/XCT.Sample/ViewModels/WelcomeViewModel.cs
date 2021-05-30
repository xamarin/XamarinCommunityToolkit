using System.Collections.Generic;
using CommunityToolkit.Maui.Sample.Models;
using CommunityToolkit.Maui.Sample.Pages.Behaviors;
using CommunityToolkit.Maui.Sample.Pages.Converters;
using CommunityToolkit.Maui.Sample.Pages.Effects;
using CommunityToolkit.Maui.Sample.Pages.Extensions;
using CommunityToolkit.Maui.Sample.Pages.TestCases;
using CommunityToolkit.Maui.Sample.Pages.Views;
using Xamarin.Forms;

namespace CommunityToolkit.Maui.Sample.ViewModels
{
	public class WelcomeViewModel : BaseGalleryViewModel
	{
		protected override IEnumerable<SectionModel> CreateItems() => new[]
		{
			new SectionModel(typeof(BehaviorsGalleryPage), "Behaviors", Color.FromHex("#8E8CD8"),
				"Behaviors lets you add functionality to user interface controls without having to subclass them. Behaviors are written in code and added to controls in XAML or code"),

			new SectionModel(typeof(ConvertersGalleryPage), "Converters", Color.FromHex("#EA005E"),
				"Converters let you convert bindings of a certain type to a different value, based on custom logic"),

			new SectionModel(typeof(EffectsGalleryPage), "Effects", Color.FromHex("#EFB950"),
				"Effects are used to add visual customization on a control by control bases"),

			new SectionModel(typeof(ExtensionsGalleryPage), "Extensions", Color.FromHex("#00CC6A"),
				"Extensions are used to supplement existing functionalities by making them easier to use"),

			new SectionModel(typeof(TestCasesGalleryPage), "Test Cases", Color.FromHex("#FF8C00"),
				"Testing is important, ok?! So this is where all of the tests for our little project reside"),

			new SectionModel(typeof(ViewsGalleryPage), "Views", Color.FromHex("#EF6950"),
				"A custom view or control allows for adding custom functionality as if it came out of the Xamarin.Forms box"),

			new SectionModel(typeof(SearchPage), "C# Markup", Color.FromHex("#50D6EF"),
				"C# Markup Extensions make it easier on the eyes to write your layouts in C# code instead of XAML")
		};
	}
}