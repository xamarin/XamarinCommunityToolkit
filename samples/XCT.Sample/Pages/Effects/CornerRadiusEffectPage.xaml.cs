using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.Pages.Effects
{
	public partial class CornerRadiusEffectPage
	{
		public CornerRadiusEffectPage()
		{
			InitializeComponent();

			SliderCornerRadiusTopLeft.ValueChanged += OnCornerRadiusValueChanged;
			SliderCornerRadiusTopRight.ValueChanged += OnCornerRadiusValueChanged;
			SliderCornerRadiusBottomLeft.ValueChanged += OnCornerRadiusValueChanged;
			SliderCornerRadiusBottomRight.ValueChanged += OnCornerRadiusValueChanged;
		}

		void OnCornerRadiusValueChanged(object sender, ValueChangedEventArgs e)
		{
			CornerRadius = new CornerRadius(
				SliderCornerRadiusTopLeft.Value, SliderCornerRadiusTopRight.Value,
				SliderCornerRadiusBottomLeft.Value, SliderCornerRadiusBottomRight.Value);
			OnPropertyChanged(nameof(CornerRadius));
		}

		public CornerRadius CornerRadius { get; private set; } = new (10);
	}
}