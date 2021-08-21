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

### 2. Create `Xamarin.CommunityToolkit.MauiCompat` and `Xamarin.CommunityToolkit.Markup.MauiCompat` 

1. Create `Xamarin.CommunityToolkit.MauiCompat`:
    ```bash
    dotnet new maui -o ./src/CommunityToolkit -n Xamarin.CommunityToolkit.MauiCompat
    ```

2. Create `Xamarin.CommunityToolkit.Markup.MauiCompat`:
    ```bash
    dotnet new maui -o ./src/Markup -n Xamarin.CommunityToolkit.Markup.MauiCompat
    ```

3. Remove `Xamarin.CommunityToolkit.MauiCompat.WinUI`
    > **Note:** This is a temporary workaround until we add WinUI support

    ```bash
    dotnet sln ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat.sln remove ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat.WinUI
    ```

4. Remove `Xamarin.CommunityToolkit.MauiCompat.WinUI`
    > **Note:** This is a temporary workaround until we add WinUI support

    ```bash
    dotnet sln ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat.sln remove ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat.WinUI
    ```

### 3. Update MauiCompat CSPROJ Files
1. Remove `InvariantGlobalization`, `RuntimeIdentifier`, `UseInterpreter`, and `OutputType` from `Xamarin.CommunityToolkit.MauiCompat.csproj`:
    ```bash
    awk '!/InvariantGlobalization/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj > temp && mv temp ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj
    awk '!/RuntimeIdentifier/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj > temp && mv temp ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj
    awk '!/UseInterpreter/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj > temp && mv temp ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj
    awk '!/OutputType/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj > temp && mv temp ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj
    ```

2. Remove `InvariantGlobalization`, `RuntimeIdentifier`, `UseInterpreter`, and `OutputType` from `Xamarin.CommunityToolkit.Markup.MauiCompat.csproj`:
    ```bash
    awk '!/InvariantGlobalization/' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj > temp && mv temp ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj
    awk '!/RuntimeIdentifier/' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj > temp && mv temp ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj
    awk '!/UseInterpreter/' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj > temp && mv temp ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj
    awk '!/OutputType/' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj > temp && mv temp ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj
    ```

## 4. Create `Directory.build.props`

1. Create `Directory.build.props` for `Xamarin.CommunityToolkit.MauiCompat`
    ```bash
    printf > ./src/CommunityToolkit/Directory.build.props "<Project>
        <PropertyGroup>
            <Nullable>enable</Nullable>
            <PackageId>Xamarin.CommunityToolkit.MauiCompat</PackageId>
            <Summary>A .NET MAUI Comapatible version of Xamarin.CommunityToolkit, a community-created toolkit with common Xamarin converters, effects, behaviors etc.</Summary>
            <PackageTag>maui,net,xamarin,ios,android,uwp,xamarin.forms,effects,controls,converters,animations,toolkit,kit,communitytoolkit,xamarincommunitytoolkit,watchos,tvos,tizen,Microsoft.Toolkit.Xamarin.Forms</PackageTag>
            <Title>Xamarin.CommunityToolkit.MauiCompat</Title>
            <Description>Xamarin.CommunityToolkit.MauiCompat is a collection of Animations, Behaviors, Converters, and Effects for mobile development with .NET MAUI. It is the .NET MAUI Compatible version of Xamarin.CommunityToolkit.</Description>
            <PackageIcon>icon.png</PackageIcon>
            <PackageVersion>$(Version)$(VersionSuffix)</PackageVersion>
            <Authors>Microsoft</Authors>
            <Owners>microsoft</Owners>
            <NeutralLanguage>en</NeutralLanguage>
            <Copyright>© Microsoft Corporation. All rights reserved.</Copyright>
            <RepositoryUrl>https://github.com/xamarin/XamarinCommunityToolkit</RepositoryUrl>
            <PackageReleaseNotes>See: http://aka.ms/xct-release-notes</PackageReleaseNotes>
            <DefineConstants>$(DefineConstants);</DefineConstants>
            <UseFullSemVerForNuGet>false</UseFullSemVerForNuGet>
            <PackageLicenseExpression>MIT</PackageLicenseExpression>
            <PackageRequireLicenseAcceptance>true</PackageRequireLicenseAcceptance>
            <PackageProjectUrl>https://github.com/xamarin/XamarinCommunityToolkit</PackageProjectUrl>
            <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
        </PropertyGroup>    

        <ItemGroup>
            <Compile Include=\"**/*.shared.cs\" />
            <Compile Include=\"**/*.shared.*.cs\" />    
            <None Include=\"../../LICENSE\" PackagePath=\"\" Pack=\"true\" />
            <None Include=\"../../assets/XamarinCommunityToolkit_128x128.png\" PackagePath=\"icon.png\" Pack=\"true\" />
        </ItemGroup>

        <ItemGroup Condition=\" \$(TargetFramework.Contains(-android)) \">
            <Compile Include=\"**\*.android.cs\" />
            <Compile Include=\"**\*.android.*.cs\" />
        </ItemGroup>

        <ItemGroup Condition=\" \$(TargetFramework.Contains(-ios)) \">
            <Compile Include=\"**\*.ios.cs\" />
            <Compile Include=\"**\*.ios.*.cs\" />
        </ItemGroup>

        <ItemGroup Condition=\" \$(TargetFramework.Contains('-windows')) \">
            <Compile Include=\"**\*.uwp.cs\" />
            <Compile Include=\"**\*.uwp.*.cs\" />
        </ItemGroup>

        <ItemGroup Condition=\" \$(TargetFramework.Contains('-maccatalyst')) \">
            <Compile Include=\"**\*.macos.cs\" />
            <Compile Include=\"**\*.macos.*.cs\" />
        </ItemGroup>
    </Project>"
    ```

