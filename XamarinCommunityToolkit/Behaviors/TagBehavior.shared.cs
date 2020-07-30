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
        public IList<TagType> TagTypes { get; } = new List<TagType>();

        string RegexPattern => $@"[{string.Join(string.Empty, TagTypes.Select(s => s.Symbol))}]\w+";

        public static readonly BindableProperty CommandProperty =
         BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TagBehavior), null, defaultBindingMode: BindingMode.TwoWay);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void OnAttachedTo(Label bindable)
        {
            OnViewPropertyChanged(bindable, new PropertyChangedEventArgs(nameof(Label.FormattedText)));
            base.OnAttachedTo(bindable);
        }

        protected override void OnViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnViewPropertyChanged(sender, e);

            var label = sender as Label;

            if (e.PropertyName == Label.FormattedTextProperty.PropertyName && $"{label.FormattedText}" is string textValue && !string.IsNullOrEmpty(textValue))
            {
                label.FormattedText.Spans.Clear();
                var formatted = label.FormattedText;
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
                    {
                        AddGestureRecognizer(span);
                    }

                }

                var remainingText = textValue.Substring(lastIndex);

                formatted.Spans.Add(CreateSpan(remainingText));
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == CommandProperty.PropertyName && Command != null)
            {
                ConfigureTags(View, AddGestureRecognizer);
            }
        }

        protected override void OnDetachingFrom(Label bindable)
        {
            base.OnDetachingFrom(bindable);

            ConfigureTags(bindable, RemoveGestureRecognizer);
        }

        void ConfigureTags(Label label, Action<Span> configAction)
        {
            if (label.FormattedText != null && label.FormattedText.Spans.Any())
            {
                var tagSpans = label.FormattedText.Spans.Where(p => Regex.Match(p.Text, RegexPattern).Success);
                foreach (var span in tagSpans)
                {
                    configAction(span);
                }
            }
        }

        Span CreateSpan(string text, bool isTag = false)
        {
            var span = new Span()
            {
                Text = text,
            };

            if (isTag)
            {
                span.Style = TagTypes.FirstOrDefault(c => text.Contains(c.Symbol)).Style;
            }

            return span;
        }

        void AddGestureRecognizer(Span span)
        {
            var tapRecognizer = span.GestureRecognizers.FirstOrDefault() as TapGestureRecognizer;
            if (tapRecognizer != null)
            {
                tapRecognizer.Command = Command;
            }
            else
            {
                span.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    Command = Command,
                    CommandParameter = span.Text
                });
            }
        }

        void RemoveGestureRecognizer(Span span)
        {
            var tapRecognizer = span.GestureRecognizers.FirstOrDefault() as TapGestureRecognizer;
            if (tapRecognizer != null)
            {
                span.GestureRecognizers.Remove(tapRecognizer);
            }
        }
    }

    public class TagType
    {
        public string Symbol { get; set; } = "#";
        public Style Style { get; set; } = new Style(typeof(Span))
        {
            Class = "DefaultSpanStyle",
            Setters =
                {
                    new Setter
                    {
                        Property = Span.TextColorProperty,
                        Value = Color.Blue
                    }
                }
        };
    }
}
