using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class SegmentedViewModel : BaseViewModel
	{
		public ICommand AddTextCommand { get; }

		public ICommand RemoveTextCommand { get; }

		public IList<string> Options => new List<string>
		{
			"Option A",
			"Option B",
			"Option C"
		};

		public IList<string> IconOptions => new List<string>
		{
			"https://raw.githubusercontent.com/xamarin/Xamarin.Forms/master/Xamarin.Forms.Controls/coffee.png",
			"https://raw.githubusercontent.com/xamarin/Xamarin.Forms/master/Xamarin.Forms.Controls/coffee.png",
			"https://raw.githubusercontent.com/xamarin/Xamarin.Forms/master/Xamarin.Forms.Controls/coffee.png"
		};

		object selectedItem;

		public object SelectedItem
		{
			get => selectedItem;
			set => SetProperty(ref selectedItem, value);
		}

		public SegmentedViewModel()
		{
			AddTextCommand = new Command<int>(AddTextExecuted);
			RemoveTextCommand = new Command<int>(RemoveTextExecuted);
		}

		void AddTextExecuted(int index)
		{
			int i;

			if (index < 0)
				i = 0;
			else if (index > Options.Count + 1)
				i = Options.Count;
			else
				i = index;

			try
			{
				Options.Insert(i, $"New {Options.Count + 1}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		void RemoveTextExecuted(int index)
		{
			try
			{
				Options.RemoveAt(index);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
