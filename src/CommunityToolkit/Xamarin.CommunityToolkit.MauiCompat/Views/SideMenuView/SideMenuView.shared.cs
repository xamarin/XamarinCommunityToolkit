using System;using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.UI.Views.Internals;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using static System.Math;
using static Microsoft.Maui.Controls.Compatibility.AbsoluteLayout;using Microsoft.Maui.Layouts;using AbsoluteLayout = Microsoft.Maui.Controls.Compatibility.AbsoluteLayout;
using IList = System.Collections.IList;

namespace Xamarin.CommunityToolkit.UI.Views
{
	[ContentProperty(nameof(Children))]
	public class SideMenuView : BaseTemplatedView<AbsoluteLayout>
	{
		const string animationName = nameof(SideMenuView);

		const uint animationRate = 16;

		const uint animationLength = 350;

		const int maxTimeShiftItemsCount = 24;

		const int minSwipeTimeShiftItemsCount = 2;

		const double swipeThresholdDistance = 17;

		const double acceptMoveThresholdPercentage = 0.3;

		const uint swipeAnimationAccelerationFactor = 2;

		static readonly Easing animationEasing = Easing.SinOut;

		static readonly TimeSpan swipeThresholdTime = TimeSpan.FromMilliseconds(Device.RuntimePlatform == Device.Android ? 100 : 60);

		readonly List<TimeShiftItem> timeShiftItems = new List<TimeShiftItem>();

		readonly SideMenuElementCollection children = new SideMenuElementCollection();

		View? overlayView, mainView, leftMenu, rightMenu, activeMenu, inactiveMenu;

		double zeroShift;

		bool isGestureStarted;

		bool isGestureDirectionResolved;

		bool isSwipe;

		double previousShift;

		public static readonly BindableProperty ShiftProperty
			= BindableProperty.Create(nameof(Shift), typeof(double), typeof(SideMenuView), 0.0, BindingMode.OneWayToSource);

		public static readonly BindableProperty CurrentGestureShiftProperty
			= BindableProperty.Create(nameof(CurrentGestureShift), typeof(double), typeof(SideMenuView), 0.0, BindingMode.OneWayToSource);

		public static readonly BindableProperty GestureThresholdProperty
			= BindableProperty.Create(nameof(GestureThreshold), typeof(double), typeof(SideMenuView), -1.0);

		public static readonly BindableProperty CancelVerticalGestureThresholdProperty
			= BindableProperty.Create(nameof(CancelVerticalGestureThreshold), typeof(double), typeof(SideMenuView), 1.0);

		public static readonly BindableProperty StateProperty
			= BindableProperty.Create(nameof(State), typeof(SideMenuState), typeof(SideMenuView), SideMenuState.MainViewShown, BindingMode.TwoWay, propertyChanged: OnStatePropertyChanged);

		public static readonly BindableProperty CurrentGestureStateProperty
			= BindableProperty.Create(nameof(CurrentGestureState), typeof(SideMenuState), typeof(SideMenuView), SideMenuState.MainViewShown, BindingMode.OneWayToSource);

		public static readonly BindableProperty PositionProperty
			= BindableProperty.CreateAttached(nameof(GetPosition), typeof(SideMenuPosition), typeof(SideMenuView), SideMenuPosition.MainView);

		public static readonly BindableProperty MenuWidthPercentageProperty
			= BindableProperty.CreateAttached(nameof(GetMenuWidthPercentage), typeof(double), typeof(SideMenuView), -1.0);

		public static readonly BindableProperty MenuGestureEnabledProperty
			= BindableProperty.CreateAttached(nameof(GetMenuGestureEnabled), typeof(bool), typeof(SideMenuView), true);

		public static readonly BindableProperty MainViewScaleFactorProperty
			= BindableProperty.CreateAttached(nameof(GetMainViewScaleFactor), typeof(double), typeof(SideMenuView), 1.0);

		public static readonly BindableProperty MainViewOpacityFactorProperty
			= BindableProperty.CreateAttached(nameof(GetMainViewOpacityFactor), typeof(double), typeof(SideMenuView), 1.0);

		public static readonly BindableProperty MenuAppearanceTypeProperty
			= BindableProperty.CreateAttached(nameof(GetMenuAppearanceType), typeof(SideMenuAppearanceType), typeof(SideMenuView), SideMenuAppearanceType.SlideOut);

