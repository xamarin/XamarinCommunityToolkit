using System;using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public static class StateLayout
	{
		internal static readonly BindablePropertyKey StateViewsPropertyKey
			= BindableProperty.CreateReadOnly("StateViews", typeof(IList<StateView>), typeof(Layout<View>), default(IList<StateView>),
				defaultValueCreator: bindable =>
				{
					var collection = new List<StateView>();
					return collection;
				});

		public static readonly BindableProperty StateViewsProperty
			= StateViewsPropertyKey.BindableProperty;

		internal static readonly BindableProperty LayoutControllerProperty
			= BindableProperty.CreateAttached("LayoutController", typeof(StateLayoutController), typeof(Layout<View>), default(StateLayoutController),
				defaultValueCreator: LayoutControllerCreator);

		internal static StateLayoutController? GetLayoutController(BindableObject b)
			=> (StateLayoutController?)b.GetValue(LayoutControllerProperty);

		public static readonly BindableProperty CurrentStateProperty
			= BindableProperty.CreateAttached("CurrentState", typeof(LayoutState), typeof(Layout<View>), default(LayoutState),
				propertyChanged: (b, o, n) => OnCurrentStateChanged(b, (LayoutState)o, (LayoutState)n));

		public static readonly BindableProperty CurrentCustomStateKeyProperty
			= BindableProperty.CreateAttached("CurrentCustomStateKey", typeof(string), typeof(Layout<View>), default(string),
				propertyChanged: (b, o, n) => OnCurrentCustomStateKeyChanged(b, (string)o, (string)n));

		public static readonly BindableProperty AnimateStateChangesProperty
			= BindableProperty.CreateAttached("AnimateStateChanges", typeof(bool), typeof(Layout<View>), true,
				propertyChanged: (b, o, n) => ((BindableObject)b).SetValue(AnimateStateChangesProperty, n));

		public static IList<StateView>? GetStateViews(BindableObject b)
			=> (IList<StateView>?)b.GetValue(StateViewsProperty);

		public static void SetCurrentState(BindableObject b, LayoutState value)
			=> b.SetValue(CurrentStateProperty, value);

		public static LayoutState GetCurrentState(BindableObject b)
			=> (LayoutState)b.GetValue(CurrentStateProperty);

		public static void SetCurrentCustomStateKey(BindableObject b, string value)
			=> b.SetValue(CurrentCustomStateKeyProperty, value);

		public static string? GetCurrentCustomStateKey(BindableObject b)
			=> (string?)b.GetValue(CurrentCustomStateKeyProperty);

		public static void SetAnimateStateChanges(BindableObject b, bool value)
			=> b.SetValue(AnimateStateChangesProperty, value);

		public static bool GetAnimateStateChanges(BindableObject b)
			=> (bool?)b.GetValue(AnimateStateChangesProperty) ?? false;

		static void OnCurrentStateChanged(BindableObject bindable, LayoutState oldValue, LayoutState newValue)
		{
			if (oldValue == newValue)
				return;

			// Swap out the current children for the Loading Template.
			switch (newValue)
			{
				case LayoutState.Custom:
					break;
				case LayoutState.None:
					GetLayoutController(bindable)?.SwitchToContent(GetAnimateStateChanges(bindable));
					break;
				default:
					GetLayoutController(bindable)?.SwitchToTemplate(newValue, null, GetAnimateStateChanges(bindable));
					break;
			}
		}

		static void OnCurrentCustomStateKeyChanged(BindableObject bindable, string oldValue, string newValue)
		{
			if (oldValue == newValue)
				return;

			var state = GetCurrentState(bindable);

			// Swap out the current children for the Loading Template.
			switch (state)
			{
				case LayoutState.None:
					GetLayoutController(bindable)?.SwitchToContent(GetAnimateStateChanges(bindable));
					break;
				case LayoutState.Custom:
					GetLayoutController(bindable)?.SwitchToTemplate(newValue, GetAnimateStateChanges(bindable));
					break;
				default:
					break;
			}
		}

		static object LayoutControllerCreator(BindableObject bindable)
		{
			if (bindable is Layout<View> layoutView)
			{
				return new StateLayoutController(layoutView)
				{
					StateViews = GetStateViews(layoutView) ?? new List<StateView>()
				};
			}

			throw new InvalidOperationException($"Cannot create the StateLayoutController. The specified view '{bindable.GetType().FullName}' does not inherit Layout<View>.");
		}
	}
}