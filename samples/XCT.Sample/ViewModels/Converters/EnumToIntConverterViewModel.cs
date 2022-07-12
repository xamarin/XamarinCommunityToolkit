using System;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class EnumToIntConverterViewModel : BaseViewModel
	{
		public string[] AllStates { get; } = Enum.GetNames(typeof(IssueState));

		IssueState selectedState = IssueState.None;

		public IssueState SelectedState
		{
			get => selectedState;
			set => SetProperty(ref selectedState, value);
		}
	}
}