		public static readonly BindableProperty ParallaxValueProperty
			= BindableProperty.CreateAttached(nameof(GetParallaxValue), typeof(double), typeof(SideMenuView), 0.0);

		public SideMenuView()
		{
			#region Required work-around to prevent linker from removing the platform-specific implementation
#if __ANDROID__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.Android.UI.Views.SideMenuViewRenderer(XCT.Context ?? throw new NullReferenceException());
#elif __IOS__
			if (System.DateTime.Now.Ticks < 0)
				_ = new Xamarin.CommunityToolkit.iOS.UI.Views.SideMenuViewRenderer();
#endif
			#endregion
		}

		public new ISideMenuList<View> Children
			=> children;

		public double Shift
		{
			get => (double)GetValue(ShiftProperty);
			set => SetValue(ShiftProperty, value);
		}

		public double CurrentGestureShift
		{
			get => (double)GetValue(CurrentGestureShiftProperty);
			set => SetValue(CurrentGestureShiftProperty, value);
		}

		public double GestureThreshold
		{
			get => (double)GetValue(GestureThresholdProperty);
			set => SetValue(GestureThresholdProperty, value);
		}

		public double CancelVerticalGestureThreshold
		{
			get => (double)GetValue(CancelVerticalGestureThresholdProperty);
			set => SetValue(CancelVerticalGestureThresholdProperty, value);
		}

		public SideMenuState State
		{
			get => (SideMenuState)GetValue(StateProperty);
			set => SetValue(StateProperty, value);
		}

		public SideMenuState CurrentGestureState
		{
			get => (SideMenuState)GetValue(CurrentGestureStateProperty);
			set => SetValue(CurrentGestureStateProperty, value);
		}

		public static SideMenuPosition GetPosition(BindableObject bindable)
			=> (SideMenuPosition)bindable.GetValue(PositionProperty);

		public static void SetPosition(BindableObject bindable, SideMenuPosition value)
			=> bindable.SetValue(PositionProperty, value);

		public static double GetMenuWidthPercentage(BindableObject bindable)
			=> (double)bindable.GetValue(MenuWidthPercentageProperty);

		public static void SetMenuWidthPercentage(BindableObject bindable, double value)
			=> bindable.SetValue(MenuWidthPercentageProperty, value);

		public static bool GetMenuGestureEnabled(BindableObject bindable)
			=> (bool)bindable.GetValue(MenuGestureEnabledProperty);

		public static void SetMenuGestureEnabled(BindableObject bindable, bool value)
			=> bindable.SetValue(MenuGestureEnabledProperty, value);

		public static double GetMainViewScaleFactor(BindableObject bindable)
			=> (double)bindable.GetValue(MainViewScaleFactorProperty);

		public static void SetMainViewScaleFactor(BindableObject bindable, double value)
			=> bindable.SetValue(MainViewScaleFactorProperty, value);

		public static double GetMainViewOpacityFactor(BindableObject bindable)
			=> (double)bindable.GetValue(MainViewOpacityFactorProperty);

		public static void SetMainViewOpacityFactor(BindableObject bindable, double value)
			=> bindable.SetValue(MainViewOpacityFactorProperty, value);

		public static SideMenuAppearanceType GetMenuAppearanceType(BindableObject bindable)
			=> (SideMenuAppearanceType)bindable.GetValue(MenuAppearanceTypeProperty);

		public static void SetMenuAppearanceType(BindableObject bindable, SideMenuAppearanceType value)
			=> bindable.SetValue(MenuAppearanceTypeProperty, value);

		public static double GetParallaxValue(BindableObject bindable)
			=> (double)bindable.GetValue(ParallaxValueProperty);

		public static void SetParallaxValue(BindableObject bindable, double value)
			=> bindable.SetValue(ParallaxValueProperty, value);

		internal void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
		{
			var shift = e.TotalX;
			var verticalShift = e.TotalY;
			switch (e.StatusType)
			{
				case GestureStatus.Started:
					OnTouchStarted();
					return;
				case GestureStatus.Running:
					OnTouchChanged(shift, verticalShift);
					return;
				case GestureStatus.Canceled:
				case GestureStatus.Completed:
					if (Device.RuntimePlatform == Device.Android)
						OnTouchChanged(shift, verticalShift);

					OnTouchEnded();
					return;
			}
		}

