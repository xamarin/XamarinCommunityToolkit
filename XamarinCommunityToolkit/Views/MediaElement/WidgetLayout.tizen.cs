using ElmSharp;
using ELayout = ElmSharp.Layout;

namespace Xamarin.CommunityToolkit.Tizen.UI.Views
{
	public class FormsLayout : ELayout
	{
		public string ThemeClass { get; private set; }
		public string ThemeGroup { get; private set; }
		public string ThemeStyle { get; private set; }

		public FormsLayout(EvasObject parent) : base(parent)
		{
		}

		public new void SetTheme(string klass, string group, string style)
		{
			base.SetTheme(klass, group, style);
			ThemeClass = klass;
			ThemeGroup = group;
			ThemeStyle = style;
		}
	}

	public class WidgetLayout : FormsLayout
	{
		public class Styles
		{
			public const string Default = "default";
		}

		public WidgetLayout(EvasObject parent, string style = Styles.Default) : base(parent) => SetTheme("layout", "elm_widget", style);
	}
}
