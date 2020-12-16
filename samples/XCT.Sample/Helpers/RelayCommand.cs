using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xamarin.CommunityToolkit.Sample
{
    public class RelayCommand : ICommand
    {
        readonly Action execute;
        readonly Func<Task> asyncExecute;

        Func<bool> canExecute;
        int executingCount;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            if (execute == null)
				throw new ArgumentNullException(nameof(execute));
            this.execute = execute;
            this.canExecute = canExecute;
        }

        protected RelayCommand(Func<Task> execute, Func<bool> canExecute = null) // This ctor is protected here and public in a derived class, to allow simple initialization like new RelayCommand(MyMethod) without errors due to ambiguity
        {
            if (execute == null)
				throw new ArgumentNullException(nameof(execute));
            asyncExecute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// If no canExecute deletage is provided, CanExecute allows a single simultaneous command execution
        /// </summary>
        /// <param name="parameter">Ignored; this is the paremeterless command class</param>
        /// <returns></returns>
        public bool CanExecute(object parameter = null)
        {
            try
            {
                return canExecute != null ? canExecute() : executingCount == 0;
            }
			catch (Exception ex)
			{
				XLog.Trace(ex);
				return true;
			}
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        // Asynchronous command handling based on http://stackoverflow.com/a/31595509/6043538
        public async void Execute(object parameter = null)
        {
            var couldExecuteBeforeExecute = CanExecute();
            if (!couldExecuteBeforeExecute)
				return;

            Interlocked.Increment(ref executingCount);
            var couldExecuteDuringExecute = CanExecute();
            if (couldExecuteDuringExecute != couldExecuteBeforeExecute)
				RaiseCanExecuteChanged(); // TODO: test if always raising canexecutechanged does not disturb normal UI element update during a short synchronous command. If so, keep preventing multiple execution but only raise CanExecuteChanged for asynchrounous commands

            try
            {
                if (execute != null)
					execute();
				else
					await asyncExecute();
            }
			catch (Exception ex)
			{
				XLog.Trace(ex);
			}
			finally
            {
                Interlocked.Decrement(ref executingCount);
                var couldExecuteAfterExecute = CanExecute();
                if (couldExecuteAfterExecute != couldExecuteDuringExecute)
					RaiseCanExecuteChanged();
            }
        }
    }

    public class RelayCommandAsync : RelayCommand
    {
        public RelayCommandAsync(Func<Task> execute, Func<bool> canExecute = null)
			: base(execute, canExecute) { } // This ctor is public here and protected in the base class, to allow simple initialization like new RelayCommandAsync(MyMethod) without errors due to ambiguity
    }

    public class RelayCommand<TParameter> : ICommand
    {
        readonly Action<TParameter> execute;
        readonly Func<TParameter, Task> asyncExecute;

        Func<TParameter, bool> canExecute;
        int executingCount;

        public RelayCommand(Action<TParameter> execute, Func<TParameter, bool> canExecute = null)
        {
            if (execute == null)
				throw new ArgumentNullException(nameof(execute));
            this.execute = execute;
            this.canExecute = canExecute;
        }

        protected RelayCommand(Func<TParameter, Task> execute, Func<TParameter, bool> canExecute = null) // This ctor is protected here and public in a derived class, to allow simple initialization like new RelayCommand(MyMethod) without errors due to ambiguity
        {
            if (execute == null)
				throw new ArgumentNullException(nameof(execute));
            asyncExecute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// If no canExecute deletage is provided, CanExecute allows a single simultaneous command execution
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter = null)
        {
            try
            {
                return canExecute != null ? canExecute((TParameter)parameter) : executingCount == 0;
            }
			catch (Exception ex)
			{
				XLog.Trace(ex);
				return true;
            }
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        // Asynchronous command handling based on http://stackoverflow.com/a/31595509/6043538
        public async void Execute(object parameterAsObject)
        {
            var couldExecuteBeforeExecute = CanExecute(parameterAsObject);
            if (!couldExecuteBeforeExecute)
				return;

            Interlocked.Increment(ref executingCount);
            var couldExecuteDuringExecute = CanExecute(parameterAsObject);
            if (couldExecuteDuringExecute != couldExecuteBeforeExecute)
				RaiseCanExecuteChanged(); // TODO: test if always raising canexecutechanged does not disturb normal UI element update during a short synchronous command. If so, keep preventing multiple execution but only raise CanExecuteChanged for asynchrounous commands

            try
            {
                var parameter = (TParameter)parameterAsObject;

                if (execute != null)
                {
                    execute(parameter);
                }
                else
                {
                    await asyncExecute(parameter);
                }
            }
			catch (Exception ex)
			{
				XLog.Trace(ex);
			}
            finally
            {
                Interlocked.Decrement(ref executingCount);
                var couldExecuteAfterExecute = CanExecute(parameterAsObject);
                if (couldExecuteAfterExecute != couldExecuteDuringExecute)
					RaiseCanExecuteChanged();
            }
        }
    }

    public class RelayCommandAsync<TParameter> : RelayCommand<TParameter>
    {
        public RelayCommandAsync(Func<TParameter, Task> execute, Func<TParameter, bool> canExecute = null)
			: base(execute, canExecute) { } // This ctor is public here and protected in the base class, to allow simple initialization like new RelayCommandAsync(MyMethod) without errors due to ambiguity
    }
}