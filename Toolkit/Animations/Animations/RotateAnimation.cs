using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Toolkit.Animations
{
    public class RotateToAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty =
            BindableProperty.Create(nameof(Rotation), typeof(double), typeof(RotateToAnimation), default(double), BindingMode.TwoWay, null);

        public double Rotation
        {
            get => (double)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.RotateTo(Rotation, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }

    public class RelRotateToAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty =
            BindableProperty.Create(nameof(Rotation), typeof(double), typeof(RelRotateToAnimation), default(double), BindingMode.TwoWay, null);

        public double Rotation
        {
            get => (double)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.RelRotateTo(Rotation, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }

    public class RotateXToAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty =
            BindableProperty.Create(nameof(Rotation), typeof(double), typeof(RotateXToAnimation), default(double), BindingMode.TwoWay, null);

        public double Rotation
        {
            get => (double)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.RotateXTo(Rotation, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }

    public class RotateYToAnimation : AnimationBase
    {
        public static readonly BindableProperty RotationProperty =
            BindableProperty.Create(nameof(Rotation), typeof(double), typeof(RotateYToAnimation), default(double), BindingMode.TwoWay, null);

        public double Rotation
        {
            get => (double)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.RotateYTo(Rotation, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }
}
