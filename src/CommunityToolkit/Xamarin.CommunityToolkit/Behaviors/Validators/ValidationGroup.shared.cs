using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.CommunityToolkit.Behaviors.Internals;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.Behaviors
{
	[Preserve(AllMembers = true)]
	[ContentProperty(nameof(Groups))]
	public class ValidationGroup : BindableObject
	{
		readonly ObservableCollection<ValidationGroup> groups = new ObservableCollection<ValidationGroup>();

		readonly IList<ValidationBehavior> validationBehaviors = new List<ValidationBehavior>();

		public static readonly BindableProperty IsValidProperty = BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(ValidationGroup), false, BindingMode.OneWayToSource);

		public ValidationGroup()
			=> groups.CollectionChanged += OnGroupsCollectionChanged;

		/// <summary>
		/// Indicates if group of validations are all valid.
		/// </summary>
		public bool IsValid
		{
			get => (bool)GetValue(IsValidProperty);
			set => SetValue(IsValidProperty, value);
		}

		public IList<ValidationGroup> Groups => groups;

		public ValidationGroup? ParentGroup { get; set; }

		/// <summary>
		/// Add validation behavior to list of validations for the group.
		/// </summary>
		/// <param name="validationBehavior"></param>
		public void Add(ValidationBehavior validationBehavior) => validationBehaviors.Add(validationBehavior);

		/// <summary>
		/// Remove validation behavior from list of validations for the group.
		/// </summary>
		/// <param name="validationBehavior"></param>
		public void Remove(ValidationBehavior validationBehavior) => validationBehaviors.Remove(validationBehavior);

		/// <summary>
		/// Check if all validations are valid and update IsValid property.
		/// </summary>
		public void Validate()
		{
			var isValid = true;

			foreach (var validationItem in validationBehaviors)
			{
				isValid &= validationItem.IsValid;
			}

			foreach (var group in groups)
			{
				isValid &= group.IsValid;
			}

			IsValid = isValid;

			ParentGroup?.Validate();
		}

		void OnGroupsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.NewItems != null)
			{
				foreach (var child in e.NewItems.OfType<ValidationGroup>())
				{
					child.ParentGroup = this;
				}
			}
		}
	}
}
