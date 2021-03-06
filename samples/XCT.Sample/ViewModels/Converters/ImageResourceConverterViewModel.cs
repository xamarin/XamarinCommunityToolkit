using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class ImageResourceConverterViewModel : BaseViewModel
	{
		readonly string img1 = "button.png";
		readonly string img2 = "logo.png";
		readonly string imagesPath = "Images";
		string defaultNamespace;

		string imageName;

		public string ImageName
		{
			get => imageName;
			set
			{
				imageName = value;
				OnPropertyChanged();
			}
		}

		ICommand changeImageCommand;

		public ICommand ChangeImageCommand => changeImageCommand ??= new Command(() =>
			{
				ImageName = ImageName.Equals(BuildEmbededImagePath(img1)) ?
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
