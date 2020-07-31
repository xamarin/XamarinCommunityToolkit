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
    public class TagBehavior : BaseBehavior<Label>
    {
        string RegexPattern => $@"[{string.Join(string.Empty, TagTypes.Select(s => s.Symbol))}]\w+";

        public static readonly BindableProperty CommandProperty =
         BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TagBehavior));

        public static readonly BindableProperty TagTypesProperty =
         BindableProperty.Create(nameof(TagTypes), typeof(IList<TagType>), typeof(TagBehavior), new List<TagType>());

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public IList<TagType> TagTypes
        {
            get => (IList<TagType>)GetValue(TagTypesProperty);
            set => SetValue(TagTypesProperty, value);
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

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == CommandProperty.PropertyName && Command != null)
                ConfigureTags(View, AddGestureRecognizer);
            else if (propertyName == TagTypesProperty.PropertyName)
                DetectAndStyleTags();
        }

        protected override void OnDetachingFrom(Label bindable)
        {
            base.OnDetachingFrom(bindable);

            ConfigureTags(bindable, RemoveGestureRecognizer);
        }

        void DetectAndStyleTags()
        {
            if (!string.IsNullOrWhiteSpace(View.FormattedText?.ToString()))
            {
                var textValue = View.FormattedText?.ToString();
                View.FormattedText.Spans.Clear();
                var formatted = View.FormattedText;
                var collection = Regex.Matches(textValue, RegexPattern, RegexOptions.Singleline);

                var lastIndex = 0;

                foreach (Match item in collection)
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

        void ConfigureTags(Label label, Action<Span> configAction)
        {
            if (label.FormattedText?.Spans.Any() ?? false)
            {
                var tagSpans = label.FormattedText.Spans.Where(p => Regex.Match(p.Text, RegexPattern).Success);
                foreach (var span in tagSpans)
                    configAction(span);
            }
        }

        Span CreateSpan(string text, bool isTag = false)
            => new Span()
            {
                Text = text,
                Style = isTag 
                    ? TagTypes.FirstOrDefault(c => text.Contains(c.Symbol))?.Style 
                    : null
            };

        void AddGestureRecognizer(Span span)
        {
            var tapRecognizer = span.GestureRecognizers.FirstOrDefault() as TapGestureRecognizer;
            if (tapRecognizer != null)
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
            var tapRecognizer = span.GestureRecognizers.FirstOrDefault() as TapGestureRecognizer;
            if (tapRecognizer != null)
                span.GestureRecognizers.Remove(tapRecognizer);
        }
    }
}