2. Create `Directory.build.props` for `Xamarin.CommunityToolkit.Markup.MauiCompat`
    ```bash
    printf > ./src/Markup/Directory.build.props "<PropertyGroup>
        <Nullable>enable</Nullable>
        <PackageId>Xamarin.CommunityToolkit.Markup.MauiCompat</PackageId>
        <Summary>A .NET MAUI-compatible community-created toolkit with C# Markup classes and fluent helper methods</Summary>
        <Authors>Microsoft</Authors>
        <Owners>Microsoft</Owners>
        <NeutralLanguage>en</NeutralLanguage>
        <Copyright>© Microsoft Corporation. All rights reserved.</Copyright> 
        <PackageLicenseExpression>MIT</PackageLicenseExpression> 
        <PackageProjectUrl>https://github.com/xamarin/XamarinCommunityToolkit</PackageProjectUrl> 
        <RepositoryUrl>https://github.com/xamarin/XamarinCommunityToolkit</RepositoryUrl>
        <PackageReleaseNotes>See: http://aka.ms/xct-release-notes</PackageReleaseNotes>
        <DefineConstants>$(DefineConstants);</DefineConstants>
        <UseFullSemVerForNuGet>false</UseFullSemVerForNuGet>
        <Title>Xamarin.CommunityToolkit.Markup.MauiCompat</Title>
        <Description>Xamarin Community Toolkit Markup MauiCompat is a set of fluent helper methods and classes to simplify building declarative .NET MAUI user interfaces in C#</Description>
        <PackageIcon>icon.png</PackageIcon>
        <Product>$(AssemblyName) ($(TargetFramework))</Product>
        <PackageVersion>$(Version)$(VersionSuffix)</PackageVersion>
        <PackageRequireLicenseAcceptance>true</PackageRequireLicenseAcceptance> 
        <PackageTags>maui,net,xamarin,xamarin.forms,toolkit,kit,communitytoolkit,xamarincommunitytoolkit,markup,csharpformarkup,csharp,csharpmarkup</PackageTags>
    </PropertyGroup>"
    ```

### 4. Add `Xamarin.CommunityToolkit` files to `Xamarin.CommunityToolkit.MauiCompat`

1. Remove `Xamarin.CommunityToolkit.MauiCompat` Template Files, except `.csproj`
    ```bash
    find ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/ -name "*" ! -name "*.csproj" -delete
    ```

2. Remove `Xamarin.CommunityToolkit.Markup.MauiCompat` Template Files, except `.csproj`
    ```bash
    find ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/ -name "*" ! -name "*.csproj" -delete
    ```

3. Copy `Xamarin.CommunityToolkit` Files to `Xamarin.CommunityToolkit.MauiCompat`
    ```bash
    rsync -avr --exclude='*.csproj' --exclude='bin' --exclude='obj' ./src/CommunityToolkit/Xamarin.CommunityToolkit/ ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/
    ```