		internal async void OnSwiped(SwipeDirection swipeDirection)
		{
			await Task.Delay(1);
			if (isGestureStarted)
				return;

			var state = ResolveSwipeState(swipeDirection == SwipeDirection.Right);
			UpdateState(state, true);
		}

		internal bool CheckGestureEnabled(SideMenuPosition menuPosition)
			=> menuPosition switch
			{
				SideMenuPosition.LeftMenu => CheckMenuGestureEnabled(leftMenu),
				SideMenuPosition.RightMenu => CheckMenuGestureEnabled(rightMenu),
				_ => true
			};

		protected override void OnControlInitialized(AbsoluteLayout control)
		{
			children.CollectionChanged += OnChildrenCollectionChanged;

			overlayView = SetupMainViewLayout(new BoxView
			{
				InputTransparent = true,
				Color = Colors.Transparent,
				GestureRecognizers =
				{
					new TapGestureRecognizer
					{
						Command = new Command(() => State = SideMenuState.MainViewShown)
					}
				}
			});

			if (Device.RuntimePlatform != Device.Android)
			{
				var panGestureRecognizer = new PanGestureRecognizer();
				panGestureRecognizer.PanUpdated += OnPanUpdated;
				GestureRecognizers.Add(panGestureRecognizer);
			}

			control.Children.Add(overlayView);
			control.LayoutChanged += OnLayoutChanged;
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
			PerformUpdate(false);
		}

		static View SetupMainViewLayout(View view)
		{
			SetLayoutFlags(view, AbsoluteLayoutFlags.All);
			SetLayoutBounds(view, new Rect(0, 0, 1, 1));
			return view;
		}

		static View SetupMenuLayout(View view, bool isLeft)
		{
			var width = GetMenuWidthPercentage(view);
			var flags = width > 0
				? AbsoluteLayoutFlags.All
				: AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.HeightProportional;
			SetLayoutFlags(view, flags);
			SetLayoutBounds(view, new Rect(isLeft ? 0 : 1, 0, width, 1));
			return view;
		}

		static void OnStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
			=> ((SideMenuView)bindable).OnStatePropertyChanged();

		void OnStatePropertyChanged()
			=> PerformUpdate(true);

		void OnTouchStarted()
		{
			if (isGestureStarted)
				return;

			isGestureDirectionResolved = false;
			isGestureStarted = true;
			zeroShift = 0;
			PopulateTimeShiftItems(0);
		}

		void OnTouchChanged(double shift, double verticalShift)
		{
			if (!isGestureStarted || Abs(CurrentGestureShift - shift) <= double.Epsilon)
				return;

			PopulateTimeShiftItems(shift);
			var absShift = Abs(shift);
			var absVerticalShift = Abs(verticalShift);
			if (!isGestureDirectionResolved && Max(absShift, absVerticalShift) > CancelVerticalGestureThreshold)
			{
				absVerticalShift *= 2.5;
				if (absVerticalShift >= absShift)
				{
					isGestureStarted = false;
					OnTouchEnded();
					return;
				}
				isGestureDirectionResolved = true;
			}

			mainView.AbortAnimation(animationName);
			var totalShift = previousShift + shift;
			if (!TryUpdateShift(totalShift - zeroShift, true))
				zeroShift = totalShift - Shift;
		}

		void OnTouchEnded()
		{
			if (!isGestureStarted)
				return;

			isGestureStarted = false;
			CleanTimeShiftItems();

			previousShift = Shift;
			var state = State;
			var isSwipe = TryResolveFlingGesture(ref state);
			PopulateTimeShiftItems(0);
			timeShiftItems.Clear();
			UpdateState(state, isSwipe);
		}

		void PerformUpdate(bool isAnimated)
		{
			var state = State;
			var start = Shift;
			var menuWidth = (state == SideMenuState.LeftMenuShown ? leftMenu : rightMenu)?.Width ?? 0;
			var end = -Sign((int)state) * menuWidth;

			if (!isAnimated)
			{
				TryUpdateShift(end, false);
				SetOverlayViewInputTransparent(state);
				return;
			}

			var animationLength = (uint)(SideMenuView.animationLength * Abs(start - end) / Width);
			if (isSwipe)
			{
				isSwipe = false;
				animationLength /= swipeAnimationAccelerationFactor;
			}
			if (animationLength == 0)
			{
				SetOverlayViewInputTransparent(state);
				return;
			}
			var animation = new Animation(v => TryUpdateShift(v, false), Shift, end);
			mainView.Animate(animationName, animation, animationRate, animationLength, animationEasing, (v, isCanceled) =>
			{
				if (isCanceled)
					return;

				SetOverlayViewInputTransparent(state);
			});
		}

