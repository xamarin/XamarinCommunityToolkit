﻿using System;
using System.Threading.Tasks;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncValueCommandTests
{
	public abstract class BaseAsyncValueCommandTests : BaseCommandTests
	{
		protected new ValueTask NoParameterTask() => ValueTaskDelay(Delay);

		protected new ValueTask IntParameterTask(int delay) => ValueTaskDelay(delay);

		protected new ValueTask StringParameterTask(string? text) => ValueTaskDelay(Delay);

		protected new ValueTask NoParameterImmediateNullReferenceExceptionTask() => throw new NullReferenceException();

		protected new ValueTask ParameterImmediateNullReferenceExceptionTask(int delay) => throw new NullReferenceException();

		protected new async ValueTask NoParameterDelayedNullReferenceExceptionTask()
		{
			await Task.Delay(Delay);
			throw new NullReferenceException();
		}

		protected new async ValueTask IntParameterDelayedNullReferenceExceptionTask(int delay)
		{
			await Task.Delay(delay);
			throw new NullReferenceException();
		}

		ValueTask ValueTaskDelay(int delay) => new ValueTask(Task.Delay(delay));
	}
}