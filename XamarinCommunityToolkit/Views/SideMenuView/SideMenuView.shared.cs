using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using static System.Math;
using static Xamarin.Forms.AbsoluteLayout;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Microsoft.Toolkit.Xamarin.Forms.UI.Views
{
    [ContentProperty(nameof(Children))]
    public class SideMenuView : TemplatedView
    {
        const string animationName = nameof(SideMenuView);

        const uint animationRate = 16;

        const uint animationLength = 350;

        const int maxTimeDiffItemsCount = 24;

        const int minSwipeTimeDiffItemsCount = 2;

        const double swipeThresholdDistance = 17;

        const double acceptMoveThresholdPercentage = 0.3;

        const uint swipeAnimationAccelerationFactor = 2;

        static readonly Easing animationEasing = Easing.SinOut;

        static readonly TimeSpan swipeThresholdTime = TimeSpan.FromMilliseconds(Device.RuntimePlatform == Device.Android ? 100 : 60);

        public static readonly BindableProperty DiffProperty
            = BindableProperty.Create(nameof(Diff), typeof(double), typeof(SideMenuView), 0.0, BindingMode.OneWayToSource);

        public static readonly BindableProperty CurrentGestureDiffProperty
            = BindableProperty.Create(nameof(CurrentGestureDiff), typeof(double), typeof(SideMenuView), 0.0, BindingMode.OneWayToSource);

        public static readonly BindableProperty GestureThresholdProperty
            = BindableProperty.Create(nameof(GestureThreshold), typeof(double), typeof(SideMenuView), 7.0);

        public static readonly BindableProperty CancelVerticalGestureThresholdProperty
            = BindableProperty.Create(nameof(CancelVerticalGestureThreshold), typeof(double), typeof(SideMenuView), 1.0);

        public static readonly BindableProperty AllowInterceptGestureProperty
            = BindableProperty.Create(nameof(AllowInterceptGesture), typeof(bool), typeof(SideMenuView), false);

        public static readonly BindableProperty StateProperty
            = BindableProperty.Create(nameof(State), typeof(SideMenuState), typeof(SideMenuView), SideMenuState.Default, BindingMode.TwoWay, propertyChanged: OnStatePropertyChanged);

        public static readonly BindableProperty CurrentGestureStateProperty
            = BindableProperty.Create(nameof(CurrentGestureState), typeof(SideMenuState), typeof(SideMenuView), SideMenuState.Default, BindingMode.OneWayToSource);

        public static readonly BindableProperty PositionProperty
            = BindableProperty.CreateAttached(nameof(GetPosition), typeof(SideMenuPosition), typeof(SideMenuView), SideMenuPosition.None);

        public static readonly BindableProperty MenuWidthPercentageProperty
            = BindableProperty.CreateAttached(nameof(GetMenuWidthPercentage), typeof(double), typeof(SideMenuView), -1.0);

        public static readonly BindableProperty MenuGestureEnabledProperty
            = BindableProperty.CreateAttached(nameof(GetMenuGestureEnabled), typeof(bool), typeof(SideMenuView), true);

        readonly List<TimeDiffItem> timeDiffItems;

        readonly SideMenuElementCollection children;

        AbsoluteLayout mainLayout;

        readonly View overlayView;

        View mainView;

        View leftMenu;

        View rightMenu;

        View activeMenu;

        View inactiveMenu;

        double zeroDiff;

        bool isGestureStarted;

        bool isGestureDirectionResolved;

        bool isSwipe;

        double previousDiff;

        public SideMenuView()
        {
            timeDiffItems = new List<TimeDiffItem>();

            children = new SideMenuElementCollection();
            children.CollectionChanged += OnChildrenCollectionChanged;

            overlayView = SetupMainViewLayout(new BoxView
            {
                InputTransparent = true,
                GestureRecognizers =
                {
                    new TapGestureRecognizer
                    {
                        Command = new Command(() => State = SideMenuState.Default)
                    }
                }
            });

            if (Device.RuntimePlatform != Device.Android)
            {
                var panGestureRecognizer = new PanGestureRecognizer();
                panGestureRecognizer.PanUpdated += OnPanUpdated;
                GestureRecognizers.Add(panGestureRecognizer);
            }

            ControlTemplate = new ControlTemplate(typeof(AbsoluteLayout));
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var diff = e.TotalX;
            var verticalDiff = e.TotalY;
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    OnTouchStarted();
                    return;
                case GestureStatus.Running:
                    OnTouchChanged(diff, verticalDiff);
                    return;
                case GestureStatus.Canceled:
                case GestureStatus.Completed:
                    if (Device.RuntimePlatform == Device.Android)
                        OnTouchChanged(diff, verticalDiff);

                    OnTouchEnded();
                    return;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public async void OnSwiped(SwipeDirection swipeDirection)
        {
            await Task.Delay(1);
            if (isGestureStarted)
                return;

            var state = ResolveSwipeState(swipeDirection == SwipeDirection.Right);
            UpdateState(state, true);
        }

        public new ISideMenuList<View> Children
            => children;

        public double Diff
        {
            get => (double)GetValue(DiffProperty);
            set => SetValue(DiffProperty, value);
        }

        public double CurrentGestureDiff
        {
            get => (double)GetValue(CurrentGestureDiffProperty);
            set => SetValue(CurrentGestureDiffProperty, value);
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

        public bool AllowInterceptGesture
        {
            get => (bool)GetValue(AllowInterceptGestureProperty);
            set => SetValue(AllowInterceptGestureProperty, value);
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

        protected override void OnChildAdded(Element child)
        {
            mainLayout ??= child as AbsoluteLayout;
            if (mainLayout != null)
            {
                mainLayout.Children.Add(overlayView);
                mainLayout.LayoutChanged += OnLayoutChanged;
            }

            base.OnChildAdded(child);
        }

        protected override void OnChildRemoved(Element child)
        {
            if (child == mainLayout)
            {
                mainLayout.LayoutChanged -= OnLayoutChanged;
                mainLayout.Children.Remove(overlayView);
                mainLayout = null;
            }
            base.OnChildRemoved(child);
        }

        static void OnStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
            => ((SideMenuView)bindable).OnStatePropertyChanged();

        void OnStatePropertyChanged()
            => PerformAnimation();

        void OnTouchStarted()
        {
            if (isGestureStarted)
                return;

            isGestureDirectionResolved = false;
            isGestureStarted = true;
            zeroDiff = 0;
            PopulateDiffItems(0);
        }

        void OnTouchChanged(double diff, double verticalDiff)
        {
            if (!isGestureStarted || Abs(CurrentGestureDiff - diff) <= double.Epsilon)
                return;

            PopulateDiffItems(diff);
            var absDiff = Abs(diff);
            var absVerticalDiff = Abs(verticalDiff);
            if (!isGestureDirectionResolved && Max(absDiff, absVerticalDiff) > CancelVerticalGestureThreshold)
            {
                absVerticalDiff *= 2.5;
                if (absVerticalDiff >= absDiff)
                {
                    isGestureStarted = false;
                    OnTouchEnded();
                    return;
                }
                isGestureDirectionResolved = true;
            }

            mainView.AbortAnimation(animationName);
            var totalDiff = previousDiff + diff;
            if (!TryUpdateDiff(totalDiff - zeroDiff, false))
                zeroDiff = totalDiff - Diff;
        }

        void OnTouchEnded()
        {
            if (!isGestureStarted)
                return;

            isGestureStarted = false;
            CleanDiffItems();

            previousDiff = Diff;
            var state = State;
            var isSwipe = TryResolveFlingGesture(ref state);
            PopulateDiffItems(0);
            timeDiffItems.Clear();
            UpdateState(state, isSwipe);
        }

        void PerformAnimation()
        {
            var state = State;
            var start = Diff;
            var menuWidth = (state == SideMenuState.LeftMenuShown ? leftMenu : rightMenu)?.Width ?? 0;
            var end = Sign((int)state) * menuWidth;

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
            var animation = new Animation(v => TryUpdateDiff(v, true), Diff, end);
            mainView.Animate(animationName, animation, animationRate, animationLength, animationEasing, (v, isCanceled) =>
            {
                if (isCanceled)
                    return;

                SetOverlayViewInputTransparent(state);
            });
        }

        void SetOverlayViewInputTransparent(SideMenuState state)
            => overlayView.InputTransparent = state == SideMenuState.Default;

        SideMenuState ResolveSwipeState(bool isRightSwipe)
        {
            var left = SideMenuState.LeftMenuShown;
            var right = SideMenuState.RightMenuShown;
            switch (State)
            {
                case SideMenuState.LeftMenuShown:
                    right = SideMenuState.Default;
                    SetActiveView(true);
                    break;
                case SideMenuState.RightMenuShown:
                    left = SideMenuState.Default;
                    SetActiveView(false);
                    break;
            }
            return isRightSwipe ? left : right;
        }

        bool TryUpdateDiff(double diff, bool shouldUpdatePreviousDiff)
        {
            SetActiveView(diff >= 0);
            if (activeMenu == null || !GetMenuGestureEnabled(activeMenu))
                return false;

            diff = Sign(diff) * Min(Abs(diff), activeMenu.Width);
            if (Abs(Diff - diff) <= double.Epsilon)
                return false;

            Diff = diff;
            SetCurrentGestureState(diff);
            if (shouldUpdatePreviousDiff)
                previousDiff = diff;
            
            mainView.TranslationX = diff;
            overlayView.TranslationX = diff;
            return true;
        }

        void SetCurrentGestureState(double diff)
        {
            var menuWidth = activeMenu?.Width ?? Width;
            var moveThreshold = menuWidth * acceptMoveThresholdPercentage;
            var absDiff = Abs(diff);
            var state = State;
            if (Sign(diff) != (int)state)
                state = SideMenuState.Default;

            if (state == SideMenuState.Default && absDiff <= moveThreshold ||
                state != SideMenuState.Default && absDiff < menuWidth - moveThreshold)
            {
                CurrentGestureState = SideMenuState.Default;
                return;
            }
            if (diff >= 0)
            {
                CurrentGestureState = SideMenuState.LeftMenuShown;
                return;
            }
            CurrentGestureState = SideMenuState.RightMenuShown;
        }

        void UpdateState(SideMenuState state, bool isSwipe)
        {
            this.isSwipe = isSwipe;
            if (State == state)
            {
                PerformAnimation();
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
            }
            else
            {
                activeMenu = rightMenu;
                inactiveMenu = leftMenu;
            }

            if (inactiveMenu == null ||
                activeMenu == null ||
                leftMenu.X + leftMenu.Width <= rightMenu.X ||
                mainLayout.Children.IndexOf(inactiveMenu) < mainLayout.Children.IndexOf(activeMenu))
                return;

            LowerChild(inactiveMenu);
        }

        bool TryResolveFlingGesture(ref SideMenuState state)
        {
            if (state != CurrentGestureState)
            {
                state = CurrentGestureState;
                return false;
            }

            if (timeDiffItems.Count < minSwipeTimeDiffItemsCount)
                return false;
            
            var lastItem = timeDiffItems.LastOrDefault();
            var firstItem = timeDiffItems.FirstOrDefault();
            var distDiff = lastItem.Diff - firstItem.Diff;

            if (Sign(distDiff) != Sign(lastItem.Diff))
                return false;

            var absDistDiff = Abs(distDiff);
            var timeDiff = lastItem.Time - firstItem.Time;

            var acceptValue = swipeThresholdDistance * timeDiff.TotalMilliseconds / swipeThresholdTime.TotalMilliseconds;

            if (absDistDiff < acceptValue)
                return false;

            state = ResolveSwipeState(distDiff > 0);
            return true;
        }

        void PopulateDiffItems(double diff)
        {
            CurrentGestureDiff = diff;

            if (timeDiffItems.Count > maxTimeDiffItemsCount)
                CleanDiffItems();

            timeDiffItems.Add(new TimeDiffItem { Time = DateTime.UtcNow, Diff = diff });
        }

        void CleanDiffItems()
        {
            var time = timeDiffItems.LastOrDefault().Time;
            for (var i = timeDiffItems.Count - 1; i >= 0; --i)
                if (time - timeDiffItems[i].Time > swipeThresholdTime)
                    timeDiffItems.RemoveAt(i);
        }

        void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (var item in e.OldItems)
                    RemoveChild((View)item);

            if (e.NewItems != null)
                foreach (var item in e.NewItems)
                    AddChild((View)item);
        }

        void AddChild(View view)
        {
            mainLayout.Children.Add(view);
            switch (GetPosition(view))
            {
                case SideMenuPosition.None:
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
            mainLayout.Children.Remove(view);
            switch (GetPosition(view))
            {
                case SideMenuPosition.None:
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

        void OnLayoutChanged(object sender, EventArgs e)
        {
            if (mainView == null)
                return;

            mainLayout.RaiseChild(mainView);
            mainLayout.RaiseChild(overlayView);
        }

        View SetupMainViewLayout(View view)
        {
            SetLayoutFlags(view, AbsoluteLayoutFlags.All);
            SetLayoutBounds(view, new Rectangle(0, 0, 1, 1));
            return view;
        }

        View SetupMenuLayout(View view, bool isLeft)
        {
            var width = GetMenuWidthPercentage(view);
            var flags = width > 0
                ? AbsoluteLayoutFlags.All
                : AbsoluteLayoutFlags.PositionProportional | AbsoluteLayoutFlags.HeightProportional;
            SetLayoutFlags(view, flags);
            SetLayoutBounds(view, new Rectangle(isLeft ? 0 : 1, 0, width, 1));
            return view;
        }
    }
}
