using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UI.Views
{
	public class StateLayoutController
	{
		readonly WeakReference<Layout<View>> layoutWeakReference;
		bool layoutIsGrid;
		LayoutState previousState;
		IList<View> originalContent;

		public IList<StateView> StateViews { get; set; }

		public StateLayoutController(Layout<View> layout)
			=> layoutWeakReference = new WeakReference<Layout<View>>(layout);

		public async void SwitchToContent(bool animate)
		{
			if (!layoutWeakReference.TryGetTarget(out var layout))
				return;

			previousState = LayoutState.None;
			await ChildrenFadeTo(layout, animate, true);

			// Put the original content back in.
			layout.Children.Clear();

			foreach (var item in originalContent)
			{
				item.Opacity = animate ? 0 : 1;
				layout.Children.Add(item);
			}

			await ChildrenFadeTo(layout, animate, false);
		}

		public void SwitchToTemplate(string customState, bool animate)
			=> SwitchToTemplate(LayoutState.Custom, customState, animate);

		public async void SwitchToTemplate(LayoutState state, string customState, bool animate)
		{
			if (!layoutWeakReference.TryGetTarget(out var layout))
				return;

			// Put the original content somewhere where we can restore it.
			if (previousState == LayoutState.None)
			{
				originalContent = new List<View>();

				foreach (var item in layout.Children)
					originalContent.Add(item);
			}

			if (HasTemplateForState(state, customState))
			{
				previousState = state;

				await ChildrenFadeTo(layout, animate, true);

				layout.Children.Clear();

				var repeatCount = GetRepeatCount(state, customState);

				if (repeatCount == 1)
				{
					var s = new StackLayout { Opacity = animate ? 0 : 1 };

					if (layout is Grid grid)
					{
						if (grid.RowDefinitions.Any())
							Grid.SetRowSpan(s, grid.RowDefinitions.Count);

						if (grid.ColumnDefinitions.Any())
							Grid.SetColumnSpan(s, grid.ColumnDefinitions.Count);

						layout.Children.Add(s);
						layoutIsGrid = true;
					}

					var view = CreateItemView(state, customState);

					if (view != null)
					{
						if (layoutIsGrid)
							s.Children.Add(view);
						else
							layout.Children.Add(view);
					}
				}
				else
				{
					var template = GetRepeatTemplate(state, customState);
					var items = new List<int>();

					for (var i = 0; i < repeatCount; i++)
						items.Add(i);

					var s = new StackLayout { Opacity = animate ? 0 : 1 };

					if (layout is Grid grid)
					{
						if (grid.RowDefinitions.Any())
							Grid.SetRowSpan(s, grid.RowDefinitions.Count);

						if (grid.ColumnDefinitions.Any())
							Grid.SetColumnSpan(s, grid.ColumnDefinitions.Count);
					}

					BindableLayout.SetItemTemplate(s, template);
					BindableLayout.SetItemsSource(s, items);

					layout.Children.Add(s);
				}
				await ChildrenFadeTo(layout, animate, false);
			}
		}

		bool HasTemplateForState(LayoutState state, string customState)
		{
			var template = StateViews.FirstOrDefault(x => (x.StateKey == state && state != LayoutState.Custom) ||
							(state == LayoutState.Custom && x.CustomStateKey == customState));

			return template != null;
		}

		int GetRepeatCount(LayoutState state, string customState)
		{
			var template = StateViews.FirstOrDefault(x => (x.StateKey == state && state != LayoutState.Custom) ||
						   (state == LayoutState.Custom && x.CustomStateKey == customState));

			if (template != null)
				return template.RepeatCount;

			return 1;
		}

		DataTemplate GetRepeatTemplate(LayoutState state, string customState)
		{
			var template = StateViews.FirstOrDefault(x => (x.StateKey == state && state != LayoutState.Custom) ||
						   (state == LayoutState.Custom && x.CustomStateKey == customState));

			if (template != null)
				return template.RepeatTemplate;

			return null;
		}

		View CreateItemView(LayoutState state, string customState)
		{
			var template = StateViews.FirstOrDefault(x => (x.StateKey == state && state != LayoutState.Custom) ||
							(state == LayoutState.Custom && x.CustomStateKey == customState));

			// TODO: This only allows for a repeatcount of 1.
			// Internally in Xamarin.Forms we cannot add the same element to Children multiple times.
			if (template != null)
				return template;

			return new Label() { Text = $"Template for {state}{customState} not defined." };
		}

		async Task ChildrenFadeTo(Layout<View> layout, bool animate, bool isHide)
		{
			if (animate && layout?.Children?.Count > 0)
			{
				var opacity = 1;
				var time = 500u;

				if (isHide)
				{
					opacity = 0;
					time = 100u;
				}

				await Task.WhenAll(layout.Children.Select(a => a.FadeTo(opacity, time)));
			}
		}
	}
}