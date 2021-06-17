using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Maui.Controls.Internals;

namespace Xamarin.CommunityToolkit.Markup.MauiCompat.UnitTests
{
    class UnitExpressionSearch : ExpressionVisitor, IExpressionSearch
	{
		List<object>? results;
		Type? targeType;

		public List<T?> FindObjects<T>(Expression expression) where T : class
		{
			results = new List<object>();
			targeType = typeof(T);
			Visit(expression);
			return results.Select(o => o as T).ToList();
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			if (node.Expression is ConstantExpression expression 
				&& node.Member is FieldInfo info)
			{
				var container = expression.Value;
				var value = info.GetValue(container);

				if (targeType?.IsInstanceOfType(value) is true 
					&& results is not null
					&& value is not null)
				{
					results.Add(value);
				}
			}

			return base.VisitMember(node);
		}
	}
}
