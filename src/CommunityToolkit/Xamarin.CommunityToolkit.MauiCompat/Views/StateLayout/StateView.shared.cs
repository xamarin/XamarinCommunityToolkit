using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class StateView : ContentView
	{
		public static readonly BindableProperty StateKeyProperty
			= BindableProperty.Create(nameof(StateKey), typeof(LayoutState), typeof(StateView), default(LayoutState));

		public static readonly BindableProperty CustomStateKeyProperty
			= BindableProperty.Create(nameof(CustomStateKey), typeof(string), typeof(StateView));

		public static readonly BindableProperty RepeatCountProperty
			= BindableProperty.Create(nameof(RepeatCount), typeof(int), typeof(StateView), 1);

		public static readonly BindableProperty TemplateProperty
			= BindableProperty.Create(nameof(Template), typeof(DataTemplate), typeof(StateView));

		public LayoutState StateKey
		{
			get => (LayoutState)GetValue(StateKeyProperty);
			set => SetValue(StateKeyProperty, value);
		}

		public string? CustomStateKey
		{
			get => (string?)GetValue(CustomStateKeyProperty);
			set => SetValue(CustomStateKeyProperty, value);
		}

		public int RepeatCount
		{
			get => (int)GetValue(RepeatCountProperty);
			set => SetValue(RepeatCountProperty, value);
		}

		public DataTemplate? Template
		{
			get => (DataTemplate?)GetValue(TemplateProperty);
			set => SetValue(TemplateProperty, value);
		}
	}
}