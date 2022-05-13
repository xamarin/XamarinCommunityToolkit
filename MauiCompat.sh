dotnet new mauilib -o ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat -n Xamarin.CommunityToolkit.MauiCompat
dotnet new mauilib -o ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat -n Xamarin.CommunityToolkit.Markup.MauiCompat

dotnet new sln -o ./src/CommunityToolkit/ -n Xamarin.CommunityToolkit.MauiCompat
dotnet sln ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat.sln add ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj

dotnet new sln -o ./src/Markup/ -n Xamarin.CommunityToolkit.Markup.MauiCompat
dotnet sln ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat.sln add ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj

sed -i '' 's/<ImplicitUsings>enable/<ImplicitUsings>false/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/Xamarin.CommunityToolkit.MauiCompat.csproj
sed -i '' 's/<ImplicitUsings>enable/<ImplicitUsings>false/g' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/**/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj

printf > ./src/CommunityToolkit/Directory.build.props "<Project>
    <PropertyGroup>
        <Nullable>enable</Nullable>
        <PackageId>Xamarin.CommunityToolkit.MauiCompat</PackageId>
        <Summary>A .NET MAUI Comapatible version of Xamarin.CommunityToolkit, a community-created toolkit with common Xamarin converters, effects, behaviors etc.</Summary>
        <PackageTag>maui,net,xamarin,ios,android,uwp,xamarin.forms,effects,controls,converters,animations,toolkit,kit,communitytoolkit,xamarincommunitytoolkit,watchos,tvos,tizen,Microsoft.Toolkit.Xamarin.Forms</PackageTag>
        <Title>Xamarin.CommunityToolkit.MauiCompat</Title>
        <Description>Xamarin.CommunityToolkit.MauiCompat is a collection of Animations, Behaviors, Converters, and Effects for mobile development with .NET MAUI. It is the .NET MAUI Compatible version of Xamarin.CommunityToolkit.</Description>
        <PackageIcon>icon.png</PackageIcon>
        <PackageVersion>\$(Version)\$(VersionSuffix)</PackageVersion>
        <Authors>Microsoft</Authors>
        <Owners>microsoft</Owners>
        <NeutralLanguage>en</NeutralLanguage>
        <Copyright>© Microsoft Corporation. All rights reserved.</Copyright>
        <RepositoryUrl>https://github.com/xamarin/XamarinCommunityToolkit</RepositoryUrl>
        <PackageReleaseNotes>See: http://aka.ms/xct-release-notes</PackageReleaseNotes>
        <DefineConstants>\$(DefineConstants);</DefineConstants>
        <UseFullSemVerForNuGet>false</UseFullSemVerForNuGet>
        <PackageLicenseExpression>MIT</PackageLicenseExpression>
        <PackageRequireLicenseAcceptance>true</PackageRequireLicenseAcceptance>
        <PackageProjectUrl>https://github.com/xamarin/XamarinCommunityToolkit</PackageProjectUrl>
        <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
        <Version>1.3.0-pre4</Version>
    </PropertyGroup>    

    <ItemGroup>
        <Compile Include=\"**/*.shared.cs\" />
        <Compile Include=\"**/*.shared.*.cs\" />    
        <None Include=\"../../../LICENSE\" PackagePath=\"\" Pack=\"true\" />
        <None Include=\"../../../assets/XamarinCommunityToolkit_128x128.png\" PackagePath=\"icon.png\" Pack=\"true\" />
    </ItemGroup>

    <ItemGroup Condition=\" \$(TargetFramework.Contains(-android)) \">
        <Compile Include=\"**\*.android.cs\" />
        <Compile Include=\"**\*.android.*.cs\" />
        <AndroidResource Include=\"Resources\**\*.axml\" />
        <AndroidResource Include=\"Resources\**\*.xml\" />
        <AndroidResource Include=\"Resources\**\*.png\" />
    </ItemGroup>

    <ItemGroup Condition=\" \$(TargetFramework.Contains(-ios)) \">
        <Compile Include=\"**\*.ios.cs\" />
        <Compile Include=\"**\*.ios.*.cs\" />
    </ItemGroup>

    <ItemGroup Condition=\" \$(TargetFramework.Contains('-windows')) \">
        <Compile Include=\"**\*.uwp.cs\" />
        <Compile Include=\"**\*.uwp.*.cs\" />
        <Compile Remove=\"**\MediaElementRenderer.uwp.cs\" />
        <Compile Remove=\"**\CameraViewRenderer.uwp.cs\" />
        <Compile Remove=\"**\DrawingViewRenderer.uwp.cs\" />
        <Compile Remove=\"**\DrawingViewService.uwp.cs\" />
    </ItemGroup>

    <ItemGroup Condition=\" \$(TargetFramework.Contains('-maccatalyst')) \">
        <Compile Include=\"**\*.ios.cs\" />
        <Compile Include=\"**\*.ios.*.cs\" />
    </ItemGroup>

    <ItemGroup Condition=\" !\$(TargetFramework.Contains('-')) \">
        <Compile Include=\"**\*.netstandard.cs\" />
        <Compile Include=\"**\*.netstandard.*.cs\" />
    </ItemGroup>
    <PropertyGroup Condition=\" !\$(TargetFramework.Contains('-')) \">
        <DefineConstants>\$(DefineConstants);NETSTANDARD</DefineConstants>
    </PropertyGroup>

</Project>"

