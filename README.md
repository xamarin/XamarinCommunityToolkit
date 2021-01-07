[<img src="https://raw.githubusercontent.com/dotnet-foundation/swag/master/logo/dotnetfoundation_v4.svg" alt=".NET Foundation" width=100>](https://dotnetfoundation.org)

<img src="assets/XamarinCommunityToolkit_128x128.png" width="64" /> Xamarin Community Toolkit
===========
Members of the community are rebooting this library under the guidance of the Xamarin team. The Xamarin Community Toolkit is a collection of common elements for mobile development with Xamarin.Forms that people tend to replicate across multiple apps. It simplifies and demonstrates common developer tasks when building apps with Xamarin.Forms. If you're looking for the historical code to reference, please check the **legacy** branch.

## Build Status

If you like to live dangerously, you can use our [nightly](https://pkgs.dev.azure.com/xamarin/public/_packaging/XamarinCommunityToolkitNightly/nuget/v3/index.json) feed to try out the latest and greatest.

| Build Server | Type         | Platform | Status |
|--------------|--------------|----------|--------|
| Azure DevOps         | Build        | Windows & Mac  | [![Build Status](https://dev.azure.com/xamarin/public/_apis/build/status/xamarin/CommunityToolkit/xamarin.XamarinCommunityToolkit%20(Public)?branchName=main)](https://dev.azure.com/xamarin/public/_build?definitionId=55&_a=summary) |

## Sample App

Browsing the [sample app](./samples) is the best place to start exploring what's available today.

## Installation

The Toolkit is available via NuGet, and should be installed into all of your projects (shared and individual platforms):

* NuGet Official Releases: [![NuGet](https://img.shields.io/nuget/vpre/Xamarin.CommunityToolkit.svg?label=NuGet)](https://www.nuget.org/packages/Xamarin.CommunityToolkit/)
* NuGet Nightly Releases: [![NuGet Nightly](https://img.shields.io/badge/NuGet-Nightly-yellow)](https://pkgs.dev.azure.com/xamarin/public/_packaging/XamarinCommunityToolkitNightly/nuget/v3/index.json)

Browse with the NuGet manager in your IDE to install them or run this command:

`Install-Package Xamarin.CommunityToolkit`

## Getting Started

After installation, start using the features you're after.

If you're using XAML, you can add this namespace to your root node to get access to all the goodness the Toolkit has to offer without having to add all kinds of namespaces seperately: `xmlns:xct="http://xamarin.com/schemas/2020/toolkit"`. I.e.:

```xaml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms" xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
xmlns:xct="http://xamarin.com/schemas/2020/toolkit">

<xct:AvatarView />

<!-- The rest of your page here -->

</ContentPage>
```

## Documentation

The documentation is currently under construction at the [official Docs repo](https://github.com/MicrosoftDocs/xamarin-communitytoolkit). We're working to get these published as soon as possible. For now, please refer to the markdown files that are in the repo. Contributions to the Docs are very much welcomed!

## Contributions welcome!

If you have one or more of these common pieces of code that you are always replicating across apps, don't hesitate to contribute! We aim to be the first NuGet package you install when creating a new Xamarin.Forms app!

Please have a look at our [contribution guide](CONTRIBUTING.md) before you get started, as well as some information on [the wiki](https://github.com/xamarin/XamarinCommunityToolkit/wiki/Contributing-to-XamarinCommunityToolkit). Also take note of the [Code of Conduct](https://dotnetfoundation.org/code-of-conduct) we adhere to.

## Project Structure

To structure our project, we have adopted a range of namespaces. You can find them below with a short description of what you will find where. For simply consuming the Toolkit in XAML you can use our simplified namespace. See the Getting Started section above.

| Namespace | Description |
|--------------|--------------|
| Xamarin.CommunityToolkit.Core | Core objects that do not fit other namespaces and are used by multiple other components |
| Xamarin.CommunityToolkit.Behaviors | Extended behaviors for the Xamarin.Forms components |
| Xamarin.CommunityToolkit.Converters | XAML converters, used to converted your data binding data into something your XAML understands |
| Xamarin.CommunityToolkit.Effects | Effects to apply light-weight renderer changes to the Xamarin.Forms renderers |
| Xamarin.CommunityToolkit.Extensions | XAML Markup Extensions to make your XAML even more functional |
| Xamarin.CommunityToolkit.ObjectModel | Things that have to do with your models and objects. Probably handy for your MVVM needs |
| Xamarin.CommunityToolkit.UI.Views | Controls such as TabView, etc. |
| Xamarin.CommunityToolkit.SampleApp | Sample App where you can find all of the above for reference. Learn how to use them and see how it all looks |
| Xamarin.CommunityToolkit.UnitTests | This is where our unit tests live. Please keep growing them! |

## Code of Conduct
As a part of the .NET Foundation, we have adopted the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct). Please familiarize yourself with that before participating with this repository. Thanks!

## .NET Foundation
This project is supported by the [.NET Foundation](https://dotnetfoundation.org).
