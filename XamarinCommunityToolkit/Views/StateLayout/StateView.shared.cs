using System;
using Xamarin.Forms;

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

		public static readonly BindableProperty RepeatTemplateProperty
			= BindableProperty.Create(nameof(RepeatTemplate), typeof(DataTemplate), typeof(StateView));

		public LayoutState StateKey
		{
			get => (LayoutState)GetValue(StateKeyProperty);
			set => SetValue(StateKeyProperty, value);
		}

		public string CustomStateKey
		{
			get => (string)GetValue(CustomStateKeyProperty);
			set => SetValue(CustomStateKeyProperty, value);
		}

		public int RepeatCount
		{
			get => (int)GetValue(RepeatCountProperty);
			set => SetValue(RepeatCountProperty, value);
		}

		public DataTemplate RepeatTemplate
		{
			get => (DataTemplate)GetValue(RepeatTemplateProperty);
			set => SetValue(RepeatTemplateProperty, value);
		}
	}
}