# CapitalizeKeyboardEffect

The **CapitalizeKeyboardEffect** enforces caps on the keyboard for an Entry control.

## Syntax

You can either use the capitalize effect from your XAML code:

```xaml

    <Entry Placeholder="start typing..." VerticalOptions="Start">
      <Entry.Effects>
        <effects:CapitalizeKeyboardEffect />
      </Entry.Effects>
    </Entry>

```

or directly from code:

```csharp

    MyEntry.Effects.Add(Effect.Resolve("FormsCommunityToolkit.Effects.CapitalizeKeyboardEffect"));

```

[CapitalizeKeyboardEffect Sample Page Source](https://github.com/FormsCommunityToolkit/Effects/blob/master/samples/Effects.SampleApp/Views/EntryPage.xaml)

## Properties

| Property Name | Type | Description |
| --- | --- | --- |
| Value | double | Some property description |

## Example Image

iOS

![CapitalizeKeyboardEffect animation](../Media/ClearEntryAndAllCaps_thumb.gif "CapitalizeKeyboardEffect")

Android 

![CapitalizeKeyboardEffect animation](../Media/AndroidEffects_thumb.gif "CapitalizeKeyboardEffect")

## Requirements (Windows 10 Device Family)

| [Device family](http://go.microsoft.com/fwlink/p/?LinkID=526370) | Universal, [Windows 10 Anniversary SDK 14393](https://blogs.windows.com/windowsexperience/2016/07/18/build14393/) 10.0.14393.0 |
| --- | --- |
| Namespace | Microsoft.Toolkit.Uwp.UI.Animations |

## API source code

* [Android source code](https://github.com/FormsCommunityToolkit/Effects/blob/master/src/Effects.Android/Effects/CapitalizeKeyboardEffect.cs)
* [iOS source code](https://github.com/FormsCommunityToolkit/Effects/blob/master/src/Effects.iOS/Effects/CapitalizeKeyboardEffect.cs)
* [UWP source code](https://github.com/FormsCommunityToolkit/Effects/blob/master/src/Effects.UWP/Effects/CapitalizeKeyboardEffect.cs)
