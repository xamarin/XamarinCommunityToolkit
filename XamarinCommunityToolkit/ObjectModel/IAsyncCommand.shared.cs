using System.Threading.Tasks;
using System.Windows.Input;

namespace Xamarin.CommunityToolkit.ObjectModel
{
	public interface IAsyncCommand<in T> : ICommand
	{
		/// <summary>
		/// Allow simultaneous/re-entrant execution of async Command. This is reflected on CanExecute result.
		/// Default value is False.
		/// </summary>
		bool AllowMultipleExecution { get; set; }

		/// <summary>
		/// Executes the Command as a Task
		/// </summary>
		/// <returns>The Task to execute</returns>
		/// <param name="parameter">Data used by the command. If the command does not require data to be passed, this object can be set to null.</param>
		Task ExecuteAsync(T parameter);

		/// <summary>
		/// Raises the CanExecuteChanged event.
		/// </summary>
		void RaiseCanExecuteChanged();
	}

	public interface IAsyncCommand : IAsyncCommand<object>
	{

	}
}