4. Copy `Xamarin.CommunityToolkit.Markup` Files to `Xamarin.CommunityToolkit.Markup.MauiCompat`
    ```bash
    rsync -avr --exclude='*.csproj' --exclude='bin' --exclude='obj' ./src/Markup/Xamarin.CommunityToolkit.Markup/ ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/ 
    ```

### 5. Update Files

```bash
# Preserve

sed -i '' 's/\[Preserve(/\[Microsoft.Maui.Controls.Internals.Preserve(/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# Internals

sed -i '' 's/using Xamarin.Forms.Internals/using Microsoft.Maui.Controls.Internals/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# WeakEventManager

sed -i '' 's/ Forms.WeakEventManager/ Microsoft.Maui.Controls.WeakEventManager/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# Colors

sed -i '' 's/ Color\./ Colors./g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\tColor\./\tColors./g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/Forms\.Color/Color/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/Colors\.From/Color\.From/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/Xamarin.Forms.Color/Microsoft.Maui.Graphics.Color/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/Colors.Default/default(Color)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# Nullability

sed -i '' 's/event EventHandler<VisualElementChangedEventArgs>? ElementChanged/event EventHandler<VisualElementChangedEventArgs> ElementChanged/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# PlatformEffect

PlatformEffect

sed -i '' 's/: Xamarin.Forms.Platform.iOS.PlatformEffect/: Microsoft.Maui.Controls.Platform.PlatformEffect/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/: PlatformEffect/: Microsoft.Maui.Controls.Platform.PlatformEffect/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# Platforms

sed -i '' 's/if MONOANDROID10_0/if ANDROID/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/if MONOANDROID/if ANDROID/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/if !MONOANDROID/if ANDROID/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/if __ANDROID_29__/if ANDROID/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/using Xamarin.Forms.Platform.Android.FastRenderers;/using Microsoft.Maui.Controls.Compatibility.Platform.Android.FastRenderers;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/using Xamarin.Forms.Platform.Android;/using Microsoft.Maui.Controls.Compatibility.Platform.Android; using Microsoft.Maui.Controls.Platform;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/using Xamarin.Forms.Platform.iOS/using Microsoft.Maui.Controls.Compatibility.Platform.iOS/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/using Xamarin.Forms.Platform.GTK/using Microsoft.Maui.Controls.Compatibility.Platform.GTK/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/using Xamarin.Forms.Platform.Tizen/using Microsoft.Maui.Controls.Compatibility.Platform.Tizen/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/using Xamarin.Forms.Platform.UWP/using Microsoft.Maui.Controls.Compatibility.Platform.UWP/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/using Xamarin.Forms.Platform.MacOS/using Microsoft.Maui.Controls.Compatibility.Platform.MacOS/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/Xamarin.Forms.PlatformConfiguration/Microsoft.Maui.Controls.PlatformConfiguration/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/Xamarin.Forms.Platform/Microsoft.Maui.Controls.Compatibility.Platform/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# IVisualElementRenderer

sed -i '' '/IVisualElementRenderer.ViewGroup/d' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/event EventHandler<VisualElementChangedEventArgs>/event EventHandler<Microsoft.Maui.Controls.Platform.VisualElementChangedEventArgs>/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/new ElementChangedEventArgs/new Microsoft.Maui.Controls.Platform.ElementChangedEventArgs/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/(ElementChangedEventArgs/(Microsoft.Maui.Controls.Platform.ElementChangedEventArgs/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/new VisualElementChangedEventArgs/new Microsoft.Maui.Controls.Platform.VisualElementChangedEventArgs/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# TextAlignment

sed -i '' 's/Xamarin.Forms.TextAlignment/Microsoft.Maui.TextAlignment/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# ElementChangedEventArgs

sed -i '' 's/override void OnElementChanged(ElementChangedEventArgs/override void OnElementChanged(Microsoft.Maui.Controls.Platform.ElementChangedEventArgs/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# XAML

sed -i '' 's/using Xamarin.Forms.Xaml;/using Microsoft.Maui.Controls.Xaml;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/Forms.Xaml/Microsoft.Maui.Controls.Xaml/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# Effects

sed -i '' 's/Xamarin.Forms.ExportEffect(/Microsoft.Maui.Controls.ExportEffect(/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

## Font

sed -i '' 's/Element.Font/Element.ToFont()/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# TypeConverter

sed -i '' 's/Xamarin.Forms.TypeConverter/System.ComponentModel.TypeConverter/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/\[TypeConverter/\[System.ComponentModel.TypeConverter/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/\[Forms.TypeConverter/\[System.ComponentModel.TypeConverter/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/ TypeConverter/ System.ComponentModel.TypeConverter/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/Xamarin.Forms.UriTypeConverter/Microsoft.Maui.Controls.UriTypeConverter/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/ConvertFromInvariantString(string value)/ConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/ConvertFromInvariantString(string\? value)/ConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# FontProperty

sed -i '' '/else if (e.PropertyName == Label.FontProperty.PropertyName)/,+1d' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# ElementChangedEventArgs

sed -i '' 's/ElementChangedEventArgs/Microsoft.Maui.Controls.Platform.ElementChangedEventArgs/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# Specific Class Names 

sed -i '' 's/using static Xamarin.Forms.AbsoluteLayout/using static Microsoft.Maui.Controls.Compatibility.AbsoluteLayout/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/Xamarin.Forms.Page/Microsoft.Maui.Controls.Page/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/Xamarin.Forms.Point/Microsoft.Maui.Graphics.Point/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/Xamarin.Forms.Size/Microsoft.Maui.Graphics.Size/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/Xamarin.Forms.View/Microsoft.Maui.Controls.View/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/ Grid/ Microsoft.Maui.Controls.Grid/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/\tGrid/\tMicrosoft.Maui.Controls.Grid/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/(Grid/(Microsoft.Maui.Controls.Grid/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/<Grid/<Microsoft.Maui.Controls.Grid/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/StackLayout/\tMicrosoft.Maui.Controls.StackLayout/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# *.android.cs

sed -i '' 's/ContainerView/Microsoft.Maui.Controls.Compatibility.Platform.Android.ContainerView/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.android.cs

sed -i '' 's/Context.ToPixels(/Microsoft.Maui.ContextExtensions.ToPixels(Context, /g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.android.cs

sed -i '' '1s/^/using Path = Android.Graphics.Path;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.android.cs

sed -i '' '1s/^/using Paint = Android.Graphics.Paint;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.android.cs

# TextSwitcherRenderer.android.cs

sed -i '' 's/Color lastUpdateColor =/Color? lastUpdateColor =/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/TextSwitcherRenderer.android.cs

sed -i '' 's/color.IsDefault/color.IsDefault()/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/TextSwitcherRenderer.android.cs

sed -i '' 's/(visualElementRenderer?.OnTouchEvent(e) ?? false) || //g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/TextSwitcherRenderer.android.cs

sed -i '' 's/.ToScaledPixel()//g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/TextSwitcherRenderer.android.cs

# Add Maui Namespaces

sed -i '' 's/using Xamarin.Forms;/using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
```

