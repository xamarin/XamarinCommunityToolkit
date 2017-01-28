# CapitalizeKeyboardEffect

The **CapitalizeKeyboardEffect** enforces caps on the keyboard for an Entry control.

## Syntax

You can either use the blur behavior from your XAML code:

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

## Example Image

iOS

![CapitalizeKeyboardEffect animation](../Media/ClearEntryAndAllCaps_thumb.gif "CapitalizeKeyboardEffect")

Android 

![CapitalizeKeyboardEffect animation](../Media/AndroidEffects_thumb.gif "CapitalizeKeyboardEffect")

## API source code

* [Android source code](https://github.com/FormsCommunityToolkit/Effects/blob/master/src/Effects.Android/Effects/CapitalizeKeyboardEffect.cs)
* [iOS source code](https://github.com/FormsCommunityToolkit/Effects/blob/master/src/Effects.iOS/Effects/CapitalizeKeyboardEffect.cs)
* [UWP source code](https://github.com/FormsCommunityToolkit/Effects/blob/master/src/Effects.UWP/Effects/CapitalizeKeyboardEffect.cs)
