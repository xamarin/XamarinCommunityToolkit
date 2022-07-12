using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.Mocks
{
	public class MockView : View
	{
		public IDictionary<string, IList<object?>> ValuesSet { get; } = new Dictionary<string, IList<object?>>();

		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName is null)
			{
				throw new ArgumentNullException(nameof(propertyName));
			}

			var value = typeof(MockView).GetProperty(propertyName)?.GetValue(this);

			if (!ValuesSet.ContainsKey(propertyName))
			{
				ValuesSet[propertyName] = new List<object?>();
			}

			ValuesSet[propertyName].Add(value);
		}
	}
}