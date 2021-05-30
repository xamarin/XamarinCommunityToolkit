namespace CommunityToolkit.Maui.Sample.ViewModels.Converters
{
	public class IndexToArrayItemConverterViewModel : BaseViewModel
	{
		int index;

		public int Index
		{
			get => index;
			set
			{
				index = value;
				OnPropertyChanged();
			}
		}
	}
}
