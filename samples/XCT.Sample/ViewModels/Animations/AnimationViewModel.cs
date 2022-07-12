using System;
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
		AnimationDetailModel selectedAnimation;
		CompoundAnimationBase? currentAnimation;
		CancellationTokenSource? cancellationTokenSource;

		public AnimationViewModel()
		{
			Animations = new ObservableCollection<AnimationDetailModel>
			{
				new AnimationDetailModel("Tada", (view, onFinished) => new TadaAnimation()),
				new AnimationDetailModel("RubberBand", (view, onFinished) => new RubberBandAnimation())
			};

			selectedAnimation = Animations.First();
			StartAnimationCommand = new AsyncCommand<View>(v => OnStart(v), _ => SelectedAnimation is not null);
			StopAnimationCommand = new Command(OnStop, () => cancellationTokenSource is not null);
		}

		public ObservableCollection<AnimationDetailModel> Animations { get; }

		public AsyncCommand<View> StartAnimationCommand { get; }

		public Command StopAnimationCommand { get; }

		public AnimationDetailModel SelectedAnimation
		{
			get => selectedAnimation;
			set => SetProperty(ref selectedAnimation, value);
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

			currentAnimation = SelectedAnimation.CreateAnimation(view, (d, b) =>
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

	public class AnimationDetailModel
	{
		public string Name { get; }

		public Func<View, Action<double, bool>, CompoundAnimationBase> CreateAnimation { get; }

		public AnimationDetailModel(string name, Func<View, Action<double, bool>, CompoundAnimationBase> createAnimation)
		{
			Name = name;
			CreateAnimation = createAnimation;
		}
	}
}