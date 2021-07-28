using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Animations
{
	public class AnimationViewModel : BaseViewModel
	{
		PreBuiltAnimationBase? currentAnimation;
		CancellationTokenSource? cancellationTokenSource;
		AnimationDetailViewModel? selectedAnimation;

		public ObservableCollection<AnimationDetailViewModel> Animations { get; }

		public AnimationDetailViewModel? SelectedAnimation
		{
			get => selectedAnimation;
			set => SetProperty(ref selectedAnimation, value);
		}

		public AsyncCommand<View> StartAnimationCommand { get; }

		public Command StopAnimationCommand { get; }

		public AnimationViewModel()
		{
			Animations = new ObservableCollection<AnimationDetailViewModel>
			{
				new AnimationDetailViewModel("Tada", (view, onFinished) => new TadaAnimationType()),
				new AnimationDetailViewModel("RubberBand", (view, onFinished) => new RubberBandAnimationType())
			};

			SelectedAnimation = Animations.First();
			StartAnimationCommand = new AsyncCommand<View>(v => OnStart(v), (view) => !(SelectedAnimation is null));
			StopAnimationCommand = new Command(OnStop, () => cancellationTokenSource != null);
		}

		async Task OnStart(View? view)
		{
			if (view is null)
			{
				return;
			}

			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}

			currentAnimation = SelectedAnimation!.CreateAnimation(view, (d, b) =>
			{
				StartAnimationCommand.ChangeCanExecute();
				StopAnimationCommand.ChangeCanExecute();
			});

			cancellationTokenSource = new CancellationTokenSource();
			StopAnimationCommand.ChangeCanExecute();

			await currentAnimation.Animate(cancellationTokenSource.Token, view);

			cancellationTokenSource = null;
			StopAnimationCommand.ChangeCanExecute();
		}

		void OnStop()
		{
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
		}
	}
}

