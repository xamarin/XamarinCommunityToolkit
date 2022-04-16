using System;using Microsoft.Extensions.Logging;
using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;

namespace Xamarin.CommunityToolkit.ObjectModel
{
	/// <summary>
	/// Factory for Xamarin.Forms.Command
	/// </summary>
	public static partial class CommandFactory
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
		/// <returns>Xamarin.Forms.Command</returns>
		public static Command Create(Action execute, Func<bool> canExecute) =>
			new Command(execute, canExecute);

		/// <summary>
		/// Initializes Xamarin.Forms.Command
		/// </summary>
		/// <param name="execute">The Function executed when Execute is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <returns>Xamarin.Forms.Command</returns>
		public static Command Create(Action<object> execute) =>
			new Command(execute);

		/// <summary>
		/// Initializes Xamarin.Forms.Command
		/// </summary>
		/// <param name="execute">The Function executed when Execute is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <returns>Xamarin.Forms.Command</returns>
		public static Command Create(Action<object> execute, Func<object, bool> canExecute) =>
			new Command(execute, canExecute);

		/// <summary>
		/// Initializes Xamarin.Forms.Command<typeparamref name="T"/>
		/// </summary>
		/// <param name="execute">The Function executed when Execute is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <returns>Xamarin.Forms.Command<typeparamref name="T"/></returns>
		public static Command<T> Create<T>(Action<T> execute) =>
			new Command<T>(execute);

		/// <summary>
		/// Initializes Xamarin.Forms.Command<typeparamref name="T"/>
		/// </summary>
		/// <param name="execute">The Function executed when Execute is called. This does not check canExecute before executing and will execute even if canExecute is false</param>
		/// <returns>Xamarin.Forms.Command<typeparamref name="T"/></returns>
		public static Command<T> Create<T>(Action<T> execute, Func<T, bool> canExecute) =>
			new Command<T>(execute, canExecute);
	}
}