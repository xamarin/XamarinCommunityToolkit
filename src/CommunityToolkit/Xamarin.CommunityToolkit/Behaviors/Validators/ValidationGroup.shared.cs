using System.Collections.Generic;
using Xamarin.CommunityToolkit.Behaviors.Internals;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.Behaviors
{
		[Preserve(AllMembers = true)]
	public class ValidationGroup : BindableObject
	{
		readonly IList<ValidationBehavior> validationBehaviors = new List<ValidationBehavior>();

		public static readonly BindableProperty IsValidProperty = BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(ValidationGroup), false, BindingMode.OneWayToSource);

		/// <summary>
		/// Indicates if group of validations are all valid.
		/// </summary>
		public bool IsValid
		{
			get => (bool)GetValue(IsValidProperty);
			set => SetValue(IsValidProperty, value);
		}

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
		public void Update()
		{
			var isValid = true;

			foreach (var validationItem in validationBehaviors)
			{
				isValid &= validationItem.IsValid;
			}

			IsValid = isValid;
		}
	}
}