		void SetOverlayViewInputTransparent(SideMenuState state)
		{
			_ = overlayView ?? throw new NullReferenceException();
			overlayView.InputTransparent = state == SideMenuState.MainViewShown;
		}

		SideMenuState ResolveSwipeState(bool isRightSwipe)
		{
			var left = SideMenuState.LeftMenuShown;
			var right = SideMenuState.RightMenuShown;
			switch (State)
			{
				case SideMenuState.LeftMenuShown:
					right = SideMenuState.MainViewShown;
					SetActiveView(true);
					break;
				case SideMenuState.RightMenuShown:
					left = SideMenuState.MainViewShown;
					SetActiveView(false);
					break;
			}
			return isRightSwipe ? left : right;
		}

		bool TryUpdateShift(double shift, bool isUserInteraction)
		{
			var isLeft = shift >= 0;
			SetActiveView(isLeft);
			if (activeMenu == null)
				return false;

			if (isUserInteraction && !GetMenuGestureEnabled(activeMenu))
				return false;

			_ = mainView ?? throw new NullReferenceException();

			var activeMenuWidth = activeMenu.Width;
			var mainViewWidth = mainView.Width;

			var sign = Sign(shift);
			shift = sign * Min(Abs(shift), activeMenuWidth);
			if (isUserInteraction && Abs(Shift - shift) <= double.Epsilon)
				return false;

			var nonZeroSign = isLeft ? -1 : 1;

			Shift = shift;
			SetCurrentGestureState(shift);
			if (!isUserInteraction)
				previousShift = shift;

			_ = overlayView ?? throw new NullReferenceException();

			using (mainView.Batch())
			using (activeMenu.Batch())
			using (overlayView.Batch())
			using (inactiveMenu?.Batch())
			{
				if (inactiveMenu != null)
					inactiveMenu.TranslationX = -inactiveMenu.Width * nonZeroSign;

				var progress = animationEasing.Ease(Abs(shift) / activeMenuWidth);
				var scale = 1 - ((1 - GetMainViewScaleFactor(activeMenu)) * progress);
				var opacity = 1 - ((1 - GetMainViewOpacityFactor(activeMenu)) * progress);

				var parallax = GetParallaxValue(activeMenu);
				parallax = Min(Abs(parallax), activeMenuWidth) * Sign(parallax);

				mainView.Scale = scale;
				mainView.Opacity = opacity;

				switch (GetMenuAppearanceType(activeMenu))
				{
					case SideMenuAppearanceType.SlideOut:
						activeMenu.TranslationX = parallax * (1 - progress) * nonZeroSign;
						mainView.TranslationX = shift - (sign * mainViewWidth * 0.5 * (1 - scale));
						overlayView.TranslationX = shift;
						break;
					case SideMenuAppearanceType.SlideIn:
						activeMenu.TranslationX = (activeMenuWidth - Abs(shift)) * nonZeroSign;
						mainView.TranslationX = parallax * nonZeroSign * progress;
						overlayView.TranslationX = parallax * nonZeroSign * progress;
						break;
					case SideMenuAppearanceType.SlideInOut:
						activeMenu.TranslationX = (activeMenuWidth - Abs(shift) - (parallax * (1 - progress))) * nonZeroSign;
						mainView.TranslationX = shift - (sign * mainViewWidth * 0.5 * (1 - scale));
						overlayView.TranslationX = shift;
						break;
				}
			}
			return true;
		}

		void SetCurrentGestureState(double shift)
		{
			var menuWidth = activeMenu?.Width ?? Width;
			var moveThreshold = menuWidth * acceptMoveThresholdPercentage;
			var absShift = Abs(shift);
			var state = State;
			if (Sign(shift) != -(int)state)
				state = SideMenuState.MainViewShown;

			if ((state == SideMenuState.MainViewShown && absShift <= moveThreshold) ||
				(state != SideMenuState.MainViewShown && absShift < menuWidth - moveThreshold))
			{
				CurrentGestureState = SideMenuState.MainViewShown;
				return;
			}
			if (shift >= 0)
			{
				CurrentGestureState = SideMenuState.LeftMenuShown;
				return;
			}
			CurrentGestureState = SideMenuState.RightMenuShown;
		}