printf > ./src/Markup/Directory.build.props "<Project>
    <PropertyGroup>
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
        <DefineConstants>\$(DefineConstants);</DefineConstants>
        <UseFullSemVerForNuGet>false</UseFullSemVerForNuGet>
        <Title>Xamarin.CommunityToolkit.Markup.MauiCompat</Title>
        <Description>Xamarin Community Toolkit Markup MauiCompat is a set of fluent helper methods and classes to simplify building declarative .NET MAUI user interfaces in C#</Description>
        <PackageIcon>icon.png</PackageIcon>
        <Product>\$(AssemblyName) (\$(TargetFramework))</Product>
        <PackageVersion>\$(Version)\$(VersionSuffix)</PackageVersion>
        <PackageRequireLicenseAcceptance>true</PackageRequireLicenseAcceptance>
        <Version>1.3.0-pre4</Version>
        <PackageTags>maui,net,xamarin,xamarin.forms,toolkit,kit,communitytoolkit,xamarincommunitytoolkit,markup,csharpformarkup,csharp,csharpmarkup</PackageTags>
    </PropertyGroup>
    
    <ItemGroup>
        <None Include=\"../../../LICENSE\" PackagePath=\"\" Pack=\"true\" />
        <None Include=\"../../../assets/XamarinCommunityToolkit_128x128.png\" PackagePath=\"icon.png\" Pack=\"true\" />
    </ItemGroup>
</Project>"

find ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/ -name "*" ! -name "*.csproj" -delete
find ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/ -name "*" ! -name "*.csproj" -delete

rsync -avr --exclude='*.csproj' --exclude='bin' --exclude='obj' ./src/CommunityToolkit/Xamarin.CommunityToolkit/ ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/
rsync -avr --exclude='*.csproj' --exclude='bin' --exclude='obj' ./src/Markup/Xamarin.CommunityToolkit.Markup/ ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/ 

# Preserve

sed -i '' 's/\[Preserve(/\[Microsoft.Maui.Controls.Internals.Preserve(/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# Internals

sed -i '' 's/using Xamarin.Forms.Internals/using Microsoft.Maui.Controls.Internals/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' '/Forms.Internals.Log/d' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# WeakEventManager

sed -i '' 's/ Forms.WeakEventManager/ Microsoft.Maui.WeakEventManager/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# Forms.Image

sed -i '' 's/Xamarin.Forms.Image/Microsoft.Maui.Controls.Image/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/Forms.Image/Microsoft.Maui.Controls.Image/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# Colors
sed -i '' 's/ Forms\.Color\.Default/ default(Microsoft.Maui.Graphics.Color)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/Snackbar/**/**.cs
sed -i '' 's/ Color\.Default\./ new Microsoft.Maui.Graphics.Color()./g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/Snackbar/**/SnackBarAppearance*.cs
sed -i '' 's/ Color\.Default/ default(Microsoft.Maui.Graphics.Color)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/Snackbar/**/**.cs
sed -i '' 's/ == Forms.Color.Default/ .Equals(new Microsoft.Maui.Graphics.Color())/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/ == XColor.Default/ .Equals(new Microsoft.Maui.Graphics.Color())/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/using Color = Xamarin.Forms.Color;/using Color = Microsoft.Maui.Graphics.Color;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/ Color\./ Colors./g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\tColor\./\tColors./g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/Xamarin.Forms.Color/Microsoft.Maui.Graphics.Color/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/ Forms\.Color/ Microsoft.Maui.Graphics.Color/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\tForms\.Color/\tMicrosoft.Maui.Graphics.Color/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/Colors\.From/Color\.From/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/Colors.Default/new Microsoft.Maui.Graphics.Color()/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/ Color.FromRgba/ new Microsoft.Maui.Graphics.Color/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\tColor.FromRgba/\tnew Microsoft.Maui.Graphics.Color/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/\.R,/.Red,/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\.G,/.Green,/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\.B,/.Blue,/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\.A,/.Alpha,/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/\.R /.Red /g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\.G /.Green /g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\.B /.Blue /g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\.A /.Alpha /g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/\.R)/.Red)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\.G)/.Green)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\.B)/.Blue)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\.A)/.Alpha)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/\.A:/.Alpha:/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/r.IsDefault)/r.IsDefault())/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/.MultiplyAlpha(/.MultiplyAlpha((float)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/\.Hue/.GetHue()/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\.Saturation/.GetSaturation()/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\.Luminosity/.GetLuminosity()/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# Nullability

sed -i '' 's/event EventHandler<VisualElementChangedEventArgs>? ElementChanged/event EventHandler<VisualElementChangedEventArgs> ElementChanged/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/\.PropertyName\./.PropertyName?./g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/(object sender, PropertyChangedEventArgs e)/(object? sender, PropertyChangedEventArgs e)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# PlatformEffect

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

sed -i '' 's/new VisualElementChangedEventArgs/new Microsoft.Maui.Controls.Platform.VisualElementChangedEventArgs/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/new ElementChangedEventArgs/new Microsoft.Maui.Controls.Platform.ElementChangedEventArgs/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/(ElementChangedEventArgs/(Microsoft.Maui.Controls.Platform.ElementChangedEventArgs/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

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

## Internals

sed -i '' 's/Element.Font/Element.ToFont()/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

#Forms.Internals.Log

# TypeConverter

