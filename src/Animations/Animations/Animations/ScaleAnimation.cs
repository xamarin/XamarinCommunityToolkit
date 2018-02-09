using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinCommunityToolkit.Animations
{
    public class ScaleToAnimation : AnimationBase
    {
        public static readonly BindableProperty ScaleProperty =
            BindableProperty.Create(nameof(Scale), typeof(double), typeof(ScaleToAnimation), default(double),  
                BindingMode.TwoWay, null);

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.ScaleTo(Scale, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }

    public class RelScaleToAnimation : AnimationBase
    {
        public static readonly BindableProperty ScaleProperty = 
            BindableProperty.Create(nameof(Scale), typeof(double), typeof(RelScaleToAnimation), default(double),      
                BindingMode.TwoWay, null);

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }

        protected override Task BeginAnimation()
        {
            if (Target == null)
            {
                throw new NullReferenceException("Null Target property.");
            }

            return Target.RelScaleTo(Scale, Convert.ToUInt32(Duration), EasingHelper.GetEasing(Easing));
        }
    }
}