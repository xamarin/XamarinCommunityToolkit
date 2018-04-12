Xamarin Community Toolkit
===========

The Xamarin Community Toolkit for Xamarin.Forms is a collection of Animations, Behaviors, Converters, and Effects for mobile development with [Xamarin.Forms](https://github.com/xamarin/Xamarin.Forms). It simplifies and demonstrates common developer tasks building iOS, Android, and UWP apps with Xamarin.Forms.

## Build status

| Target | Branch | Status |
| ------ | ------ | ------ | 
| Toolkit | master |![Build status](https://devdiv.visualstudio.com/_apis/public/build/definitions/0bdbc590-a062-4c3f-b0f6-9383f67865ee/8450/badge)  |

| Toolkit | NuGet | 
| ------ | ------ |
| Animations | [![NuGet](https://img.shields.io/nuget/v/Xamarin.Toolkit.Animations.svg?label=NuGet)](https://www.nuget.org/packages/Xamarin.Toolkit.Animations/) |
| Behaviors | [![NuGet](https://img.shields.io/nuget/v/Xamarin.Toolkit.Behaviors.svg?label=NuGet)](https://www.nuget.org/packages/Xamarin.Toolkit.Behaviors/) |
| Converters | [![NuGet](https://img.shields.io/nuget/v/Xamarin.Toolkit.Converters.svg?label=NuGet)](https://www.nuget.org/packages/Xamarin.Toolkit.Converters/) |
| Effects | [![NuGet](https://img.shields.io/nuget/v/Xamarin.Toolkit.Effects.svg?label=NuGet)](https://www.nuget.org/packages/Xamarin.Toolkit.Effects/) |

Development NuGet feed: https://www.myget.org/F/xamarin-toolkit/api/v3/index.json

## Contributing
Do you want to contribute? Here are our [contribution guidelines](CONTRIBUTING.md).

## Building Xamarin Community Toolkit
Xamarin Community Toolkit is built with the new SDK style projects with multi-targeting enabled. This means that all code for iOS, Android, and UWP exist inside of the Xamarin Community Toolkit project.

If building on Visual Studio 2017 simply open the solution and build the project.

If using Visual Studio for Mac the project can be built at the command line with MSBuild. To change the project type that you are working with simply edit Toolkit.csproj and modify the TargetFrameworks for only the project type you want to use.

## Xamarin Community Toolkit Sample App

Want to see the toolkit in action before jumping into the code?  Download and play with the [Sample App](/Samples).

Follow the build instructions above to build the full library.

## Features and documentation

Documentation for our features is currently being transfered to http://docs.microsoft.com. Until this migration is complete, you can find the current Markdown files under [/docs](/docs).

### Animations

### Behaviors

### Converters

### Effects


## Feedback and Requests

Please use [GitHub issues](https://github.com/xamarin/XamarinCommunityToolkit/issues) for questions, comments, feature requests, or if would like to vote on what others are recommending.

## Principles

 - Principle #1: The toolkit will be kept simple.
 - Principle #2: As soon as a comparable feature is available in Xamarin.Forms, it will be marked as deprecated.
 - Principle #3: All features will be supported for two Xamarin.Forms release cycles or until another principle supersedes it.


For more information see the [Code of Conduct](CODE_OF_CONDUCT). 

## License
MIT License, see license file.
