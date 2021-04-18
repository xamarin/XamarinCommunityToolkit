using System;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.UI.Views;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Converters
{
	public class EnumToListConverterViewModel : BaseViewModel
	{
		List<Type> enumTypes = new List<Type>() { typeof(BadgePosition), typeof(CameraCaptureMode), typeof(CharacterType) };
		Type? selectedItem = null;

		public List<Type> EnumTypes
		{
			get => enumTypes;
			set => SetProperty(ref enumTypes, value);
		}

		public Type? SelectedItem
		{
			get => selectedItem;
			set => SetProperty(ref selectedItem, value);
		}
	}
}