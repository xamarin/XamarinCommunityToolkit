using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class BaseBehavior : Behavior<View>
	{
		static readonly MethodInfo getContextMethod = typeof(BindableObject).GetRuntimeMethods()?.FirstOrDefault(m => m.Name == "GetContext");

		static readonly FieldInfo bindingField = getContextMethod?.ReturnType.GetRuntimeField("Binding");

		BindingBase defaultBindingContextBinding;

		protected View View { get; private set; }

		protected virtual void OnViewPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
		}

		protected override void OnAttachedTo(View bindable)
		{
			base.OnAttachedTo(bindable);
			View = bindable;
			bindable.PropertyChanged += OnViewPropertyChanged;

			if (!IsBound(BindingContextProperty))
			{
				defaultBindingContextBinding = new Binding
				{
					Path = BindingContextProperty.PropertyName,
					Source = bindable
				};
				SetBinding(BindingContextProperty, defaultBindingContextBinding);
			}
		}

		protected override void OnDetachingFrom(BindableObject bindable)
		{
			base.OnDetachingFrom(bindable);

			if (defaultBindingContextBinding != null)
			{
				RemoveBinding(BindingContextProperty);
				defaultBindingContextBinding = null;
			}

			bindable.PropertyChanged -= OnViewPropertyChanged;
			View = null;
		}

		protected bool IsBound(BindableProperty property, BindingBase defaultBinding = null)
		{
			var context = getContextMethod?.Invoke(this, new object[] { property });
			return context != null
				&& bindingField?.GetValue(context) is BindingBase binding
				&& binding != defaultBinding;
		}
	}
}