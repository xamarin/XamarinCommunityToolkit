using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class ImageResourceConverterViewModel : BaseViewModel
	{
		const string img1 = "button.png";
		const string img2 = "logo.png";
		const string imagesPath = "Images";

		string defaultNamespace;

		string? imageName;

		public string? ImageName
		{
			get => imageName;
			set => SetProperty(ref imageName, value);
		}

		ICommand? changeImageCommand;

		public ICommand ChangeImageCommand => changeImageCommand ??= new Command(() =>
			{
				ImageName = (ImageName?.Equals(BuildEmbededImagePath(img1)) ?? false) ?
							BuildEmbededImagePath(img2) :
							BuildEmbededImagePath(img1);
			});

		public ImageResourceConverterViewModel()
		{
			defaultNamespace = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
			ImageName = BuildEmbededImagePath(img1);
		}

		string BuildEmbededImagePath(string imgName)
			=> $"{defaultNamespace}.{imagesPath}.{imgName}";
	}
}