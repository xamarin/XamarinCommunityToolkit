using System;
namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class VariableMultiValueConverterViewModel : BaseViewModel
	{
		bool checkBoxValue1 = false;
		bool checkBoxValue2 = false;
		bool checkBoxValue3 = false;

		public bool CheckBoxValue1
		{
			get => checkBoxValue1;
			set => SetProperty(ref checkBoxValue1, value);
		}

		public bool CheckBoxValue2
		{
			get => checkBoxValue2;
			set => SetProperty(ref checkBoxValue2, value);
		}

		public bool CheckBoxValue3
		{
			get => checkBoxValue3;
			set => SetProperty(ref checkBoxValue3, value);
		}
	}
}
