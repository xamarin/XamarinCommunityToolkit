using System.ComponentModel;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Microsoft.Graphics.Canvas.Effects;
using FormsCommunityToolkit.Effects.UWP.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportEffect(typeof(ViewBlurEffect), nameof(ViewBlurEffect))]
namespace FormsCommunityToolkit.Effects.UWP.Effects
{
    public class ViewBlurEffect : PlatformEffect
    {
        private SpriteVisual _blurVisual;
        private CompositionBrush _blurBrush;
        private Visual _rootVisual;

        private Compositor Compositor { get; set; }

        protected override void OnAttached()
        {
            double blurAmount = (double)Element.GetValue(BlurEffect.BlurAmountProperty);

            _rootVisual = ElementCompositionPreview.GetElementVisual(Container);

            Compositor = _rootVisual.Compositor;

            _blurVisual = Compositor.CreateSpriteVisual();

            var brush = BuildBlurBrush();
            brush.SetSourceParameter("source", Compositor.CreateBackdropBrush());
            _blurBrush = brush;
            _blurVisual.Brush = _blurBrush;

            ElementCompositionPreview.SetElementChildVisual(Container, _blurVisual);

            Container.Loading += OnLoading;
            Container.Unloaded += OnUnloaded;

            _blurBrush.Properties.InsertScalar("Blur.BlurAmount", (float)blurAmount);
            _rootVisual.Properties.InsertScalar("BlurAmount", (float)blurAmount);

            SetUpPropertySetExpressions();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == "BlurAmount")
            {
                double blurAmount = (double)Element.GetValue(BlurEffect.BlurAmountProperty);
                _rootVisual.Properties.InsertScalar("BlurAmount", (float)blurAmount);
            }
        }

        private void SetUpPropertySetExpressions()
        {
            var exprAnimation = Compositor.CreateExpressionAnimation();
            exprAnimation.Expression = "sourceProperties.BlurAmount";
            exprAnimation.SetReferenceParameter("sourceProperties", _rootVisual.Properties);

            _blurBrush.Properties.StartAnimation("Blur.BlurAmount", exprAnimation);
        }

        protected override void OnDetached()
        {
            Container.SizeChanged -= OnSizeChanged;
        }

        private void OnLoading(FrameworkElement sender, object args)
        {
            Container.SizeChanged += OnSizeChanged;
            OnSizeChanged(this, null);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if(Container != null)
                Container.SizeChanged -= OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_blurVisual != null)
            {
                _blurVisual.Size = new Vector2((float)Container.ActualWidth, (float)Container.ActualHeight);
            }
        }

        private CompositionEffectBrush BuildBlurBrush()
        {
            var blurEffect = new GaussianBlurEffect()
            {
                Name = "Blur",
                BlurAmount = 0.0f,
                BorderMode = EffectBorderMode.Hard,
                Optimization = EffectOptimization.Balanced,
                Source = new CompositionEffectSourceParameter("source"),
            };

            var factory = Compositor.CreateEffectFactory(
                blurEffect,
                new[] { "Blur.BlurAmount" }
            );

            var brush = factory.CreateBrush();
            return brush;
        }
    }
}
