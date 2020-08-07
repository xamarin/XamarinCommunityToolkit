Xamarin Community Toolkit
===========
Members of the community are rebooting this library under the guidance of the Xamarin team. The Xamarin Community Toolkit is a collection of common elements for mobile development with Xamarin.Forms that people tend to replicate across multiple apps. It simplifies and demonstrates common developer tasks when building apps with Xamarin.Forms. If you're looking for the historical code to reference, please check the **legacy** branch.

## Build Status

If you like to live dangerously (and while we are preparing our v1), you can use our [nightly](https://pkgs.dev.azure.com/xamarin/public/_packaging/XamarinCommunityToolkitNightly/nuget/v3/index.json) feed to try out packages right now.

| Build Server | Type         | Platform | Status |
|--------------|--------------|----------|--------|
| Azure DevOps         | Build        | Windows & Mac  | [![Build Status](https://dev.azure.com/xamarin/public/_apis/build/status/xamarin/CommunityToolkit/xamarin.XamarinCommunityToolkit%20(Public)?branchName=main)](https://dev.azure.com/xamarin/public/_build?definitionId=55&_a=summary)                                                  |

## Sample App

Browsing the [sample app](./XamarinCommunityToolkitSample) is the best place to start exploring what's available today. 

## Installation

The toolkit is available via NuGet, and should be installed into all your projects:

* NuGet Official Releases: [![NuGet](https://img.shields.io/nuget/vpre/Xamarin.CommunityToolkit.svg?label=NuGet)](https://www.nuget.org/packages/Microsoft.Toolkit.Xamarin.Forms)
* NuGet Nightly Releases: [![NuGet Nightly](https://img.shields.io/badge/NuGet-Nightly-yellow)](https://pkgs.dev.azure.com/xamarin/public/_packaging/XamarinCommunityToolkitNightly/nuget/v3/index.json)

Now all you need to do is use it! 

For example, to use the `AvatarView` you first include the toolkit namespace:

```xaml
xmlns:views="clr-namespace:Microsoft.Toolkit.Xamarin.Forms.UI.Views;assembly=Microsoft.Toolkit.Xamarin.Forms"
```

Then place the view in your XAML:

```xaml
<views:AvatarView Text="DO" Size="46" Color="#FF3300" TextColor="White" />
```

Of course, `AvatarView` also supports `Source` for loading images plus a few other properties. Check it out!

![AvatarView](./images/avatar-do.png)

## Contributions welcome!

If you have one or more of these common pieces of code that you are always replicating across apps, don't hesitate to contribute! We aim to be the first NuGet package you install when creating a new Xamarin app!

Please have a look at our [contribution guide](CONTRIBUTING.md) before you get started.

## Community Toolkit Principles
- Principle #1: The toolkit will be kept simple.
- Principle #2: As soon as a comparable feature is available in Xamarin.Forms, it will be marked as deprecated.
- Principle #3: All features will be supported for two Xamarin.Forms release cycles or until another principle supersedes it.

## Project Structure

To structure our project, we have adopted a range of namespaces. You can find them below with a short description of what you will find where.

| Namespace | Description |
|--------------|--------------|
| Xamarin.CommunityToolkit | Main NuGet package includes code only helpers, etc. |
| Xamarin.CommunityToolkit.UI | UI Packages - XAML converters, Visual tree extensions, and other extensions and helpers for your XAML UI |
| Xamarin.CommunityToolkit.UI.Controls | Controls such as TabView, etc.|
| Xamarin.CommunityToolkit.UI.Layout | Layouts such as WrapLayout, etc. |
| Xamarin.CommunityToolkit.SampleApp | Sample App where you can find all of the above for reference. How to use them and see how it looks |
| Xamarin.CommunityToolkit.UnitTests | This is where our unit tests live. Please keep growing them! |
