using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.CommunityToolkit.Behaviors.Internals;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.Behaviors
{
	[ContentProperty(nameof(Children))]
	public class MultiValidationBehavior : ValidationBehavior
	{
		public static readonly BindableProperty ErrorsProperty =
			BindableProperty.Create(nameof(Errors), typeof(List<object>), typeof(MultiValidationBehavior), null, BindingMode.OneWayToSource);

		public static readonly BindableProperty ErrorProperty =
			BindableProperty.CreateAttached(nameof(GetError), typeof(object), typeof(MultiValidationBehavior), null);

		readonly ObservableCollection<ValidationBehavior> children
			= new ObservableCollection<ValidationBehavior>();

		public MultiValidationBehavior()
			=> children.CollectionChanged += OnChildrenCollectionChanged;

		public List<object> Errors
		{
			get => (List<object>)GetValue(ErrorsProperty);
			set => SetValue(ErrorsProperty, value);
		}

		public IList<ValidationBehavior> Children
			=> children;

		public static object GetError(BindableObject bindable)
			=> bindable.GetValue(ErrorProperty);

		public static void SetError(BindableObject bindable, object value)
			=> bindable.SetValue(ErrorProperty, value);

		protected override bool Validate(object value)
		{
			var errors = children.Where(c =>
			{
				c.Value = value;
				c.ForceValidate();
				return !c.IsValid;
			}).Select(c => GetError(c));

			if (!errors.Any())
			{
				Errors = null;
				return true;
			}

			if (!Errors?.SequenceEqual(errors) ?? true)
				Errors = errors.ToList();

			return false;
		}

		void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var child in e.NewItems.OfType<ValidationBehavior>())
				{
					child.SetBinding(BindingContextProperty, new Binding
					{
						Path = BindingContextProperty.PropertyName,
						Source = this
					});
				}
			}

			if (e.OldItems != null)
			{
				foreach (var child in e.OldItems.OfType<ValidationBehavior>())
					child.RemoveBinding(BindingContextProperty);
			}
		}
	}
}