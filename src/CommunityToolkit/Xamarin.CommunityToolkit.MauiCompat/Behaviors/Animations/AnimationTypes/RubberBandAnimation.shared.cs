using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.Behaviors
{
	/// <summary>
	/// A 'Rubber band' animation. Results in:
	/// <list type="bullet">
	/// <item>stretching the width and squashing the height</item>
	/// <item>swapping to squash the width and stretch the height</item>
	/// <item>gradually swap until we reach 100% width and height again</item>
	/// </list>
	/// </summary>
	public class RubberBandAnimation : CompoundAnimationBase
	{
		/// <inheritdoc />
		protected override uint DefaultDuration { get; set; } = 1000;

		/// <inheritdoc />
		protected override Animation CreateAnimation(params View[] views) => Create(views);

		static Animation Create(params View[] views)
		{
			var animation = new Animation();

			foreach (var view in views)
			{
				animation.Add(0, 0.3, new Animation(v => view.ScaleX = v, 1, 1.25));
				animation.Add(0, 0.3, new Animation(v => view.ScaleY = v, 1, 0.75));

				animation.Add(0.3, 0.4, new Animation(v => view.ScaleX = v, 1.25, 0.75));
				animation.Add(0.3, 0.4, new Animation(v => view.ScaleY = v, 0.75, 1.25));

				animation.Add(0.4, 0.5, new Animation(v => view.ScaleX = v, 0.75, 1.15));
				animation.Add(0.4, 0.5, new Animation(v => view.ScaleY = v, 1.25, 0.85));

				animation.Add(0.5, 0.65, new Animation(v => view.ScaleX = v, 1.15, 0.95));
				animation.Add(0.5, 0.65, new Animation(v => view.ScaleY = v, 0.85, 1.05));

				animation.Add(0.65, 0.75, new Animation(v => view.ScaleX = v, 0.95, 1.05));
				animation.Add(0.65, 0.75, new Animation(v => view.ScaleY = v, 1.05, 0.95));

				animation.Add(0.75, 1, new Animation(v => view.ScaleX = v, 1.05, 1));
				animation.Add(0.75, 1, new Animation(v => view.ScaleY = v, 0.95, 1));
			}

			return animation;
		}
	}
}