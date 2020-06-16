using System;
using Xamarin.Forms;
using XamarinCommunityToolkit.Helpers;
using XamarinCommunityToolkit.Interfaces;

namespace XamarinCommunityToolkit.Behaviors
{
    public class DataChangedBehavior : BaseBehavior<View>
    {
        public static readonly BindableProperty BindingProperty = 
            BindableProperty.Create(nameof(Binding), typeof(object), typeof(DataChangedBehavior), null, propertyChanged: OnValueChanged);
        public static readonly BindableProperty ComparisonConditionProperty = 
            BindableProperty.Create(nameof(ComparisonCondition), typeof(ComparisonCondition), typeof(DataChangedBehavior), ComparisonCondition.Equal, propertyChanged: OnValueChanged);
        public static readonly BindableProperty ValueProperty = 
            BindableProperty.Create(nameof(Value), typeof(object), typeof(DataChangedBehavior), null, propertyChanged: OnValueChanged);

        public object Binding
        {
            get { return (object)GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }

        public ComparisonCondition ComparisonCondition
        {
            get { return (ComparisonCondition)GetValue(ComparisonConditionProperty); }
            set { SetValue(ComparisonConditionProperty, value); }
        }

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Triggered whenever the Binding, ComparisonCondition, or Value properties change
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static async void OnValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            DataChangedBehavior behavior = (DataChangedBehavior)bindable;

            if (behavior.AssociatedObject == null)
            {
                return;
            }

            // If the comparison returns true, execute all the actions declared in XAML.
            if (Compare(behavior.Binding, behavior.ComparisonCondition, behavior.Value))
            {
                foreach (BindableObject item in behavior.Actions)
                {
                    // Set the BindingContext of each action to the BindingContext of the behavior.
                    item.BindingContext = behavior.BindingContext;
                    IAction action = (IAction)item;
                    await action.Execute(bindable, newValue);
                }
            }
        }

        /// <summary>
        /// Compares the leftOperand to the rightOperand
        /// </summary>
        /// <param name="leftOperand">Left operand of the comparison</param>
        /// <param name="operatorType">Operator type</param>
        /// <param name="rightOperand">Right operand of the comparison</param>
        /// <returns></returns>
        private static bool Compare(object leftOperand, ComparisonCondition operatorType, object rightOperand)
        {
            if (leftOperand != null && rightOperand != null)
            {
                // Get the converted right operand based on the type of the leftOperand
                // so the comparison can be done correctly.
                rightOperand = Convert(rightOperand.ToString(), leftOperand.GetType().FullName);
            }

            IComparable leftComparableOperand = leftOperand as IComparable;
            IComparable rightComparableOperand = rightOperand as IComparable;

            if ((leftComparableOperand != null) && (rightComparableOperand != null))
            {
                return EvaluateComparable(leftComparableOperand, operatorType, rightComparableOperand);
            }
            
            // Compare the objects.
            switch (operatorType)
            {
                case ComparisonCondition.Equal:
                    return object.Equals(leftOperand, rightOperand);
                case ComparisonCondition.NotEqual:
                    return !object.Equals(leftOperand, rightOperand);
                case ComparisonCondition.LessThan:
                case ComparisonCondition.LessThanOrEqual:
                case ComparisonCondition.GreaterThan:
                case ComparisonCondition.GreaterThanOrEqual:
                    {
                        if (leftComparableOperand == null && rightComparableOperand == null)
                        {
                            throw new ArgumentException("Invalid left and right operands");
                        }
                        else if (leftComparableOperand == null)
                        {
                            throw new ArgumentException("Invalid left operand");
                        }
                        else
                        {
                            throw new ArgumentException("Invalid right operand");
                        }
                    }
            }

            return false;
        }

        /// <summary>
        /// Converts the string value to its object value based on the reflected type name
        /// </summary>
        /// <param name="value">Object to convert</param>
        /// <param name="destinationTypeFullName">Full type name</param>
        /// <returns></returns>
        public static object Convert(string value, string destinationTypeFullName)
        {
            if (string.IsNullOrWhiteSpace(destinationTypeFullName))
            {
                throw new ArgumentNullException(destinationTypeFullName);
            }

            string scope = GetScope(destinationTypeFullName);

            if (string.Equals(scope, "System", StringComparison.Ordinal))
            {
                if (string.Equals(destinationTypeFullName, typeof(string).FullName, StringComparison.Ordinal))
                {
                    return value;
                }
                else if (string.Equals(destinationTypeFullName, typeof(bool).FullName, StringComparison.Ordinal))
                {
                    return bool.Parse(value);
                }
                else if (string.Equals(destinationTypeFullName, typeof(int).FullName, StringComparison.Ordinal))
                {
                    return int.Parse(value);
                }
                else if (string.Equals(destinationTypeFullName, typeof(double).FullName, StringComparison.Ordinal))
                {
                    return double.Parse(value);
                }
            }

            return null;
        }

        /// <summary>
        /// Compares the leftOperand to the rightOperand using the operatorType
        /// </summary>
        /// <param name="leftOperand">Left operand of the comparison</param>
        /// <param name="operatorType">Operator type</param>
        /// <param name="rightOperand">Right operand of the comparison</param>
        /// <returns></returns>
        private static bool EvaluateComparable(IComparable leftOperand, ComparisonCondition operatorType, IComparable rightOperand)
        {
            int comparison = leftOperand.CompareTo(rightOperand);

            switch (operatorType)
            {
                case ComparisonCondition.Equal:
                    return comparison == 0;
                case ComparisonCondition.NotEqual:
                    return comparison != 0;
                case ComparisonCondition.LessThan:
                    return comparison < 0;
                case ComparisonCondition.LessThanOrEqual:
                    return comparison <= 0;
                case ComparisonCondition.GreaterThan:
                    return comparison > 0;
                case ComparisonCondition.GreaterThanOrEqual:
                    return comparison >= 0;
            }

            return false;
        }

        /// <summary>
        /// Returns the substring from the first character to "."
        /// </summary>
        /// <param name="name">Scope name</param>
        /// <returns>Substring</returns>
        private static string GetScope(string name)
        {
            int indexOfLastPeriod = name.LastIndexOf('.');

            if (indexOfLastPeriod != name.Length - 1)
            {
                return name.Substring(0, indexOfLastPeriod);
            }

            return name;
        }
    }
}
