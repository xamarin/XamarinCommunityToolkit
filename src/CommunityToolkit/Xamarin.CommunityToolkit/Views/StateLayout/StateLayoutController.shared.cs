using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
		CancellationTokenSource animationTokenSource;

		public IList<StateView> StateViews { get; set; }

		public StateLayoutController(Layout<View> layout)
			=> layoutWeakReference = new WeakReference<Layout<View>>(layout);

		public async void SwitchToContent(bool animate)
		{
			if (!layoutWeakReference.TryGetTarget(out var layout))
				return;

			var token = RebuildAnimationTokenSource(layout);

			previousState = LayoutState.None;
			await ChildrenFadeTo(layout, animate, true);

			if (token.IsCancellationRequested)
				return;

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

			var token = RebuildAnimationTokenSource(layout);

			// Put the original content somewhere where we can restore it.
			if (previousState == LayoutState.None)
			{
				originalContent = new List<View>();

				foreach (var item in layout.Children)
					originalContent.Add(item);
			}

			var view = GetViewForState(state, customState);

			if (view != null)
			{
				previousState = state;

				await ChildrenFadeTo(layout, animate, true);

				if (token.IsCancellationRequested)
					return;

				layout.Children.Clear();

				var repeatCount = GetRepeatCount(state, customState);
				var template = GetTemplate(state, customState);

				if (template != null)
				{
					// We have a template we can use.
					var items = new List<int>();

					for (var i = 0; i < repeatCount; i++)
						items.Add(i);

					// We create a StackLayout to stack repeating items.
					// It takes VerticalOptions and HorizontalOptions from the
					// StateView to allow for more control over how it layouts.
					var s = new StackLayout
					{
						Opacity = animate ? 0 : 1,
						VerticalOptions = view.VerticalOptions,
						HorizontalOptions = view.HorizontalOptions
					};

					// If the layout we're applying StateLayout to is a Grid,
					// we want to have the StateLayout span the entire Grid surface.
					// Otherwise it would just end up in row 0 : column 0.
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
				else
				{
					if (repeatCount > 1)
						throw new ArgumentException("Please use a Template instead of directly defining content when using a RepeatCount > 1.");

					// No template, so we use the children of the StateView.
					// We create a StackLayout to stack repeating items.
					// It takes VerticalOptions and HorizontalOptions from the
					// StateView to allow for more control over how it layouts.
					var s = new StackLayout
					{
						Opacity = animate ? 0 : 1,
						VerticalOptions = view.VerticalOptions,
						HorizontalOptions = view.HorizontalOptions
					};

					// If the layout we're applying StateLayout to is a Grid,
					// we want to have the StateLayout span the entire Grid surface.
					// Otherwise it would just end up in row 0 : column 0.
					if (layout is Grid grid)
					{
						if (grid.RowDefinitions.Any())
							Grid.SetRowSpan(s, grid.RowDefinitions.Count);

						if (grid.ColumnDefinitions.Any())
							Grid.SetColumnSpan(s, grid.ColumnDefinitions.Count);

						layout.Children.Add(s);
						layoutIsGrid = true;
					}

					var itemView = CreateItemView(state, customState);

					if (itemView != null)
					{
						if (layoutIsGrid)
							s.Children.Add(itemView);
						else
							layout.Children.Add(itemView);
					}
				}

				await ChildrenFadeTo(layout, animate, false);
			}
		}

		StateView GetViewForState(LayoutState state, string customState)
		{
			var view = StateViews.FirstOrDefault(x => (x.StateKey == state && state != LayoutState.Custom) ||
							(state == LayoutState.Custom && x.CustomStateKey == customState));

			return view;
		}

		int GetRepeatCount(LayoutState state, string customState)
		{
			var template = StateViews.FirstOrDefault(x => (x.StateKey == state && state != LayoutState.Custom) ||
						   (state == LayoutState.Custom && x.CustomStateKey == customState));

			if (template != null)
				return template.RepeatCount;

			return 1;
		}

		DataTemplate GetTemplate(LayoutState state, string customState)
		{
			var view = StateViews.FirstOrDefault(x => (x.StateKey == state && state != LayoutState.Custom) ||
						   (state == LayoutState.Custom && x.CustomStateKey == customState));

			if (view != null)
				return view.Template;

			return null;
		}

		View CreateItemView(LayoutState state, string customState)
		{
			var view = StateViews.FirstOrDefault(x => (x.StateKey == state && state != LayoutState.Custom) ||
							(state == LayoutState.Custom && x.CustomStateKey == customState));

			// TODO: This only allows for a repeatcount of 1.
			// Internally in Xamarin.Forms we cannot add the same element to Children multiple times.
			if (view != null)
				return view;

			return new Label() { Text = $"View for {state}{customState} not defined." };
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

		CancellationToken RebuildAnimationTokenSource(Layout<View> layout)
		{
			animationTokenSource?.Cancel();
			animationTokenSource?.Dispose();

			foreach (var child in layout.Children)
				ViewExtensions.CancelAnimations(child);

			animationTokenSource = new CancellationTokenSource();
			return animationTokenSource.Token;
		}
	}
}