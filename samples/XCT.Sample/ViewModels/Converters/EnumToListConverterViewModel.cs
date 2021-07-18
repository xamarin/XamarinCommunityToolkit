using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public enum CustomEnum
	{
		A,
		B,
		C,
		D,
		E,
		F
	}

	public class EnumToListConverterViewModel : BaseViewModel
	{
		List<Type> enumTypes;
		Type? selectedEnumType;
		object? selectedItem = null;

		public EnumToListConverterViewModel()
		{
			enumTypes = new List<Type>() { typeof(CustomEnum), typeof(BadgePosition), typeof(CameraCaptureMode), typeof(CharacterType) };
			selectedEnumType = enumTypes.FirstOrDefault();
		}

		public List<Type> EnumTypes
		{
			get => enumTypes;
			set => SetProperty(ref enumTypes, value);
		}

		public Type? SelectedEnumType
		{
			get => selectedEnumType;
			set => SetProperty(ref selectedEnumType, value);
		}

		public object? SelectedItem
		{
			get => selectedItem;
			set => SetProperty(ref selectedItem, value);
		}
	}
}