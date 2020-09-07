using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.Behaviors
{
	public class ImpliedOrderGridBehavior : Behavior<Grid>
	{
		private bool[][] usedMatrix;
		private int rowCount;
		private int columnCount;
		private Grid attachedGrid;

		public bool ThrowOnLayoutWarning { get; set; }

		protected override void OnAttachedTo(Grid bindable)
		{
			base.OnAttachedTo(bindable);

			bindable.ChildAdded += InternalGrid_ChildAdded;
			attachedGrid = bindable;
		}

		protected override void OnDetachingFrom(Grid bindable)
		{
			base.OnDetachingFrom(bindable);

			bindable.ChildAdded -= InternalGrid_ChildAdded;
			attachedGrid = null;
		}

		private void InternalGrid_ChildAdded(object sender, ElementEventArgs e) => 
			ProcessElement(e.Element);


		void LogWarning(string warning)
		{
			Log.Warning(nameof(ImpliedOrderGridBehavior), warning);
			if (ThrowOnLayoutWarning)
				throw new Exception(warning);
		}

		bool[][] InitMatrix()
		{
			rowCount = attachedGrid.RowDefinitions.Count;
			if (rowCount == 0)
				rowCount = 1;
			columnCount = attachedGrid.ColumnDefinitions.Count;
			if (columnCount == 0)
				columnCount = 1;
			var newMatrix = new bool[rowCount][];
			for (var r = 0; r < rowCount; r++)
				newMatrix[r] = new bool[columnCount];
			return newMatrix;
		}

		void FindNextCell(out int rowIndex, out int columnIndex)
		{
			usedMatrix ??= InitMatrix();

			// Find the first available row
			var row = usedMatrix.FirstOrDefault(r => r.Any(c => !c));

			// If no row is found, set cell to origin and log
			if (row == null)
			{
				LogWarning("Defined cells exceeded.");
				rowIndex = rowCount == 0 ? 0 : rowCount - 1;
				columnIndex = columnCount == 0 ? 0 : columnCount - 1;
				return;
			}
			rowIndex = usedMatrix.IndexOf(row);

			// Find the first available column
			columnIndex = row.IndexOf(row.First(c => c == false));
		}

		void UpdateUsedCells(int row, int column, int rowSpan, int columnSpan)
		{
			var rowEnd = row + rowSpan;
			var columnEnd = column + columnSpan;

			if (columnEnd > columnCount)
			{
				columnEnd = columnCount;
				LogWarning($"View at row {row} column {columnEnd} with column span {columnSpan} exceeds the defined grid columns.");
			}

			if (rowEnd > rowCount)
			{
				rowEnd = rowCount;
				LogWarning($"View at row {row} column {columnEnd} with row span {rowSpan} exceeds the defined grid rows.");
			}

			for (var r = row; r < rowEnd; r++)
				for (var c = column; c < columnEnd; c++)
					usedMatrix[r][c] = true;
		}

		void ProcessElement(BindableObject view)
		{
			var columnSpan = Grid.GetColumnSpan(view);
			var rowSpan = Grid.GetRowSpan(view);

			FindNextCell(out var row, out var column);
			UpdateUsedCells(row, column, rowSpan, columnSpan);

			// Set attributes
			view.SetValue(Grid.ColumnProperty, column);
			view.SetValue(Grid.RowProperty, row);
		}
	}
}
