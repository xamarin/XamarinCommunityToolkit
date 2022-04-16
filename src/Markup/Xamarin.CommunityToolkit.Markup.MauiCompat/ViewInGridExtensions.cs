using System;
using System.Globalization;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;using Grid = Microsoft.Maui.Controls.Grid;

namespace Xamarin.CommunityToolkit.Markup
{
	public static class ViewInGridExtensions
	{
		public static TView Row<TView>(this TView view, int row) where TView : View
		{
			view.SetValue(Grid.RowProperty, row);
			return view;
		}

		public static TView Row<TView>(this TView view, int row, int span) where TView : View
		{
			view.SetValue(Grid.RowProperty, row);
			view.SetValue(Grid.RowSpanProperty, span);
			return view;
		}

		public static TView RowSpan<TView>(this TView view, int span) where TView : View
		{
			view.SetValue(Grid.RowSpanProperty, span);
			return view;
		}

		public static TView Column<TView>(this TView view, int column) where TView : View
		{
			view.SetValue(Grid.ColumnProperty, column);
			return view;
		}

		public static TView Column<TView>(this TView view, int column, int span) where TView : View
		{
			view.SetValue(Grid.ColumnProperty, column);
			view.SetValue(Grid.ColumnSpanProperty, span);
			return view;
		}

		public static TView ColumnSpan<TView>(this TView view, int span) where TView : View
		{
			view.SetValue(Grid.ColumnSpanProperty, span);
			return view;
		}

		public static TView Row<TView, TRow>(this TView view, TRow row) where TView : View where TRow : Enum
		{
			var rowIndex = row.ToInt();
			view.SetValue(Grid.RowProperty, rowIndex);
			return view;
		}

		public static TView Row<TView, TRow>(this TView view, TRow first, TRow last) where TView : View where TRow : Enum
		{
			var rowIndex = first.ToInt();
			var span = last.ToInt() - rowIndex + 1;
			view.SetValue(Grid.RowProperty, rowIndex);
			view.SetValue(Grid.RowSpanProperty, span);
			return view;
		}

		public static TView Column<TView, TColumn>(this TView view, TColumn column) where TView : View where TColumn : Enum
		{
			var columnIndex = column.ToInt();
			view.SetValue(Grid.ColumnProperty, columnIndex);
			return view;
		}

		public static TView Column<TView, TColumn>(this TView view, TColumn first, TColumn last) where TView : View where TColumn : Enum
		{
			var columnIndex = first.ToInt();
			view.SetValue(Grid.ColumnProperty, columnIndex);

			var span = last.ToInt() + 1 - columnIndex;
			view.SetValue(Grid.ColumnSpanProperty, span);

			return view;
		}

		static int ToInt(this Enum enumValue) => Convert.ToInt32(enumValue, CultureInfo.InvariantCulture);
	}
}