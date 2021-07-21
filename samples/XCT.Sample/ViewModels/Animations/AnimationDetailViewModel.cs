using System;
using Xamarin.CommunityToolkit.Animations;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Animations
{
	public class AnimationDetailViewModel : BaseViewModel
	{
		public string Name { get; }
		public Func<View, Action<double, bool>, AnimationBase> CreateAnimation { get; }

		public AnimationDetailViewModel(string name, Func<View, Action<double, bool>, AnimationBase> createAnimation)
		{
			Name = name;
			CreateAnimation = createAnimation;
		}
	}
}
