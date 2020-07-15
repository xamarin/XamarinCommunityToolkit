Xamarin Community Toolkit
===========
Members of the community are rebooting this library under the guidance of the Xamarin team. The Xamarin Community Toolkit is a collection of common elements for mobile development with Xamarin.Forms that people tend to replicate across multiple apps. It simplifies and demonstrates common developer tasks when building apps with Xamarin.Forms. If you're looking for the historical code to reference, please check the **legacy** branch.

## Build Status

If you like to live dangerously (and while we are preparing our v1), you can use our [nightly](https://pkgs.dev.azure.com/xamarin/public/_packaging/XamarinCommunityToolkitNightly/nuget/v3/index.json) feed to try out packages right now.

| Build Server | Type         | Platform | Status |
|--------------|--------------|----------|--------|
| Azure DevOps         | Build        | Windows & Mac  | [![Build Status](https://dev.azure.com/xamarin/public/_apis/build/status/xamarin/CommunityToolkit/xamarin.XamarinCommunityToolkit%20(Public)?branchName=main)](https://dev.azure.com/xamarin/public/_build?definitionId=55&_a=summary)                                                  |

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
| Microsoft.Toolkit.Xamarin.Forms | Main NuGet package includes code only helpers, etc. |
| Microsoft.Toolkit.Xamarin.Forms.UI | UI Packages - XAML converters, Visual tree extensions, and other extensions and helpers for your XAML UI |
| Microsoft.Toolkit.Xamarin.Forms.UI.Controls | Controls such as TabView, etc.|
| Microsoft.Toolkit.Xamarin.Forms.UI.Layout | Layouts such as WrapLayout, etc. |
| Microsoft.Toolkit.Xamarin.Forms.SampleApp | Sample App where you can find all of the above for reference. How to use them and see how it looks |
| Microsoft.Toolkit.Xamarin.Forms.UnitTests | This is where our unit tests live. Please keep growing them! |