using System.Collections.Generic;
using Xamarin.CommunityToolkit.Behaviors.Internals;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Xamarin.CommunityToolkit.Behaviors
{
		[Preserve(AllMembers = true)]
	public class ValidationGroupBehavior : Behavior<View>
	{
		private readonly IList<ValidationBehavior> _validationBehaviors;

		public static readonly BindableProperty IsValidProperty = BindableProperty.Create(nameof(IsValid), typeof(bool), typeof(ValidationGroupBehavior), false, BindingMode.OneWayToSource);

		/// <summary>
		/// Indicates if group of validations are all valid.
		/// </summary>
		public bool IsValid
		{
			get { return (bool)GetValue(IsValidProperty); }
			set { SetValue(IsValidProperty, value); }
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="ValidationGroupBehavior"/> class.
		/// </summary>
		public ValidationGroupBehavior()
		{
			_validationBehaviors = new List<ValidationBehavior>();
		}

		/// <summary>
		/// Add validation behavior to list of validations for the group.
		/// </summary>
		/// <param name="validationBehavior"></param>
		public void Add(ValidationBehavior validationBehavior)
		{
			_validationBehaviors.Add(validationBehavior);
		}

		/// <summary>
		/// Remove validation behavior from list of validations for the group.
		/// </summary>
		/// <param name="validationBehavior"></param>
		public void Remove(ValidationBehavior validationBehavior)
		{
			_validationBehaviors.Remove(validationBehavior);
		}

		/// <summary>
		/// Check if all validations are valid and update IsValid property.
		/// </summary>
		public void Update()
		{
			var isValid = true;

			foreach (var validationItem in _validationBehaviors)
			{
				isValid = isValid && validationItem.IsValid;
			}

			IsValid = isValid;
		}

	}
}
