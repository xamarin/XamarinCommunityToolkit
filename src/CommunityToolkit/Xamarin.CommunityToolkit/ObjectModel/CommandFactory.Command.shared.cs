using System;
using System.Reflection;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.ObjectModel
{
	/// <summary>
	/// Factory for Xamarin.Forms.Command
	/// </summary>
	public partial class CommandFactory
	{
		/// <summary>
		/// Initializes Xamarin.Forms.Command
		/// </summary>
		/// <param name="execute">The Function executed when Execute is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <returns>Xamarin.Forms.Command</returns>
		public static Command Create(Action execute) =>
			new Command(execute);

		/// <summary>
		/// Initializes Xamarin.Forms.Command
		/// </summary>
		/// <param name="execute">The Function executed when Execute is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not Command should execute.</param>
		/// <returns>Xamarin.Forms.Command</returns>
		public static Command Create(Action execute, Func<object, bool> canExecute) =>
			new Command(ConvertExecute(execute), canExecute);

		/// <summary>
		/// Initializes Xamarin.Forms.Command
		/// </summary>
		/// <param name="execute">The Function executed when Execute is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not Command should execute.</param>
		/// <returns>Xamarin.Forms.Command</returns>
		public static Command Create(Action execute, Func<bool> canExecute) =>
			new Command(execute, canExecute);

		/// <summary>
		/// Initializes Xamarin.Forms.Command<typeparamref name="TExecute"/>
		/// </summary>
		/// <param name="execute">The Function executed when Execute is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <returns>Xamarin.Forms.Command<typeparamref name="TExecute"/></returns>
		public static Command<TExecute> Create<TExecute>(Action<TExecute> execute) =>
			new Command<TExecute>(execute);

		/// <summary>
		/// Initializes Xamarin.Forms.Command
		/// </summary>
		/// <param name="execute">The Function executed when Execute is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not Command should execute.</param>
		/// <returns>Xamarin.Forms.Command</returns>
		public static Command Create<TExecute>(Action<TExecute> execute, Func<object, bool> canExecute) =>
			new Command(ConvertExecute(execute), canExecute);

		/// <summary>
		/// Initializes Xamarin.Forms.Command
		/// </summary>
		/// <param name="execute">The Function executed when Execute is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not Command should execute.</param>
		/// <returns>Xamarin.Forms.Command</returns>
		public static Command Create<TExecute>(Action<TExecute> execute, Func<bool> canExecute) =>
			new Command(ConvertExecute(execute), ConvertCanExecute(canExecute));

		/// <summary>
		/// Initializes Xamarin.Forms.Command
		/// </summary>
		/// <param name="execute">The Function executed when Execute is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <param name="canExecute">The Function that verifies whether or not Command should execute.</param>
		/// <returns>Xamarin.Forms.Command</returns>
		public static Command Create<TExecute, TCanExecute>(Action<TExecute> execute, Func<TCanExecute, bool> canExecute) =>
			new Command(ConvertExecute(execute), ConvertCanExecute(canExecute));

		#region Helper methods to ensure Command behaviour is consistent with other commands

		static Action<object> ConvertExecute(Action execute)
		{
			if (execute == null)
				return null;

			return p => execute();
		}

		static Action<object> ConvertExecute<T>(Action<T> execute)
		{
			if (execute == null)
				return null;

			return p => Execute(execute, p);
		}

		static void Execute<T>(Action<T> execute, object parameter)
		{
			switch (parameter)
			{
				case T validParameter:
					execute(validParameter);
					break;

				case null when !typeof(T).GetTypeInfo().IsValueType:
					execute((T)parameter);
					break;

				case null:
				default:
					return;
			}
		}

		static Func<object, bool> ConvertCanExecute(Func<bool> canExecute)
		{
			if (canExecute == null)
				return null;

			return _ => canExecute();
		}

		static Func<object, bool> ConvertCanExecute<T>(Func<T, bool> canExecute)
		{
			if (canExecute == null)
				return null;

			return p => CanExecute(canExecute, p);
		}

		static bool CanExecute<T>(Func<T, bool> canExecute, object parameter) => parameter switch
		{
			T validParameter => canExecute(validParameter),
			null when !typeof(T).GetTypeInfo().IsValueType => canExecute((T)parameter),
			_ => false,
		};

		#endregion
	}
}