sed -i '' 's/Xamarin.Forms.TypeConverter/System.ComponentModel.TypeConverter/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/\[TypeConverter/\[System.ComponentModel.TypeConverter/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/\[TypeConversion/\[System.ComponentModel.TypeConverter/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\[Microsoft.Maui.Controls.Xaml.TypeConversion/\[System.ComponentModel.TypeConverter/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/\[Forms.TypeConverter/\[System.ComponentModel.TypeConverter/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/ TypeConverter/ System.ComponentModel.TypeConverter/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/Xamarin.Forms.UriTypeConverter/Microsoft.Maui.Controls.UriTypeConverter/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/ConvertFromInvariantString(string value)/ConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object valueObject)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/ConvertFromInvariantString(string\? value)/ConvertFrom(System.ComponentModel.ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object valueObject)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/if (value != null)/if (valueObject is not string value){throw new InvalidOperationException("Only typeof(string) allowed");}if (value != null)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Helpers/SafeAreaTypeConverter.shared.cs

sed -i '' 's/if (value == null)/if (valueObject is not string value){throw new InvalidOperationException("Only typeof(string) allowed");}if (value == null)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Core/MediaSourceConverter.shared.cs

sed -i '' 's/return/if (valueObject is not string value){throw new InvalidOperationException("Only typeof(string) allowed");}return/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Core/FileMediaSourceConverter.shared.cs

sed -i '' 's/return/if (valueObject is not string value){throw new InvalidOperationException("Only typeof(string) allowed");}return/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/MediaElement/UriTypeConverter.shared.cs

# Font

sed -i '' '/else if (e.PropertyName == Label.FontProperty.PropertyName)/,+1d' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/Font.FontSize/Font.Size/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' '1s/^/using Font = Microsoft.Maui.Font;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBarActionOptions.shared.cs

sed -i '' '1s/^/using Font = Microsoft.Maui.Font;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.android.cs

sed -i '' '1s/^/using Font = Microsoft.Maui.Font;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.ios.macos.cs

sed -i '' '1s/^/using Font = Microsoft.Maui.Font;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/MessageOptions.shared.cs

## ToUIFont

sed -i '' '1s/^/using Microsoft.Maui.Controls.Platform;using Microsoft.Extensions.DependencyInjection;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBarAppearance.ios.cs

sed -i '' 's/Forms.Font.Default.ToUIFont();/Microsoft.Maui.Font.Default.ToUIFont(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetRequiredService<IFontManager>());/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBarAppearance.ios.cs

sed -i '' '1s/^/using Microsoft.Maui.Controls.Platform;using Microsoft.Extensions.DependencyInjection;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.ios.macos.cs

sed -i '' 's/.Font.ToUIFont();/.Font.ToUIFont(sender.Handler?.MauiContext?.Services.GetRequiredService<IFontManager>());/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.ios.macos.cs

# Typeface

## SnackBar.android

sed -i '' '1s/^/using Microsoft.Maui.Controls.Platform;using Microsoft.Extensions.DependencyInjection;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.android.cs

sed -i '' 's/if (arguments.MessageOptions.Font != Font.Default)/var fontManager = sender.Handler?.MauiContext?.Services.GetRequiredService<IFontManager>();\
\
			if (fontManager is null)\
			{\
				throw new ArgumentException("Unable to get IFontManager implementation");\
			}\
if (arguments.MessageOptions.Font != Font.Default)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.android.cs

sed -i '' 's/ToTypeface()/ToTypeface(fontManager)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.android.cs

## TextSwitcherRenderer.android
sed -i '' 's/var newTypeface = f.ToTypeface();/var fontManager = Element.Handler?.MauiContext?.Services.GetRequiredService<IFontManager>();\
\
			if (fontManager is null)\
			{\
				throw new ArgumentException("Unable to get IFontManager implementation");\
			}\
var newTypeface = f.ToTypeface(fontManager);/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/TextSwitcherRenderer.android.cs
sed -i '' 's/child.RecalculateSpanPositions(/Xamarin.CommunityToolkit.Extensions.Internals.TextViewExtensions.RecalculateSpanPositions(child, /g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/TextSwitcherRenderer.android.cs


# Controls 

sed -i '' 's/Xamarin.Forms.Page/Microsoft.Maui.Controls.Page/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/Xamarin.Forms.View/Microsoft.Maui.Controls.View/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/Forms.View/Microsoft.Maui.Controls.View/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# Layouts

sed -i '' 's/ Layout / Microsoft.Maui.Controls.Layout /g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/ Layout)/ Microsoft.Maui.Controls.Layout)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/using static Xamarin.Forms.AbsoluteLayout/using static Microsoft.Maui.Controls.Compatibility.AbsoluteLayout;using Microsoft.Maui.Layouts;using AbsoluteLayout = Microsoft.Maui.Controls.Compatibility.AbsoluteLayout/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/StackLayout/\tMicrosoft.Maui.Controls.StackLayout/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/ GridLength/ Microsoft.Maui.GridLength/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\tGridLength/\tMicrosoft.Maui.GridLength/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/(GridLength/(Microsoft.Maui.GridLength/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/<GridLength/<Microsoft.Maui.GridLength/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs


sed -i '' 's/ Grid/ Microsoft.Maui.Controls.Compatibility.Grid/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/\tGrid/\tMicrosoft.Maui.Controls.Compatibility.Grid/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/(Grid/(Microsoft.Maui.Controls.Compatibility.Grid/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/<Grid/<Microsoft.Maui.Controls.Compatibility.Grid/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# Graphics

sed -i '' 's/Xamarin.Forms.Point/Microsoft.Maui.Graphics.Point/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/Xamarin.Forms.Size/Microsoft.Maui.Graphics.Size/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/Xamarin.Forms.View/Microsoft.Maui.Controls.View/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# ViewExtensions

