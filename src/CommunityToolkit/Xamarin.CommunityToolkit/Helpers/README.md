 
# Xamarin.CommunityToolkit.Helpers Docs

## `DelegateWeakEventManager`

An event implementation that enables the [garbage collector to collect an object without needing to unsubscribe event handlers](http://paulstovell.com/blog/weakevents).

Inspired by [Xamarin.Forms.WeakEventManager](https://github.com/xamarin/Xamarin.Forms/blob/master/Xamarin.Forms.Core/WeakEventManager.cs).

Expands functionality of Xamarin.Forms.WeakEventManager to support `Delegate` events

### Using `EventHandler`

```csharp
readonly WeakEventManager _canExecuteChangedEventManager = new WeakEventManager();

public event EventHandler CanExecuteChanged
{
    add => _canExecuteChangedEventManager.AddEventHandler(value);
    remove => _canExecuteChangedEventManager.RemoveEventHandler(value);
}

void OnCanExecuteChanged() => _canExecuteChangedEventManager.RaiseEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));
```

### Using `Delegate`

```csharp
readonly WeakEventManager _propertyChangedEventManager = new WeakEventManager();

public event PropertyChangedEventHandler PropertyChanged
{
    add => _propertyChangedEventManager.AddEventHandler(value);
    remove => _propertyChangedEventManager.RemoveEventHandler(value);
}

void OnPropertyChanged([CallerMemberName]string propertyName = "") => _propertyChangedEventManager.RaiseEvent(this, new PropertyChangedEventArgs(propertyName), nameof(PropertyChanged));
```

### Using `Action`

```csharp
readonly WeakEventManager _weakActionEventManager = new WeakEventManager();

public event Action ActionEvent
{
    add => _weakActionEventManager.AddEventHandler(value);
    remove => _weakActionEventManager.RemoveEventHandler(value);
}

void OnActionEvent(string message) => _weakActionEventManager.RaiseEvent(message, nameof(ActionEvent));
```

## `WeakEventManager<T>`
An event implementation that enables the [garbage collector to collect an object without needing to unsubscribe event handlers](http://paulstovell.com/blog/weakevents).

Inspired by [Xamarin.Forms.WeakEventManager](https://github.com/xamarin/Xamarin.Forms/blob/master/Xamarin.Forms.Core/WeakEventManager.cs).

### Using `EventHandler<T>`

```csharp
readonly WeakEventManager<string> _errorOcurredEventManager = new WeakEventManager<string>();

public event EventHandler<string> ErrorOcurred
{
    add => _errorOcurredEventManager.AddEventHandler(value);
    remove => _errorOcurredEventManager.RemoveEventHandler(value);
}

void OnErrorOcurred(string message) => _errorOcurredEventManager.RaiseEvent(this, message, nameof(ErrorOcurred));
```

#### Using `Action<T>`

```csharp
readonly WeakEventManager<string> _weakActionEventManager = new WeakEventManager<string>();

public event Action<string> ActionEvent
{
    add => _weakActionEventManager.AddEventHandler(value);
    remove => _weakActionEventManager.RemoveEventHandler(value);
}

void OnActionEvent(string message) => _weakActionEventManager.RaiseEvent(message, nameof(ActionEvent));
```