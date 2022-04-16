using System;using Microsoft.Extensions.Logging;
using System.Linq;
using Xamarin.CommunityToolkit.Behaviors.Internals;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Internals;

namespace Xamarin.CommunityToolkit.Behaviors
{
	/// <summary>
	/// The <see cref="ImpliedOrderGridBehavior"/> enables you to automatically assign a <see cref="Grid"/> row and column to a view based on the order the view is added to the <see cref="Grid"/>. You only need to setup the row and column definitions and then add children to the <see cref="Grid"/>. You may still assign RowSpan and ColumnSpan to views and their values will be taken into account when assigning a row and column to a view. If a view has a user defined row or column value it will be honored.
	/// </summary>
	public class ImpliedOrderGridBehavior : BaseBehavior<Microsoft.Maui.Controls.Compatibility.Grid>
	{
		bool[][]? usedMatrix;
		int rowCount;
		int columnCount;

		/// <summary>
		/// When set to true, warnings will throw an exception instead of being logged. Defaults to false.
		/// </summary>
		public bool ThrowOnLayoutWarning { get; set; }

		protected override void OnAttachedTo(Microsoft.Maui.Controls.Compatibility.Grid bindable)
		{
			base.OnAttachedTo(bindable);

			bindable.ChildAdded += OnInternalGridChildAdded;
		}

		protected override void OnDetachingFrom(Microsoft.Maui.Controls.Compatibility.Grid bindable)
		{
			base.OnDetachingFrom(bindable);

			bindable.ChildAdded -= OnInternalGridChildAdded;
		}

		void OnInternalGridChildAdded(object? sender, ElementEventArgs e) =>
			ProcessElement(e.Element);

		void LogWarning(string warning)
		{
			(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetService(typeof(ILogger)) as ILogger)?.LogWarning(nameof(ImpliedOrderGridBehavior), warning);
			if (ThrowOnLayoutWarning)
				throw new Exception(warning);
		}

		bool[][] InitMatrix()
		{
			_ = View ?? throw new NullReferenceException($"{nameof(View)} cannot be null.");

			rowCount = View.RowDefinitions.Count;
			if (rowCount == 0)
				rowCount = 1;

			columnCount = View.ColumnDefinitions.Count;
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
				rowIndex = Math.Max(rowCount - 1, 0);
				columnIndex = Math.Max(columnCount - 1, 0);
				return;
			}
			rowIndex = Array.IndexOf(usedMatrix, row);

			// Find the first available column
			columnIndex = Array.IndexOf(row, row.FirstOrDefault(c => !c));
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
			{
				for (var c = column; c < columnEnd; c++)
				{
					if (usedMatrix?[r][c] ?? throw new NullReferenceException())
						LogWarning($"Cell at row {r} column {c} has already been used.");

					usedMatrix[r][c] = true;
				}
			}
		}

		void ProcessElement(BindableObject view)
		{
			var columnSpan = Microsoft.Maui.Controls.Compatibility.Grid.GetColumnSpan(view);
			var rowSpan = Microsoft.Maui.Controls.Compatibility.Grid.GetRowSpan(view);

			FindNextCell(out var row, out var column);

			// Check to see if the user manually assigned a row or column
			if (view.IsSet(Microsoft.Maui.Controls.Compatibility.Grid.ColumnProperty))
				column = Microsoft.Maui.Controls.Compatibility.Grid.GetColumn(view);
			if (view.IsSet(Microsoft.Maui.Controls.Compatibility.Grid.RowProperty))
				row = Microsoft.Maui.Controls.Compatibility.Grid.GetRow(view);

			UpdateUsedCells(row, column, rowSpan, columnSpan);

			// Set attributes
			view.SetValue(Microsoft.Maui.Controls.Compatibility.Grid.ColumnProperty, column);
			view.SetValue(Microsoft.Maui.Controls.Compatibility.Grid.RowProperty, row);
		}
	}
}