sed -i '' 's/\tViewExtensions./\tMicrosoft.Maui.Controls.ViewExtensions./g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# *.android.cs

sed -i '' 's/ContainerView/Microsoft.Maui.Controls.Platform.Compatibility.ContainerView/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.android.cs
sed -i '' 's/ContainerView(Context, basePopup.Content/ContainerView(Context, basePopup.Content, Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PopupRenderer.android.cs

sed -i '' 's/View.Context.ToPixels(/Microsoft.Maui.Platform.ContextExtensions.ToPixels(View.Context ?? throw new NullReferenceException(), /g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.android.cs
sed -i '' 's/Context.ToPixels(/Microsoft.Maui.Platform.ContextExtensions.ToPixels(Context, /g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.android.cs
sed -i '' 's/context.ToPixels(/Microsoft.Maui.Platform.ContextExtensions.ToPixels(context, /g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.android.cs

sed -i '' 's/Resource.Id/Xamarin.CommunityToolkit.MauiCompat.Resource.Id/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.android.cs
sed -i '' 's/Resource.Layout/Xamarin.CommunityToolkit.MauiCompat.Resource.Layout/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.android.cs

sed -i '' '1s/^/using Path = Android.Graphics.Path;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.android.cs

sed -i '' '1s/^/using Paint = Android.Graphics.Paint;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.android.cs

sed -i '' 's/ShapeDrawable/global::Android.Graphics.Drawables.ShapeDrawable/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.android.cs

# BarStyle.android.cs

sed -i '' '1s/^/using Window = Android.Views.Window;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/BarStyle.android.cs

# TextSwitcherRenderer.android.cs

sed -i '' 's/(visualElementRenderer?.OnTouchEvent(e) ?? false) || //g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/TextSwitcherRenderer.android.cs

sed -i '' 's/f.ToScaledPixel()/(float)f.Size/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/TextSwitcherRenderer.android.cs

sed -i '' 's/children.ForEach(/Array.ForEach(children,/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/TextSwitcherRenderer.android.cs

sed -i '' '1s/^/using Microsoft.Extensions.DependencyInjection;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/TextSwitcherRenderer.android.cs

sed -i '' 's/ToAttributed(Element.ToFont(), Element.TextColor, nextView);/ToSpannableString(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetRequiredService<IFontManager>(), defaultColor: Element.TextColor);/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/TextSwitcherRenderer.android.cs

## PlatformTouchEffect.ios.cs

sed -i '' 's/(isStarted ? color : control.BackgroundColor).ToCGColor()/Microsoft.Maui.Platform.ColorExtensions.ToCGColor(isStarted ? color : control.BackgroundColor)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformTouchEffect.ios.cs

## DrawingViewRenderer.ios.cs

sed -i '' 's/void OnLinesCollectionChanged(object sender/void OnLinesCollectionChanged(object? sender/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/DrawingViewRenderer.ios.cs

sed -i '' 's/currentPoint.ToPoint()/Microsoft.Maui.Platform.CoreGraphicsExtensions.ToPoint(currentPoint)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/DrawingViewRenderer.ios.cs

# DrawingViewService.ios.cs

sed -i '' 's/backgroundColor.ToCGColor()/Microsoft.Maui.Platform.ColorExtensions.ToCGColor(backgroundColor)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/DrawingViewService.ios.cs

sed -i '' 's/strokeColor.ToCGColor()/Microsoft.Maui.Platform.ColorExtensions.ToCGColor(strokeColor)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/DrawingViewService.ios.cs

sed -i '' 's/line.LineColor.ToCGColor()/Microsoft.Maui.Platform.ColorExtensions.ToCGColor(line.LineColor)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/DrawingViewService.ios.cs

# SnackbarAppearance.ios.cs

sed -i '' '1s/^/using Microsoft.Maui;using Microsoft.Maui.Controls.Compatibility.Platform.iOS;using Microsoft.Maui.Graphics;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackbarAppearance.ios.cs

sed -i '' 's/color.A /color.Alpha /g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackbarAppearance.ios.cs

# Snackbar.android.cs

sed -i '' 's/await GetRendererWithRetries(sender)/(await GetRendererWithRetries(sender))?.View ?? sender.ToPlatform(sender.Handler.MauiContext)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.android.cs
sed -i '' 's/renderer.View/renderer/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.android.cs
sed -i '' 's/namespace/using Microsoft.Maui.Platform;namespace/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.android.cs

# IconTintColorEffectRouter.android.cs

sed -i '' 's/args.PropertyName?.Equals(IconTintColorEffect.TintColorProperty.PropertyName)/args.PropertyName?.Equals(IconTintColorEffect.TintColorProperty.PropertyName) is true/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/IconTintColorEffectRouter.android.cs

sed -i '' 's/args.PropertyName?.Equals(Microsoft.Maui.Controls.Image.SourceProperty.PropertyName)/args.PropertyName?.Equals(Microsoft.Maui.Controls.Image.SourceProperty.PropertyName) is true/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/IconTintColorEffectRouter.android.cs

sed -i '' 's/args.PropertyName?.Equals(Microsoft.Maui.Controls.ImageButton.SourceProperty.PropertyName)/args.PropertyName?.Equals(Microsoft.Maui.Controls.ImageButton.SourceProperty.PropertyName) is true/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/IconTintColorEffectRouter.android.cs

