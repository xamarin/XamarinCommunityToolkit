using System;using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;

namespace Xamarin.CommunityToolkit.ObjectModel
{
	public static class ObservableRangeCollectionExtension
	{
		/// <summary>
		/// To be used in collection initializer
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void Add<T>(this ObservableRangeCollection<T> source, IEnumerable<T> collection)
		{
			_ = source ?? throw new ArgumentNullException(nameof(source));

			source.AddRange(collection);
		}
	}
}