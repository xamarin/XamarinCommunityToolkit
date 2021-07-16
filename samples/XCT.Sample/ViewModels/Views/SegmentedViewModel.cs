using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Sample.ViewModels.Views
{
	public class SegmentedViewModel : BaseViewModel
	{
		public ICommand AddCommand { get; }

		public ICommand RemoveCommand { get; }

		public SegmentMode SegmentMode { get; set; } = SegmentMode.Text;

		public IList<string> Options { get; }

		public IList<string> IconOptions { get; }

		object? selectedItem;

		public object? SelectedItem
		{
			get => selectedItem;
			set => SetProperty(ref selectedItem, value);
		}

		public SegmentedViewModel()
		{
			AddCommand = new Command<int>(AddExecuted);
			RemoveCommand = new Command<int>(RemoveExecuted);

			Options = new ObservableCollection<string>
			{
				"Option 1",
				"Option 2",
				"Option 3"
			};

			IconOptions = new ObservableCollection<string>
			{
				"https://raw.githubusercontent.com/xamarin/Xamarin.Forms/master/Xamarin.Forms.Controls/coffee.png",
				"https://raw.githubusercontent.com/xamarin/Xamarin.Forms/master/Xamarin.Forms.Controls/coffee.png",
				"https://raw.githubusercontent.com/xamarin/Xamarin.Forms/master/Xamarin.Forms.Controls/coffee.png"
			};
		}

		void AddExecuted(int index)
		{
			if (SegmentMode == SegmentMode.Text)
				AddText(index);
			else
				AddIcon(index);
		}

		void RemoveExecuted(int index)
		{
			if (SegmentMode == SegmentMode.Text)
				RemoveText(index);
			else
				RemoveIcon(index);
		}

		void AddText(int index)
		{
			int i;

			if (index < 0)
				i = 0;
			else if (index > Options.Count)
				i = Options.Count;
			else
				i = index;

			try
			{
				Options.Insert(i, $"Option {Options.Count + 1}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		void RemoveText(int index)
		{
			int i;

			if (index < 0)
				i = 0;
			else if (index > Options.Count)
				i = Options.Count - 1;
			else
				i = index;

			try
			{
				Options.RemoveAt(i);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		void AddIcon(int index)
		{
			int i;

			if (index < 0)
				i = 0;
			else if (index > IconOptions.Count)
				i = IconOptions.Count;
			else
				i = index;

			try
			{
				IconOptions.Insert(i, "https://raw.githubusercontent.com/xamarin/Xamarin.Forms/master/Xamarin.Forms.Controls/coffee.png");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		void RemoveIcon(int index)
		{
			int i;

			if (index < 0)
				i = 0;
			else if (index > IconOptions.Count)
				i = IconOptions.Count - 1;
			else
				i = index;

			try
			{
				IconOptions.RemoveAt(i);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