sed -i '' 's/SetImageViewTintColor(ImageView image, Color color)/SetImageViewTintColor(ImageView image, Microsoft.Maui.Graphics.Color color)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/IconTintColorEffectRouter.android.cs

sed -i '' 's/SetButtonTintColor(Button button, Color color)/SetButtonTintColor(Button button, Microsoft.Maui.Graphics.Color color)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/IconTintColorEffectRouter.android.cs

sed -i '' '1s/^/using Button = Android.Widget.Button;/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/IconTintColorEffectRouter.android.cs

# IconTintColorEffectRouter.ios.cs

sed -i '' 's/args.PropertyName?.Equals(IconTintColorEffect.TintColorProperty.PropertyName)/args.PropertyName?.Equals(IconTintColorEffect.TintColorProperty.PropertyName) is true/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/IconTintColorEffectRouter.ios.cs

sed -i '' 's/args.PropertyName?.Equals(Image.SourceProperty.PropertyName)/args.PropertyName?.Equals(Microsoft.Maui.Controls.Image.SourceProperty.PropertyName) is true/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/IconTintColorEffectRouter.ios.cs

sed -i '' 's/args.PropertyName?.Equals(ImageButton.SourceProperty.PropertyName)/args.PropertyName?.Equals(Microsoft.Maui.Controls.ImageButton.SourceProperty.PropertyName) is true/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/IconTintColorEffectRouter.ios.cs

# SemanticEffectRouterBase.ios.cs

sed -i '' 's/(T)Element.Effects.FirstOrDefault(e => e is T)/(T)Element.Effects.First(e => e is T);/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SemanticEffectRouterBase.ios.cs

# CameraViewRenderer.android.cs

sed -i '' 's/visualElementRenderer?.OnTouchEvent(e) is true || //g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/CameraViewRenderer.android.cs

sed -i '' 's/static void MeasureExactly(AView control, VisualElement? element, Context? context)/static void MeasureExactly(AView control, VisualElement? element, Context context)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/CameraViewRenderer.android.cs

sed -i '' 's/Context.GetFragmentManager();/Microsoft.Maui.Platform.ContextExtensions.GetFragmentManager(Context ?? throw new NullReferenceException()) ?? throw new InvalidOperationException();/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/CameraViewRenderer.android.cs

# CameraViewRenderer.ios.cs

sed -i '' 's/RequestAvAsset/RequestAVAsset/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/CameraViewRenderer.ios.cs

# VisualElementExtension.shared.cs

sed -i '' 's/v,/(float)v,/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/VisualElementExtension.shared.cs
sed -i '' 's/, v/, (float)v/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/VisualElementExtension.shared.cs
sed -i '' 's/.Descendants()/.GetVisualTreeDescendants()/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/VisualElementExtensions.uwp.cs

# NativeSnackBar.ios.macos.cs

sed -i '' 's/public SnackBarLayout Microsoft.Maui.Controls.Layout/public SnackBarLayout Layout/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/NativeSnackBar.ios.macos.cs

# NativeSnackBarButton.ios.macos.cs

sed -i '' 's/LineBreakMode =/TitleLabel.LineBreakMode =/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/NativeSnackButton.ios.macos.cs

# VisualFeedbackEffect.shared.cs

sed -i '' 's/nativeColor.Alpha/nativeColor.A/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/VisualFeedbackEffect.shared.cs

# VisualFeedbackEffectRouter.shared.cs

sed -i '' 's/nativeColor.Alpha/nativeColor.A/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/VisualFeedbackEffectRouter.android.cs

# PlatformShadowEffect.ios.macos.cs

sed -i '' 's/ShadowEffect.GetColor(Element).ToCGColor()/Microsoft.Maui.Platform.ColorExtensions.ToCGColor(ShadowEffect.GetColor(Element))/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformShadowEffect.ios.macos.cs

sed -i '' 's/using Xamarin.CommunityToolkit.Android.Effects;/using System;using Xamarin.CommunityToolkit.Android.Effects;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformShadowEffect.android.cs

# PlatformTouchEffect.android.cs
sed -i '' 's/ViewGroup? Group => Container ?? Control as ViewGroup;/ViewGroup? Group => (Container ?? Control) as ViewGroup;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformTouchEffect.android.cs
sed -i '' 's/XColor.Transparent/Microsoft.Maui.Graphics.Colors.Transparent/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformTouchEffect.android.cs
sed -i '' 's/new Rectangle/new Rect/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformTouchEffect.android.cs

# ColorExtension.shared.cs

sed -i '' 's/(double)/(float)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ColorExtension.shared.cs
sed -i '' 's/WithRed(this Color baseColor, double newR)/WithRed(this Color baseColor, float newR)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ColorExtension.shared.cs
sed -i '' 's/WithGreen(this Color baseColor, double newG)/WithGreen(this Color baseColor, float newG)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ColorExtension.shared.cs
sed -i '' 's/WithBlue(this Color baseColor, double newB)/WithBlue(this Color baseColor, float newB)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ColorExtension.shared.cs
sed -i '' 's/WithAlpha(this Color baseColor, double newA)/WithAlpha(this Color baseColor, float newA)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ColorExtension.shared.cs
sed -i '' 's/WithCyan(this Color baseColor, double newC)/WithCyan(this Color baseColor, float newC)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ColorExtension.shared.cs
sed -i '' 's/WithMagenta(this Color baseColor, double newM)/WithMagenta(this Color baseColor, float newM)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ColorExtension.shared.cs
sed -i '' 's/WithYellow(this Color baseColor, double newY)/WithYellow(this Color baseColor, float newY)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ColorExtension.shared.cs
sed -i '' 's/WithBlackKey(this Color baseColor, double newK)/WithBlackKey(this Color baseColor, float newK)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ColorExtension.shared.cs
sed -i '' 's/double GetPercentBlackKey/float GetPercentBlackKey/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ColorExtension.shared.cs
sed -i '' 's/double GetPercentCyan/float GetPercentCyan/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ColorExtension.shared.cs
sed -i '' 's/double GetPercentMagenta/float GetPercentMagenta/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ColorExtension.shared.cs
sed -i '' 's/double GetPercentYellow/float GetPercentYellow/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ColorExtension.shared.cs

