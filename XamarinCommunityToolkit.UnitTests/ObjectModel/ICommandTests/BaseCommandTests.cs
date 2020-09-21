using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UnitTests.Mocks;
using Xamarin.Forms;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests
{
	public abstract class BaseCommandTests
	{
		public const int Delay = 500;

		public BaseCommandTests() => Device.PlatformServices = new MockPlatformServices();

		protected Task NoParameterTask() => Task.Delay(Delay);

		protected Task IntParameterTask(int delay) => Task.Delay(delay);

		protected Task StringParameterTask(string text) => Task.Delay(Delay);

		protected Task NoParameterImmediateNullReferenceExceptionTask() => throw new NullReferenceException();

		protected Task ParameterImmediateNullReferenceExceptionTask(int delay) => throw new NullReferenceException();

		protected async Task NoParameterDelayedNullReferenceExceptionTask()
		{
			await Task.Delay(Delay);
			throw new NullReferenceException();
		}

		protected async Task IntParameterDelayedNullReferenceExceptionTask(int delay)
		{
			await Task.Delay(delay);
			throw new NullReferenceException();
		}

		protected bool CanExecuteTrue(bool parameter) => true;

		protected bool CanExecuteTrue(string parameter) => true;

		protected bool CanExecuteTrue(object parameter) => true;

		protected bool CanExecuteFalse(bool parameter) => false;

		protected bool CanExecuteFalse(string parameter) => false;

		protected bool CanExecuteFalse(object parameter) => false;

		protected bool CanExecuteDynamic(object booleanParameter)
		{
			if (booleanParameter is bool parameter)
				return parameter;

			throw new InvalidCastException();
		}
	}
}