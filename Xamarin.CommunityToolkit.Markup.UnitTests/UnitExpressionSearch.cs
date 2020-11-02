using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.Markup.UnitTests
{
	class UnitExpressionSearch : ExpressionVisitor, IExpressionSearch
	{
		List<object> results;
		Type targeType;
		public List<T> FindObjects<T>(Expression expression) where T : class
		{
			results = new List<object>();
			targeType = typeof(T);
			Visit(expression);
			return results.Select(o => o as T).ToList();
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			if (node.Expression is ConstantExpression && node.Member is FieldInfo)
			{
				var container = ((ConstantExpression)node.Expression).Value;
				var value = ((FieldInfo)node.Member).GetValue(container);

				if (targeType.IsInstanceOfType(value))
				{
					results.Add(value);
				}
			}
			return base.VisitMember(node);
		}
	}
}
