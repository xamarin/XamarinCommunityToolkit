using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace FormsCommunityToolkit.Effects
{
    public static class SearchBarSuggestionEffect
    {
        public static readonly BindableProperty SuggestionsProperty = BindableProperty.CreateAttached("Suggestions", typeof(ObservableCollection<string>), typeof(SearchBarSuggestionEffect), new ObservableCollection<string>(), propertyChanged: OnSuggestionsChanged);
        public static readonly BindableProperty TextChangedActionProperty = BindableProperty.CreateAttached("TextChangedAction", typeof(Action), typeof(SearchBarSuggestionEffect), null, propertyChanged: OnTextChangedActionChanged);

        public static ObservableCollection<string> GetSuggestions(BindableObject view)
        {
            return (ObservableCollection<string>)view.GetValue(SuggestionsProperty);
        }
        
        public static void SetSuggestions(BindableObject view, ObservableCollection<string> value)
        {
            view.SetValue(SuggestionsProperty, value);
        }

        public static Action GetTextChangedAction(BindableObject view)
        {
            return (Action)view.GetValue(TextChangedActionProperty);
        }

        public static void SetTextChangedAction(BindableObject view, Action value)
        {
            view.SetValue(TextChangedActionProperty, value);
        }

        private static void OnSuggestionsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as SearchBar;
            if (view == null)
                return;

            bindable.SetValue(SuggestionsProperty, (ObservableCollection<string>)newValue);
        }
        private static void OnTextChangedActionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as SearchBar;
            if (view == null)
                return;

            bindable.SetValue(TextChangedActionProperty, (Action)newValue);
        }
    }

    public class UWPSearchBarSuggestionEffect : RoutingEffect
    {
        public UWPSearchBarSuggestionEffect() : base("FormsCommunityToolkit.Effects.UWPSearchBarSuggestionEffect")
        {
        }
    }
}
