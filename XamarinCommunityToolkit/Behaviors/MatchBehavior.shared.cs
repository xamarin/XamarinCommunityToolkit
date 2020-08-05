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

        public static readonly BindableProperty CommandProperty =
         BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(MatchBehavior));

        public static readonly BindableProperty MatchTypesProperty =
         BindableProperty.Create(nameof(MatchTypes), typeof(IList<MatchType>), typeof(MatchBehavior), new List<MatchType>());

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public IList<MatchType> MatchTypes
        {
            get => (IList<MatchType>)GetValue(MatchTypesProperty);
            set => SetValue(MatchTypesProperty, value);
        }

        protected override void OnAttachedTo(Label bindable)
        {
            base.OnAttachedTo(bindable);
            DetectAndStyleTags();
        }

        protected override void OnViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewPropertyChanged(sender, e);

            if (e.PropertyName == Label.FormattedTextProperty.PropertyName)
                DetectAndStyleTags();
        }

        protected override void OnDetachingFrom(Label bindable)
        {
            base.OnDetachingFrom(bindable);

            MatchTypes = new List<MatchType>();

            // Remove tap gestures
            foreach (var span in bindable.FormattedText.Spans.Where(x => x.GestureRecognizers.Any()))
            {
                if (span.GestureRecognizers.FirstOrDefault() is TapGestureRecognizer tapRecognizer)
                    span.GestureRecognizers.Remove(tapRecognizer);
            }
        }

        void DetectAndStyleTags()
        {
            if (string.IsNullOrWhiteSpace(View.FormattedText?.ToString())) return;

            var textValue = View.FormattedText?.ToString();
            View.FormattedText.Spans.Clear();
            var formatted = View.FormattedText;

            var regexMatches = FindMatches(textValue);

            foreach (var span in GenerateSpans(textValue, regexMatches))
                formatted.Spans.Add(span);

        }

        /// <summary>
        /// Gets list of all matches from multiple Regexs and combines all results
        /// </summary>
        /// <param name="textValue"></param>
        IEnumerable<RegexMatches> FindMatches(string textValue)
        {
            IEnumerable<RegexMatches> regexMatches = new List<RegexMatches>();
            foreach (var matchType in MatchTypes)
            {
                var matches = Regex.Matches(textValue, matchType.Regex, RegexOptions.Singleline).OfType<Match>()
                    .Where(m => m.Success);

                regexMatches = regexMatches.Concat(matches.Select(x => new RegexMatches {Match = x, Type = matchType}))
                    .OrderBy(x => x.Match.Index);
            }

            return regexMatches;
        }

        /// <summary>
        /// Create spans and add tap gesture to matches
        /// </summary>
        /// <param name="textValue"></param>
        /// <param name="regexMatches"></param>
        List<Span> GenerateSpans(string textValue, IEnumerable<RegexMatches> regexMatches)
        {
            var sections = new List<Span>();
            var lastIndex = 0;
            foreach (var item in regexMatches)
            {
                // Gets text between to matches
                var sectionLength = item.Match.Index - lastIndex;
                sections.Add(new Span { Text = textValue.Substring(lastIndex, sectionLength) });
                lastIndex = item.Match.Index + item.Match.Length;

                // Get matched value
                var span = new Span
                {
                    Text = item.Type.GetValue(item.Match.Value),
                    Style = item.Type.Style
                };
                // TODO: Add longpress gesture to copy to clipboard
                // LongPressGestureRecognizer - https://github.com/xamarin/Xamarin.Forms/issues/3480
                // Clipboard - https://docs.microsoft.com/en-us/xamarin/essentials/clipboard?context=xamarin/android
                var tapGestureRecognizer = new TapGestureRecognizer
                {
                    Command = Command,
                    CommandParameter = item.Type.GetValue(item.Match.Value)
                };

                span.GestureRecognizers.Add(tapGestureRecognizer);
                sections.Add(span);
            }
            // Get end of text that isnt a match
            sections.Add(new Span { Text = textValue.Substring(lastIndex) });

            return sections;
        }
    }

    public class RegexMatches
    {
        public Match Match { get; set; }
        public MatchType Type { get; set; }
    }

    public abstract class MatchType
    {
        public abstract string Regex { get; }
        public Style Style { get; set; }
        public virtual string GetValue(string rawText) => rawText;
    }

    public class HashtagMatchType : MatchType
    {
        public override string Regex => $@"#\w+";
    }
    public class MentionMatchType : MatchType
    {
        public override string Regex => $@"[@]\w+";
    }
}
