# Maui Compat Steps

The following steps are used to create the MauiCompat libraries

### 1. Create Xamarin.CommunityToolit.MauiCompat Branch From Latest Tag

1. Create new branch from the latest tag:
    ```bash
    git checkout -b Xamarin.CommunityToolkit.MauiCompat-[version] [tag]
    ```
    > **Example:** `git checkout -b Xamarin.CommunityToolkit.MauiCompat-1.3.0-pre2 1.3.0-pre2`
2. Push new branch to github

    ```bash
    git push --set-upstream origin Xamarin.CommunityToolkit.MauiCompat-[version]
    ```
    > **Example:** `git push --set-upstream origin Xamarin.CommunityToolkit.MauiCompat-1.3.0-pre2`

### 2. Run MauiCompat.sh script to migrate Xamarin.CommunityToolkit and create `Xamarin.CommunityToolkit.MauiCompat` and `Xamarin.CommunityToolkit.Markup.MauiCompat` 

```zsh
zsh ./MauiCompat.sh
```

### 3. Add MauiColorExtensions Classes

In .NET MAUI the `Color` object has been changed from a `struct` to an `object`. Because of that, we now need to be aware of passing `null` values on methods that have to do with colors. For instance, the `ToAndroid()` and `ToUIColor()` extension methods that take a .NET MAUI `Color` and convert it into a native platform color. There are multiple ways to solve this, but we chose to overload these extension methods with our own. This method checks if the incoming color is `null`, if it is, we replace the color with `Colors.Transparent` and pass it to the extension method and return its value.

This way we didn't need to edit all the lines with this extension method individually.

#### 3.1 Add `using` Statement to Files Which Reference Original Extension Methods
Now in each file that references the `ToAndroid()` and `ToUIColor()` methods we need to add the `using Xamarin.CommunityToolkit.MauiCompat;` line to make sure that it uses _our_ extension method. There is one catch: you have to declare this using **inside** the namespace declaration so that it will prefer _our_ extension method over the .NET MAUI one. In example:

```cs
// ... Rest omitted for brevity
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
// ... Rest omitted for brevity

[assembly: ExportRenderer(typeof(DrawingView), typeof(DrawingViewRenderer))]

namespace Xamarin.CommunityToolkit.UI.Views
{
	// Make sure this using is here!
	using Xamarin.CommunityToolkit.MauiCompat;

	public class DrawingViewRenderer : ViewRenderer<DrawingView, View> { }
```

### 4. Build projects

```zsh
dotnet build ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat.sln -c Release

dotnet build ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat.sln -c Release
```
