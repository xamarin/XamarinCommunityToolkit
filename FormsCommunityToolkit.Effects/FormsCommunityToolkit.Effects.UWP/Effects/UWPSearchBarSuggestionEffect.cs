using FormsCommunityToolkit.Effects.UWP.Effects;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using System.ComponentModel;

[assembly: ExportEffect(typeof(UWPSearchBarSuggestionEffect), nameof(UWPSearchBarSuggestionEffect))]
namespace FormsCommunityToolkit.Effects.UWP.Effects
{
	public class UWPSearchBarSuggestionEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			var autoSuggestBox = Control as AutoSuggestBox;
			if (autoSuggestBox != null)
			{
				autoSuggestBox.SuggestionChosen += OnSuggestionChosen;
				autoSuggestBox.TextChanged += OnTextChangedEffect;
				autoSuggestBox.AutoMaximizeSuggestionArea = true;
				autoSuggestBox.ItemsSource = SearchBarSuggestionEffect.GetSuggestions(Element);
			}
		}

		protected override void OnDetached()
		{
			var autoSuggestBox = Control as AutoSuggestBox;
			autoSuggestBox.SuggestionChosen -= OnSuggestionChosen;
			autoSuggestBox.TextChanged -= OnTextChangedEffect;
		}

		private void OnSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
		{
			((IElementController)Element).SetValueFromRenderer(SearchBar.TextProperty, sender.Text);
		}

		private void OnTextChangedEffect(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs e)
		{
			Action platformSpecificAction = (Action)Element.GetValue(SearchBarSuggestionEffect.TextChangedActionProperty);
			platformSpecificAction?.Invoke();
		}

		protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
		{
			base.OnElementPropertyChanged(args);
			if (args.PropertyName == SearchBarSuggestionEffect.SuggestionsProperty.PropertyName)
				UpdateItemsSource();
		}

		private void UpdateItemsSource()
		{
			((AutoSuggestBox)Control).ItemsSource = SearchBarSuggestionEffect.GetSuggestions(Element);
		}
	}
}
