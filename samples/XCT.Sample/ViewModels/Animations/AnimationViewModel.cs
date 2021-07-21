using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Animations
{
	public class AnimationViewModel : BaseViewModel
	{
		AnimationWrapper? currentAnimation;
		AnimationDetailViewModel? selectedAnimation;

		public ObservableCollection<AnimationDetailViewModel> Animations { get; }

		public AnimationDetailViewModel? SelectedAnimation
		{
			get => selectedAnimation;
			set => SetProperty(ref selectedAnimation, value);
		}

		public Command StartAnimationCommand { get; }
		public Command StopAnimationCommand { get; }

		public AnimationViewModel()
		{
			Animations = new ObservableCollection<AnimationDetailViewModel>
			{
				new AnimationDetailViewModel("Tada", (view, onFinished) => new TadaAnimationType().CreateAnimation(onFinished: onFinished, views: view)),
				//new AnimationDetailViewModel("RubberBand", (view, onFinished) => new RubberBandAnimation(onFinished: onFinished, views: view))
			};

			SelectedAnimation = Animations.First();
			StartAnimationCommand = new Command<View>(OnStart, (view) => !(SelectedAnimation is null) && currentAnimation?.IsRunning != true);
			StopAnimationCommand = new Command(OnStop, () => currentAnimation?.IsRunning == true);
		}

		void OnStart(View view)
		{
			if (currentAnimation != null)
			{
				currentAnimation.Abort();
			}

			currentAnimation = SelectedAnimation!.CreateAnimation(view, (d, b) =>
			{
				StartAnimationCommand.ChangeCanExecute();
				StopAnimationCommand.ChangeCanExecute();
			});

			currentAnimation.Commit();

			StopAnimationCommand.ChangeCanExecute();
		}

		void OnStop()
		{
			if (currentAnimation != null)
			{
				currentAnimation.Abort();
			}
		}
	}
}

