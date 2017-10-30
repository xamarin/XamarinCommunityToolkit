
## CheckBox

A really simple implementation of a Checkbox control for Xamarin.Forms.

### SetUp

#### iOS

Make sure to call `FormsCommunityToolkit.Controls.iOS.Controls.Init();` right after `Xamarin.Forms.Init();` in your projects. Look a tht `AppDelegate.cs` code snippet from the sample app:

```
global::Xamarin.Forms.Forms.Init();
FormsCommunityToolkit.Controls.iOS.Controls.Init();
```

### Usage

#### In code
##### Creating Checkboxes
```
var cb1 = new Checkbox();
var cb2 = new Checkbox() { IsEnabled = false };
var cb3 = new Checkbox() { IsChecked = true };
```

##### Assign events
```
cb1.CheckedChanged +=
    (sender, e) =>
    {
        cb2.IsEnabled = e.IsChecked;
        cb3.IsChecked = !cb1.IsChecked;
    };
```

#### XAML
First, pull in the namespace:
```
xmlns:controls="clr-namespace:FormsCommunityToolkit.Controls;assembly=FormsCommunityToolkit.Controls"
```

Then, use it
```
<controls:CheckBox IsChecked="{Binding Check1}" />
<controls:CheckBox IsChecked="{Binding Check2}" IsEnabled="{Binding Check1}" />
<controls:CheckBox IsChecked="{Binding Check1, Mode=OneWay, Converter={StaticResource BoolInverter}}" />
<controls:CheckBox IsVisible="{Binding Check2}" />
```
As you can see, the `IsChecked` property is bindable.
