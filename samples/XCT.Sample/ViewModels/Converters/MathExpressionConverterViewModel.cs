using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class MathExpressionConverterViewModel : BaseViewModel
	{
		string variableX0Text;
		string variableX1Text;
		string variableX2Text;

		public string VariableX0Text
		{
			get => variableX0Text;
			set
			{
				if (SetProperty(ref variableX0Text, value))
				{
					OnPropertyChanged(nameof(X0));
				}
			}
		}

		public string VariableX1Text
		{
			get => variableX1Text;
			set
			{
				if (SetProperty(ref variableX1Text, value))
				{
					OnPropertyChanged(nameof(X1));
				}
			}
		}

		public string VariableX2Text
		{
			get => variableX2Text;
			set
			{
				if (SetProperty(ref variableX2Text, value))
				{
					OnPropertyChanged(nameof(X2));
				}
			}
		}

		public double X0 => double.TryParse(variableX0Text, out var result) ? result : 0d;
		public double X1 => double.TryParse(variableX1Text, out var result) ? result : 0d;
		public double X2 => double.TryParse(variableX2Text, out var result) ? result : 0d;

		public MathExpressionConverterViewModel()
		{
			variableX0Text = "5";
			variableX1Text = "-2";
			variableX2Text = "1.5";
		}
	}
}