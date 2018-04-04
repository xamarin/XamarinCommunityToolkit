using System;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using System.ComponentModel;
using RoutingEffects = Xamarin.Toolkit.Effects;
using PlatformEffects = Xamarin.Toolkit.Effects.UWP;

[assembly: ExportEffect(typeof(PlatformEffects.SearchBarSuggestion), nameof(RoutingEffects.SearchBarSuggestionEffect))]
namespace Xamarin.Toolkit.Effects.UWP
{
    public class SearchBarSuggestion : PlatformEffect
    {
        protected override void OnAttached()
        {
            var autoSuggestBox = Control as AutoSuggestBox;
            if (autoSuggestBox != null)
            {
                autoSuggestBox.SuggestionChosen += OnSuggestionChosen;
                autoSuggestBox.TextChanged += OnTextChangedEffect;
                autoSuggestBox.AutoMaximizeSuggestionArea = true;
                autoSuggestBox.ItemsSource = RoutingEffects.SearchBarSuggestion.GetSuggestions(Element);
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
            Action platformSpecificAction = (Action)Element.GetValue(RoutingEffects.SearchBarSuggestion.TextChangedActionProperty);
            platformSpecificAction?.Invoke();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == RoutingEffects.SearchBarSuggestion.SuggestionsProperty.PropertyName)
                UpdateItemsSource();
        }

        private void UpdateItemsSource()
        {
            ((AutoSuggestBox)Control).ItemsSource = RoutingEffects.SearchBarSuggestion.GetSuggestions(Element);
        }
    }
}