using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin.CommunityToolkit.Sample.Pages.Behaviors
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImpliedOrderGridBehaviorPage
	{
		public ImpliedOrderGridBehaviorPage() => InitializeComponent();

		void ButtonAddRow_OnClicked(object sender, EventArgs e) => TestGrid.RowDefinitions.Add(new RowDefinition());

		void ButtonAddColumn_OnClicked(object sender, EventArgs e) => TestGrid.ColumnDefinitions.Add(new ColumnDefinition());

		void ButtonRemoveRow_OnClicked(object sender, EventArgs e)
		{
			var lastRow = TestGrid.RowDefinitions.Last();
			TestGrid.RowDefinitions.Remove(lastRow);
		}

		void ButtonRemoveColumn_OnClicked(object sender, EventArgs e)
		{
			var lastColumn = TestGrid.ColumnDefinitions.Last();
			TestGrid.ColumnDefinitions.Remove(lastColumn);
		}
	}
}