### 6. Manually Update TypeConverters

In .NET 6, `System.ComponentModel.TypeConverter` requires us to override `ConvertFrom` which uses `object value` as a parameter. 

Previously, in .NET Standard we used `ConvertFromInvariantString(string value)`.

To create the MauiCompat library, we must update the logic in each converter to handle an object instead of a string.

Start by adding the following line:

```cs
if (value is not string text)
    throw new InvalidOperationException("Only typeof(string) allowed");
```

Then, in the method body, replace the variable `value` with `text`.

- [ ] MediaSourceConverter.shared.cs
- [ ] FileMediaSourceConverter.shared.cs
- [ ] SafeAreaTypeConverter.shared.cs
- [ ] UriTypeConverter.shared.cs

### 7. Manually Update `IStreamImageConverter`

`Microsoft.Maui.IStreamImageConverter` adds a new API, `public bool IsEmpty { get; }`.

Add the following code to implement the new API:

```cs
public bool IsEmpty => Stream == null;
```

- [ ] StreamMediaSource.shared.cs

### 8. Manually Update Array.ForEach

.NET 6 no longer includes the `Array.ForEach` extension method. We need to refactor it to use `ForEach()`.

Example:
```cs
//previously children.ForEach(c => c.Hint = value);
foreach (var c in children)
    c.Hint = value;
```

- [ ] TextSwitcherRenderer.android.cs