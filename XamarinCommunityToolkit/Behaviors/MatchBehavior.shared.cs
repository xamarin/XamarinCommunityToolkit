using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace Microsoft.Toolkit.Xamarin.Forms.Behaviors
{
    public class MatchBehavior : BaseBehavior<Label>
    {
        public static readonly BindableProperty CommandProperty 
            = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(MatchBehavior));

        public static readonly BindableProperty MatchTypesProperty 
            = BindableProperty.Create(nameof(MatchTypes), typeof(IEnumerable<MatchType>), typeof(MatchBehavior), Enumerable.Empty<MatchType>());

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public IEnumerable<MatchType> MatchTypes
        {
            get => (IEnumerable<MatchType>)GetValue(MatchTypesProperty);
            set => SetValue(MatchTypesProperty, value);
        }

        protected override void OnAttachedTo(Label bindable)
        {
            base.OnAttachedTo(bindable);
            DetectAndStyleMatches();
        }

        protected override void OnViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewPropertyChanged(sender, e);
            if (e.PropertyName == Label.FormattedTextProperty.PropertyName)
                DetectAndStyleMatches();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == CommandProperty.PropertyName && Command != null)
                ConfigureGestures(View, AddGestureRecognizer);
            else if (propertyName == MatchTypesProperty.PropertyName)
                DetectAndStyleMatches();
        }

        protected override void OnDetachingFrom(Label bindable)
        {
            ConfigureGestures(bindable, RemoveGestureRecognizer);
            base.OnDetachingFrom(bindable);
        }

        void DetectAndStyleMatches()
        {
            if (!string.IsNullOrWhiteSpace(View?.FormattedText?.ToString()))
            {
                var textValue = View.FormattedText?.ToString();
                View.FormattedText.Spans.Clear();
                var formatted = View.FormattedText;

                var collection = MatchTypes.SelectMany(c => c.Regex.Value.Matches(textValue).OfType<Match>().Where(c => c.Success)).OrderBy(x => x.Index);

                var lastIndex = 0;

                foreach (var item in collection)
                {
                    var text = textValue.Substring(lastIndex, item.Index - lastIndex);
                    formatted.Spans.Add(CreateSpan(text));
                    lastIndex = item.Index + item.Length;

                    var span = CreateSpan(item.Value, true);

                    formatted.Spans.Add(span);

                    if (Command != null)
                        AddGestureRecognizer(span);
                }

                var remainingText = textValue.Substring(lastIndex);

                formatted.Spans.Add(CreateSpan(remainingText));
            }
        }


        void ConfigureGestures(Label label, Action<Span> configAction)
        {
            if (label.FormattedText?.Spans.Any() ?? false)
            {
                var matchSpans = label.FormattedText.Spans.Where(p => MatchTypes.Any(c => c.Regex.Value.Match(p.Text).Success));
                foreach (var span in matchSpans)
                    configAction(span);
            }
        }

        Span CreateSpan(string text, bool isMatch = false)
         => new Span()
         {
             Text = text,
             Style = isMatch
                 ? MatchTypes.FirstOrDefault(c => c.Regex.Value.Match(text).Success)?.Style
                 : null
         };

        void AddGestureRecognizer(Span span)
        {
            var gesture = span.GestureRecognizers.FirstOrDefault();
            if (gesture is TapGestureRecognizer tapRecognizer)
                tapRecognizer.Command = Command;
            else
                span.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = Command,
                    CommandParameter = span.Text
                });
        }

        void RemoveGestureRecognizer(Span span)
        {
            var gesture = span.GestureRecognizers.FirstOrDefault();
            if (gesture is TapGestureRecognizer tapRecognizer)
                span.GestureRecognizers.Remove(tapRecognizer);
        }
    }
}