# TouchEffect.shared.cs

sed -i '' 's/OnLayoutChildAdded(layout, new ElementEventArgs(view));/OnLayoutChildAdded(layout, new ElementEventArgs((Element)view));/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/TouchEffect.shared.cs

# GravatarImageExtension.shared.cs
sed -i '' 's/using System;/using System;using Microsoft.Extensions.DependencyInjection;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/GravatarImageExtension.shared.cs

# AvatarView.shared.cs

sed -i '' 's/using System;/using System;using static Microsoft.Maui.Controls.Compatibility.AbsoluteLayout;using Microsoft.Maui.Layouts;using AbsoluteLayout = Microsoft.Maui.Controls.Compatibility.AbsoluteLayout;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/AvatarView.shared.cs
sed -i '' 's/uriSource\.GetStreamAsync/((IStreamImageSource)uriSource).GetStreamAsync/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/AvatarView.shared.cs

# MotionEventHelper.android.cs 

sed -i '' '/if (layout.CascadeInputTransparent)/,+1d' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/MotionEventHelper.android.cs

# StateLayoutController.shared.cs

sed -i '' 's/Microsoft.Maui.Controls.Grid/Microsoft.Maui.Controls.Compatibility.Grid/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/StateLayoutController.shared.cs

# TabBadgeTemplate.shared.cs

sed -i '' 's/Frame/Microsoft.Maui.Controls.Frame/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/TabBadgeTemplate.shared.cs

# Device.macOS

sed -i '' 's/Device.macOS/Device.MacCatalyst/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.shared.cs

# CameraFragment.android.cs

sed -i '' 's/MauiCompat.Resource.Layout.CameraFragment/MauiCompat.Resource.Layout.camerafragment/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/CameraFragment.android.cs
sed -i '' 's/namespace/using RectF = Android.Graphics.RectF;namespace/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/CameraFragment.android.cs

# RangeSlider.shared.cs

sed -i '' 's/return value.Clamp(MinimumValue, MaximumValue);/return Math.Clamp(value, MinimumValue, MaximumValue);/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/RangeSlider.shared.cs

# Replace Xamarin.Forms Namespace

sed -i '' 's/using Xamarin.Forms;/using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/using Xamarin.Forms;/using Microsoft.Maui; using Microsoft.Maui.Controls; using Microsoft.Maui.Graphics; using Microsoft.Maui.Controls.Compatibility;/g' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/**/**.cs
sed -i '' 's/using Microsoft.Maui.Controls.Compatibility;/using Microsoft.Maui.Controls.Compatibility;using Microsoft.Maui.Layouts;using FlexLayout = Microsoft.Maui.Controls.FlexLayout;/g' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/ViewInFlexLayoutExtensions.cs
sed -i '' 's/Xamarin.Forms/Microsoft.Maui.Controls/g' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/ElementExtensions.cs
sed -i '' 's/Xamarin.Forms.Rectangle/Microsoft.Maui.Graphics.Rect/g' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/RelativeLayout.cs
sed -i '' 's/Xamarin.Forms.RelativeLayout/Microsoft.Maui.Controls.Compatibility.RelativeLayout/g' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/RelativeLayout.cs
sed -i '' 's/Xamarin.Forms.View/Microsoft.Maui.Controls.View/g' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/RelativeLayout.cs
sed -i '' 's/Xamarin.Forms.Constraint/Microsoft.Maui.Controls.Compatibility.Constraint/g' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/RelativeLayout.cs
sed -i '' 's/using Microsoft.Maui.Controls.Compatibility;/using Microsoft.Maui.Controls.Compatibility;using Grid = Microsoft.Maui.Controls.Grid;/g' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/ViewInGridExtensions.cs
sed -i '' 's/using Xamarin.Forms.Shapes;/using Microsoft.Maui.Controls.Shapes;using Rect = Microsoft.Maui.Graphics.Rect;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Effects/CornerRadius/CornerRadiusEffect.shared.cs
sed -i '' 's/new Rectangle/new Rect/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/*.shared.cs

# MauiColorExtensions
printf > ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Extensions/MauiColorExtensions.android.cs "
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
}"

printf > ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Extensions/MauiColorExtensions.ios.cs "
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

			return Microsoft.Maui.Platform.ColorExtensions.ToPlatform(colorToConvert);
		}
	}
}"

sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Effects/IconTintColor/IconTintColorEffectRouter.ios.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Effects/StatusBar/PlatformStatusBarEffect.ios.cs
sed -i '' 's/\[Foundation.Preserve(AllMembers = true)/using Xamarin.CommunityToolkit.MauiCompat; \[Foundation.Preserve(AllMembers = true)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Effects/VisualFeedback/VisualFeedbackEffectRouter.ios.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/DrawingView/Renderer/DrawingViewRenderer.ios.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/MediaElement/iOS/MediaElementRenderer.ios.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/Popup/iOS/PopupRenderer.ios.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/Snackbar/SnackBar.ios.macos.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/Snackbar/Helpers/iOS/SnackBarAppearance.ios.cs
sed -i '' 's/UIColor.SystemGrayColor/UIColor.SystemGray/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/Snackbar/Helpers/iOS/SnackBarAppearance.ios.cs

sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Effects/IconTintColor/IconTintColorEffectRouter.android.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Effects/Shadow/PlatformShadowEffect.android.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Effects/StatusBar/PlatformStatusBarEffect.android.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Effects/Touch/PlatformTouchEffect.android.cs
sed -i '' 's/\[Microsoft.Maui.Controls.Internals.Preserve(AllMembers = true)/using Xamarin.CommunityToolkit.MauiCompat; \[Microsoft.Maui.Controls.Internals.Preserve(AllMembers = true)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Effects/VisualFeedback/VisualFeedbackEffectRouter.android.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/PlatformConfiguration/AndroidSpecific/NavigationBar/PlatformNavigationBarEffect.android.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/CameraView/Android/CameraFragment.android.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/DrawingView/Renderer/DrawingViewRenderer.android.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/DrawingView/Service/DrawingViewService.android.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/MediaElement/Android/MediaElementRenderer.android.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/Snackbar/SnackBar.android.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/ViewSwitcher/BackgroundManager.android.cs
sed -i '' 's/public class/using Xamarin.CommunityToolkit.MauiCompat; public class/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Views/ViewSwitcher/TextSwitcherRenderer.android.cs

# StreamMediaSource

sed -i '' 's/readonly object synchandle = new object();/public bool IsEmpty => Stream == null; readonly object synchandle = new object();/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Core/StreamMediaSource.shared.cs

# nfloat

sed -i '' 's/nfloat/System.Runtime.InteropServices.NFloat/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# PlatformStatusBarEffect

sed -i '' 's/FormsAppCompatActivity/MauiAppCompatActivity/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformStatusBarEffect.android.cs
sed -i '' 's/Activity.SetStatusBarColor/Activity.Window.SetStatusBarColor/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformStatusBarEffect.android.cs

# AVMediaType

sed -i '' 's/photoOutput.ConnectionFromMediaType(AVMediaType.Video)/photoOutput.ConnectionFromMediaType(new NSString("video"))/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video)/AVCaptureDevice.DevicesWithMediaType("video")/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/AVMediaType.Audio/AVMediaTypes.Audio/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/AVMediaType.Video/AVAuthorizationMediaType.Video/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# Log

sed -i '' 's/using System;/using System;using Microsoft.Extensions.Logging;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/Log\./(Microsoft.Maui.Controls.Application.Current?.Handler.MauiContext?.Services.GetService(typeof(ILogger)) as ILogger)?.Log/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

# WINUI
sed -i '' 's/using Xamarin.CommunityToolkit.UI.Views;/using Xamarin.CommunityToolkit.UI.Views;using Microsoft.Maui.Controls.Compatibility;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs
sed -i '' 's/UAP10_0/WINDOWS/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/**.cs

sed -i '' 's/using Windows.UI.Xaml/using Microsoft.UI.Xaml/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/LifeCycleEffectRouter.uwp.cs

sed -i '' 's/using Windows.UI.Xaml/using Microsoft.UI.Xaml/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/RemoveBorderEffect.uwp.cs

sed -i '' 's/using Windows.UI.Xaml/using Microsoft.UI.Xaml/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SelectAllTextEffect.uwp.cs

sed -i '' 's/using Windows.UI.Xaml/using Microsoft.UI.Xaml/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SemanticEffectRouter.uwp.cs

sed -i '' 's/using Windows.UI.Xaml/using Microsoft.UI.Xaml/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SemanticEffectRouterBase.uwp.cs
sed -i '' 's/Microsoft.Maui.Controls.Compatibility.Platform.UWP.PlatformEffect/Microsoft.Maui.Controls.Platform.PlatformEffect/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SemanticEffectRouterBase.uwp.cs

sed -i '' 's/using Xamarin.CommunityToolkit.UWP.Effects;/using Xamarin.CommunityToolkit.UWP.Effects;using Microsoft.Maui.Platform;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformStatusBarEffect.uwp.cs

sed -i '' 's/using Windows.UI.Xaml/using Microsoft.UI.Xaml/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformTouchEffect.uwp.cs

sed -i '' 's/using Windows.UI.Xaml/using Microsoft.UI.Xaml/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SemanticExtensions.uwp.cs
sed -i '' 's/using Microsoft.UI.Xaml;/using Microsoft.UI.Xaml;using Window = Microsoft.UI.Xaml.Window;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SemanticExtensions.uwp.cs

sed -i '' 's/view.IsInNativeLayout = true;//g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ViewToRendererConverter.uwp.cs
sed -i '' 's/view.IsInNativeLayout = false;//g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ViewToRendererConverter.uwp.cs
sed -i '' 's/Rectangle/Rect/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ViewToRendererConverter.uwp.cs
sed -i '' 's/Microsoft.Maui.Controls.Layout/Layout/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ViewToRendererConverter.uwp.cs
sed -i '' 's/using Windows.UI.Xaml/using Microsoft.UI.Xaml/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ViewToRendererConverter.uwp.cs
sed -i '' 's/using Microsoft.UI.Xaml;/using Microsoft.UI.Xaml;using Microsoft.UI.Xaml.Shapes;using Layout = Microsoft.Maui.Controls.Compatibility.Layout;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ViewToRendererConverter.uwp.cs

