using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.CommunityToolkit.Helpers;
using Xunit;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class VariableMultiConverter_Tests
	{
		[Theory]
		[InlineData(new object[] { true, true, true }, MultiBindingCondition.All, true)]
		[InlineData(new object[] { true, false, true }, MultiBindingCondition.All, false)]
		[InlineData(new object[] { false, false, false }, MultiBindingCondition.All, false)]
		[InlineData(new object[] { true, true, true }, MultiBindingCondition.Any, true)]
		[InlineData(new object[] { false, false, false }, MultiBindingCondition.Any, false)]
		[InlineData(new object[] { false, true, false }, MultiBindingCondition.Any, true)]
		[InlineData(new object[] { true, true, true }, MultiBindingCondition.Count, true, 3)]
		[InlineData(new object[] { false, false, false }, MultiBindingCondition.Count, false, 1)]
		[InlineData(new object[] { false, true, false }, MultiBindingCondition.Count, true, 1)]
		[InlineData(null, MultiBindingCondition.All, false)]
		[InlineData(null, MultiBindingCondition.Any, false)]
		[InlineData(null, MultiBindingCondition.Count, false)]
		public void VariableMultiConverter(object[] value, MultiBindingCondition type, object expectedResult, int count = 0)
		{
			var variableMultiConverter = new VariableMultiConverter() { ConditionType = type, Count = count };
			var result = variableMultiConverter.Convert(value, typeof(bool), null, CultureInfo.CurrentCulture);
			Assert.Equal(result, expectedResult);
		}
	}
}
