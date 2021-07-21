using System;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Animations
{
	public class AnimationDetailViewModel : BaseViewModel
	{
		public string Name { get; }
		public Func<View, Action<double, bool>, AnimationWrapper> CreateAnimation { get; }

		public AnimationDetailViewModel(string name, Func<View, Action<double, bool>, AnimationWrapper> createAnimation)
		{
			Name = name;
			CreateAnimation = createAnimation;
		}
	}
}
