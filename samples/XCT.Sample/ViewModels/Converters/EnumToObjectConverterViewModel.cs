using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class EnumToObjectConverterViewModel : BaseViewModel
	{
		CameraFlashMode flashMode = CameraFlashMode.Auto;

		public CameraFlashMode FlashMode
		{
			get => flashMode;
			set => SetProperty(ref flashMode, value);
		}

		public ICommand FlashModeCommand { get; }

		public EnumToObjectConverterViewModel() =>
			FlashModeCommand = CommandFactory.Create(ExecuteFlashModeCommand);

		void ExecuteFlashModeCommand()
			=> FlashMode = Next(FlashMode);

		CameraFlashMode Next(CameraFlashMode value)
			=> value switch
		{
			CameraFlashMode.Off => CameraFlashMode.On,
			CameraFlashMode.On => CameraFlashMode.Auto,
			CameraFlashMode.Auto => CameraFlashMode.Torch,
			_ => CameraFlashMode.Off,
		};
	}
}