sed -i '' 's/using Microsoft.Maui.Controls.Compatibility.Platform.UWP;/using Microsoft.Maui.Controls.Compatibility.Platform.UWP;using Microsoft.Maui.Controls.Platform;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/ImageSourceValidator.android.ios.macos.uwp.wpf.gtk.tizen.cs

sed -i '' 's/if NETSTANDARD || __WATCHOS__ || __TVOS__/if NETSTANDARD || __WATCHOS__ || __TVOS__ || WINDOWS/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/DrawingViewService.shared.cs

sed -i '' 's/Windows.UI.Xaml/Microsoft.UI.Xaml/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/GravatarImageSourceHandler.uwp.cs
sed -i '' 's/using Microsoft.Maui.Controls.Compatibility.Platform.UWP;/using Microsoft.Maui.Controls.Compatibility.Platform.UWP;using Microsoft.Maui.Controls.Platform;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/GravatarImageSourceHandler.uwp.cs

sed -i '' 's/Windows.UI.Xaml/Microsoft.UI.Xaml/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/AspectExtensions.uwp.cs

sed -i '' 's/Windows.UI.Xaml/Microsoft.UI.Xaml/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PopupRenderer.uwp.cs
sed -i '' 's/using Microsoft.Maui.Controls.Compatibility.Platform.UWP;/using Microsoft.Maui.Controls.Compatibility.Platform.UWP;using Microsoft.Maui.Platform;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PopupRenderer.uwp.cs

sed -i '' 's/using Windows.UI.Xaml/using Microsoft.UI.Xaml/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SemanticOrderViewRenderer.uwp.cs
sed -i '' 's/if (element is VisualElement ve)/if (element is FrameworkElement ve)/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SemanticOrderViewRenderer.uwp.cs

sed -i '' 's/MasterDetailPage/FlyoutPage/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/NavigationExtensions.uwp.cs

sed -i '' 's/Windows.UI/Microsoft.UI/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformShadowEffect.uwp.cs
sed -i '' 's/using Microsoft.Maui.Controls.Compatibility.Grid = Xamarin.Forms.Grid;/using Microsoft.Maui.Platform;using Application = Microsoft.Maui.Controls.Application;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformShadowEffect.uwp.cs
sed -i '' 's/Layout<View>/Microsoft.Maui.Controls.StackLayout/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformShadowEffect.uwp.cs
sed -i '' 's/var renderer = shadowPanel?.Children.First().GetOrCreateRenderer();/var renderer = shadowPanel?.Children.First().ToPlatform(Application.Current.MainPage.Handler?.MauiContext);/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformShadowEffect.uwp.cs
sed -i '' 's/ElementCompositionPreview.SetElementChildVisual(renderer?.ContainerElement, spriteVisual);/ElementCompositionPreview.SetElementChildVisual(renderer, spriteVisual);/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/PlatformShadowEffect.uwp.cs

sed -i '' 's/using Windows.UI/using Microsoft.UI/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.uwp.cs
sed -i '' 's/using Microsoft.Maui.Controls.Compatibility.Platform.UWP;/using Microsoft.Maui.Controls.Compatibility.Platform.UWP;using Microsoft.Maui.Controls;using Border = Microsoft.UI.Xaml.Controls.Border;using Grid = Microsoft.UI.Xaml.Controls.Grid;using RowDefinition = Microsoft.UI.Xaml.Controls.RowDefinition;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.uwp.cs
sed -i '' 's/Microsoft.Maui.Controls.Compatibility.Grid/Grid/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.uwp.cs
sed -i '' 's/Microsoft.Maui.GridLength/GridLength/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.uwp.cs
sed -i '' 's/Forms.VisualElement/VisualElement/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBar.uwp.cs

sed -i '' 's/using Windows.UI/using Microsoft.UI/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBarLayout.uwp.wpf.cs
sed -i '' 's/Microsoft.Maui.Controls.Compatibility.Grid/Grid/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBarLayout.uwp.wpf.cs
sed -i '' 's/Microsoft.Maui.GridLength/GridLength/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBarLayout.uwp.wpf.cs
sed -i '' 's/ToBrush()/ToPlatform()/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBarLayout.uwp.wpf.cs
sed -i '' 's/Forms.//g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBarLayout.uwp.wpf.cs
sed -i '' 's/Font.Font/Font./g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBarLayout.uwp.wpf.cs
sed -i '' 's/using Microsoft.Maui.Controls.Compatibility.Platform.UWP;/using Microsoft.Maui.Controls.Compatibility.Platform.UWP;using Microsoft.Maui;using Microsoft.Maui.Controls;using Microsoft.Maui.Platform;using Microsoft.UI.Xaml.Controls;using Microsoft.UI.Xaml.Media;using Button = Microsoft.UI.Xaml.Controls.Button;using ColumnDefinition = Microsoft.UI.Xaml.Controls.ColumnDefinition;using CornerRadius = Microsoft.UI.Xaml.CornerRadius;using Grid = Microsoft.UI.Xaml.Controls.Grid;using GridLength = Microsoft.UI.Xaml.GridLength;using RowDefinition = Microsoft.UI.Xaml.Controls.RowDefinition;using Thickness = Microsoft.UI.Xaml.Thickness;/g' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/**/SnackBarLayout.uwp.wpf.cs