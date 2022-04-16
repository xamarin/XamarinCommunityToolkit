using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;
using static Microsoft.Maui.Controls.Compatibility.Constraint;
using Bounds = System.Linq.Expressions.Expression<System.Func<Microsoft.Maui.Graphics.Rect>>;
using Expression = System.Linq.Expressions.Expression<System.Func<double>>;
using ParentMeasure = System.Func<Microsoft.Maui.Controls.Compatibility.RelativeLayout, double>;
using ViewMeasure = System.Func<Microsoft.Maui.Controls.Compatibility.RelativeLayout, Microsoft.Maui.Controls.View, double>;

namespace Xamarin.CommunityToolkit.Markup
{
	public static class RelativeLayoutExtensions
	{
		// TODO: Mark obsolete after we implement factory method in MAUI: public static RelativeLayout RelativeLayout(params ConstrainedView?[] constrainedViews) { ... }
		public static TRelativeLayout Children<TRelativeLayout>(this TRelativeLayout layout, params ConstrainedView?[] constrainedViews) where TRelativeLayout : RelativeLayout
		{
			foreach (var constrainedView in constrainedViews)
				constrainedView?.AddTo(layout);
			return layout;
		}
	}

	public static class ConstrainViewExtensions
	{
		public static UnconstrainedView Unconstrained<TView>(this TView view) where TView : View => new UnconstrainedView(view);

		public static ConstraintsConstrainedView Constraints<TView>(this TView view) where TView : View => new ConstraintsConstrainedView(view);

		public static BoundsConstrainedView Constrain<TView>(this TView view, Bounds bounds) where TView : View => new BoundsConstrainedView(view, bounds);

		public static ExpressionsConstrainedView Constrain<TView>(this TView view) where TView : View => new ExpressionsConstrainedView(view);
	}

	public class UnconstrainedView : ConstrainedView
	{
		public UnconstrainedView(View view)
			: base(view) { }

		public override void AddTo(RelativeLayout layout) => ((Layout<View>)layout).Children.Add(view);
	}

	public class BoundsConstrainedView : ConstrainedView
	{
		readonly Bounds bounds;

		public BoundsConstrainedView(View view, Bounds bounds)
			: base(view) => this.bounds = bounds;

		public override void AddTo(RelativeLayout layout) => layout.Children.Add(view, bounds);
	}

	public class ExpressionsConstrainedView : ConstrainedView
	{
		Expression? x, y, width, height;

		public ExpressionsConstrainedView(View view)
			: base(view) { }

		public ExpressionsConstrainedView X(Expression x) { this.x = x; return this; }

		public ExpressionsConstrainedView Y(Expression y) { this.y = y; return this; }

		public ExpressionsConstrainedView Width(Expression width) { this.width = width; return this; }

		public ExpressionsConstrainedView Height(Expression height) { this.height = height; return this; }

		public override void AddTo(RelativeLayout layout) => layout.Children.Add(view, x, y, width, height);
	}

	public class ConstraintsConstrainedView : ConstrainedView
	{
		Constraint? x, y, width, height;

		public ConstraintsConstrainedView(View view)
			: base(view) { }

		public ConstraintsConstrainedView X(double x) { this.x = Constant(x); return this; }

		public ConstraintsConstrainedView Y(double y) { this.y = Constant(y); return this; }

		public ConstraintsConstrainedView Width(double width) { this.width = Constant(width); return this; }

		public ConstraintsConstrainedView Height(double height) { this.height = Constant(height); return this; }

		public ConstraintsConstrainedView X(ParentMeasure x) { this.x = RelativeToParent(x); return this; }

		public ConstraintsConstrainedView Y(ParentMeasure y) { this.y = RelativeToParent(y); return this; }

		public ConstraintsConstrainedView Width(ParentMeasure width) { this.width = RelativeToParent(width); return this; }

		public ConstraintsConstrainedView Height(ParentMeasure height) { this.height = RelativeToParent(height); return this; }

		public ConstraintsConstrainedView X(View view, ViewMeasure x) { this.x = RelativeToView(view, x); return this; }

		public ConstraintsConstrainedView Y(View view, ViewMeasure y) { this.y = RelativeToView(view, y); return this; }

		public ConstraintsConstrainedView Width(View view, ViewMeasure width) { this.width = RelativeToView(view, width); return this; }

		public ConstraintsConstrainedView Height(View view, ViewMeasure height) { this.height = RelativeToView(view, height); return this; }

		public override void AddTo(RelativeLayout layout) => layout.Children.Add(view, x, y, width, height);
	}

	public abstract class ConstrainedView
	{
		protected readonly View view;

		protected ConstrainedView(View view) => this.view = view;

		public abstract void AddTo(RelativeLayout layout);
	}
}