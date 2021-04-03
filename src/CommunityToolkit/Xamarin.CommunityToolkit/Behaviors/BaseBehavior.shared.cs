﻿using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors.Internals
{
	/// <summary>
	/// Abstract class for our behaviors to inherit.
	/// </summary>
	/// <typeparam name="TView">The <see cref="VisualElement"/> that the behavior can be applied to</typeparam>
	public abstract class BaseBehavior<TView> : Behavior<TView> where TView : VisualElement
	{
		static readonly MethodInfo? getContextMethod
			= typeof(BindableObject).GetRuntimeMethods()?.FirstOrDefault(m => m.Name == "GetContext");

		static readonly FieldInfo? bindingField
			= getContextMethod?.ReturnType.GetRuntimeField("Binding");

		BindingBase? defaultBindingContextBinding;

		protected TView? View { get; private set; }

		internal void BindContextIfNeeded(Binding binding)
		{
			if (!IsBound(BindingContextProperty))
			{
				SetBinding(BindingContextProperty, defaultBindingContextBinding = binding);
			}
		}

		internal void ClearContextIfNeeded()
		{
			if (defaultBindingContextBinding != null)
			{
				RemoveBinding(BindingContextProperty);
				defaultBindingContextBinding = null;
			}
		}

		protected virtual void OnViewPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
		}

		protected override void OnAttachedTo(TView bindable)
		{
			base.OnAttachedTo(bindable);
			View = bindable;
			bindable.PropertyChanged += OnViewPropertyChanged;
			BindContextIfNeeded(new Binding
			{
				Path = BindingContextProperty.PropertyName,
				Source = bindable
			});
		}

		protected override void OnDetachingFrom(TView bindable)
		{
			base.OnDetachingFrom(bindable);
			ClearContextIfNeeded();
			bindable.PropertyChanged -= OnViewPropertyChanged;
			View = null;
		}

		protected bool IsBound(BindableProperty property, BindingBase? defaultBinding = null)
		{
			var context = getContextMethod?.Invoke(this, new object[] { property });
			return context != null
				&& bindingField?.GetValue(context) is BindingBase binding
				&& binding != defaultBinding;
		}
	}
}