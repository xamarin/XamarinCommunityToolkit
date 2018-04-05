using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Xamarin.Toolkit.Effects
{
    public static class SearchBarSuggestion
    {
        public static readonly BindableProperty SuggestionsProperty = BindableProperty.CreateAttached("Suggestions", typeof(ObservableCollection<string>), typeof(SearchBarSuggestion), new ObservableCollection<string>(), propertyChanged: OnSuggestionsChanged);
        public static readonly BindableProperty TextChangedActionProperty = BindableProperty.CreateAttached("TextChangedAction", typeof(Action), typeof(SearchBarSuggestion), null, propertyChanged: OnTextChangedActionChanged);

        public static ObservableCollection<string> GetSuggestions(BindableObject view)
        {
            return (ObservableCollection<string>)view.GetValue(SuggestionsProperty);
        }

        public static void SetSuggestions(BindableObject view, ObservableCollection<string> value) =>
            view.SetValue(SuggestionsProperty, value);

        public static Action GetTextChangedAction(BindableObject view) =>
            (Action)view.GetValue(TextChangedActionProperty);

        public static void SetTextChangedAction(BindableObject view, Action value) =>
            view.SetValue(TextChangedActionProperty, value);

        static void OnSuggestionsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as SearchBar;
            if (view == null)
                return;

            bindable.SetValue(SuggestionsProperty, (ObservableCollection<string>)newValue);
        }

        static void OnTextChangedActionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as SearchBar;
            if (view == null)
                return;

            bindable.SetValue(TextChangedActionProperty, (Action)newValue);
        }
    }

    public class SearchBarSuggestionEffect : RoutingEffect
    {
        public SearchBarSuggestionEffect()
            : base(EffectIds.SearchBarSuggestion)
        {
        }
    }
}