		void UpdateState(SideMenuState state, bool isSwipe)
		{
			if (!CheckMenuGestureEnabled(state))
				return;

			this.isSwipe = isSwipe;
			if (State == state)
			{
				PerformUpdate(true);
				return;
			}
			State = state;
		}

		void SetActiveView(bool isLeft)
		{
			if (isLeft)
			{
				activeMenu = leftMenu;
				inactiveMenu = rightMenu;
				return;
			}

			activeMenu = rightMenu;
			inactiveMenu = leftMenu;
		}

		bool CheckMenuGestureEnabled(SideMenuState state)
		{
			var view = state switch
			{
				SideMenuState.LeftMenuShown => leftMenu,
				SideMenuState.RightMenuShown => rightMenu,
				_ => activeMenu
			};

			if (view == null)
				return false;

			return GetMenuGestureEnabled(view);
		}

		bool TryResolveFlingGesture(ref SideMenuState state)
		{
			if (state != CurrentGestureState)
			{
				state = CurrentGestureState;
				return false;
			}

			if (timeShiftItems.Count < minSwipeTimeShiftItemsCount)
				return false;

			var lastItem = timeShiftItems.LastOrDefault();
			var firstItem = timeShiftItems.FirstOrDefault();
			var shiftDifference = lastItem.Shift - firstItem.Shift;

			if (Sign(shiftDifference) != Sign(lastItem.Shift))
				return false;

			var absShiftDifference = Abs(shiftDifference);
			var timeDifference = lastItem.Time - firstItem.Time;

			var acceptValue = swipeThresholdDistance * timeDifference.TotalMilliseconds / swipeThresholdTime.TotalMilliseconds;

			if (absShiftDifference < acceptValue)
				return false;

			state = ResolveSwipeState(shiftDifference > 0);
			return true;
		}

		void PopulateTimeShiftItems(double shift)
		{
			CurrentGestureShift = shift;

			if (timeShiftItems.Count > maxTimeShiftItemsCount)
				CleanTimeShiftItems();

			timeShiftItems.Add(new TimeShiftItem { Time = DateTime.UtcNow, Shift = shift });
		}

		void CleanTimeShiftItems()
		{
			var time = timeShiftItems.LastOrDefault().Time;
			for (var i = timeShiftItems.Count - 1; i >= 0; --i)
			{
				if (time - timeShiftItems[i].Time > swipeThresholdTime)
					timeShiftItems.RemoveAt(i);
			}
		}

		void OnChildrenCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			HandleChildren(e.OldItems, RemoveChild);
			HandleChildren(e.NewItems, AddChild);
		}

		void HandleChildren(IList items, Action<View> action)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					if (item != null && action != null)
						action((View)item);
				}
			}
		}

		void AddChild(View view)
		{
			Control?.Children.Add(view);
			switch (GetPosition(view))
			{
				case SideMenuPosition.MainView:
					mainView = SetupMainViewLayout(view);
					break;
				case SideMenuPosition.LeftMenu:
					leftMenu = SetupMenuLayout(view, true);
					break;
				case SideMenuPosition.RightMenu:
					rightMenu = SetupMenuLayout(view, false);
					break;
			}
		}

		void RemoveChild(View view)
		{
			Control?.Children.Remove(view);
			switch (GetPosition(view))
			{
				case SideMenuPosition.MainView:
					mainView = null;
					break;
				case SideMenuPosition.LeftMenu:
					leftMenu = null;
					break;
				case SideMenuPosition.RightMenu:
					rightMenu = null;
					break;
			}

			if (activeMenu == view)
				activeMenu = null;
			else if (inactiveMenu == view)
				inactiveMenu = null;
		}

		void RaiseMenuIfNeeded(View? menuView)
		{
			if (menuView != null && GetMenuAppearanceType(menuView) == SideMenuAppearanceType.SlideIn)
				Control?.RaiseChild(menuView);
		}

		void OnLayoutChanged(object? sender, EventArgs e)
		{
			if (mainView == null)
				return;

			using (Control?.Batch())
			{
				Control?.RaiseChild(mainView);
				Control?.RaiseChild(overlayView);

				RaiseMenuIfNeeded(leftMenu);
				RaiseMenuIfNeeded(rightMenu);
			}
		}

		bool CheckMenuGestureEnabled(View? menuView)
			=> menuView != null && GetMenuGestureEnabled(menuView);
	}
}