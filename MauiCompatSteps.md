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

### 3. Manually Update TypeConverters

In .NET 6, `System.ComponentModel.TypeConverter` requires us to override `ConvertFrom` which uses `object value` as a parameter. 

Previously, in .NET Standard we used `ConvertFromInvariantString(string value)`.

To create the MauiCompat library, we must update the logic in each converter to handle an object instead of a string.

Start by adding the following line:

```cs
if (valueObject is not string value)
    throw new InvalidOperationException("Only typeof(string) allowed");
```

- [ ] FileMediaSourceConverter.shared.cs
- [ ] MediaSourceConverter.shared.cs
- [ ] SafeAreaTypeConverter.shared.cs
- [ ] UriTypeConverter.shared.cs

### 4. Manually Update `IStreamImageConverter`

`Microsoft.Maui.IStreamImageConverter` adds a new API, `public bool IsEmpty { get; }`.

Add the following code to implement the new API:

```cs
public bool IsEmpty => Stream == null;
```

- [ ] StreamMediaSource.shared.cs

### 5. Add MauiColorExtensions Classes

In .NET MAUI the `Color` object has been changed from a `struct` to an `object`. Because of that, we now need to be aware of passing `null` values on methods that have to do with colors. For instance, the `ToAndroid()` and `ToUIColor()` extension methods that take a .NET MAUI `Color` and convert it into a native platform color. There are multiple ways to solve this, but we chose to overload these extension methods with our own. This method checks if the incoming color is `null`, if it is, we replace the color with `Colors.Transparent` and pass it to the extension method and return its value.

This way we didn't need to edit all the lines with this extension method individually.

#### 5.1 Create `MauiColorExtensions.android.cs`
The code looks like underneath.

```cs
using AColor = Android.Graphics.Color;

namespace Xamarin.CommunityToolkit.MauiCompat
{
	public static partial class MauiColorExtensions
	{	
		public static AColor ToAndroid(this Microsoft.Maui.Graphics.Color self)
		{
			var colorToConvert = self;

			if (colorToConvert == null)
			{
				colorToConvert = Microsoft.Maui.Graphics.Colors.Transparent;
			}

			return Microsoft.Maui.Controls.Compatibility.Platform.Android.ColorExtensions.ToAndroid(colorToConvert);
		}
	}
}
```

#### 5.2 Create `MauiColorExtensions.ios.cs`
The code looks like underneath.

```cs
using iColor = UIKit.UIColor;

namespace Xamarin.CommunityToolkit.MauiCompat
{
	public static partial class MauiColorExtensions
	{
		public static iColor ToUIColor(this Microsoft.Maui.Graphics.Color self)
		{
			var colorToConvert = self;

			if (colorToConvert == null)
			{
				colorToConvert = Microsoft.Maui.Graphics.Colors.Transparent;
			}

			return Microsoft.Maui.Controls.Compatibility.Platform.iOS.ColorExtensions.ToUIColor(colorToConvert);
		}
	}
}

```

#### 5.3 Add `using` Statement to Files Which Reference Original Extension Methods
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

### 6. Build projects

```zsh
dotnet build ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat.sln -c Release

dotnet build ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat.sln -c Release
```
