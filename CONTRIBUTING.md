# Contributing

Thank you for your interest in contributing to XamarinCommunityToolkit! In this document we'll outline what you need to know about contributing and how to get started.

First and foremost: we're all friends here. Whether you are a first-time contributor or a core team member from one of the associated projects, we welcome any and all people to contribute to our lovely little project. I mean, it is called *community* toolkit after all.

Having that said, if you are a first-timer and you could use some help please reach out to any core member. They will be happy to help you out or find someone who can.

Furthermore, for anyone, we would like you to take into consideration the following guidelines.

### Make an effort to be nice

If you disagree, that's fine. We don't think about everything the same way, be respectful and at some point decide to agree to disagree. If a decision needs to be made, try to involve at least one other person without continuing an endless discussion

When you disagree with a piece of code that is written, try to be helpful and explain why you disagree or how things can be improved (according to you). Always remember there are numerous ways to solve things, there is not one right way, but it's always good to learn about alternatives

During a code review try to make a habit out of it to say at least one nice thing. Obviously about something you like in the code. If a change is not that big or so straight-forward that you can't comment nicely on that, find something else to compliment the person. Make an effort to look at their profile of blog and mention something you like, make that persons day a bit better! <3

### Make an effort to see it from their perspective

Remember English is not everyones native language. Written communication always lacks non-verbal communication. With written communication in a language that is not your native tongue it is even harder to express certain emotions.

Always assume that people mean to do right. Try to read a sentence a couple of times over and take things more literal. Try to place yourself in their shoes and see the message beyond the actual words. 

Things might come across different than they were intended, please keep that in mind and always check to see how someone meant it. If you're not sure, pull someone offline in a private channel on Twitter or email and chat about it for a bit. Maybe even jump on a call to collaborate. We're living in the 21st century, all the tools are there, why not use them to get to know each other and be friends?!

Besides language, we understand that contributing to open-source mostly happens in your spare time. Remember that priorities might change and we can only spend our time once. This works as a two-way street: don't expect things to be solved instantly, but also please let us know if you do not have the capacity to finish work you have in progress. There is no shame in that. That wat it's clear to other people that they can step in and take over.

### THANK YOU!

Lastly, a big thank you for spending your precious time on our project. We appreciate any effort you make to help us with this project.

## Code of Conduct

Please see our [Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

As should be clear by now: we assume everyone tries to do their best, everyone should be treated with respect and equally.

In the unfortunate event that doesn't happen, please feel free to report it to any of the team members or reach out to [Gerald](maillo:gerald.versluis@microsoft.com) directly.

We will take appropriate actions and measures if necessary.

## Prerequisites

You will need to complete a Contribution License Agreement before any pull request can be accepted. Complete the CLA at https://cla.dotnetfoundation.org/. This will also be triggered whenever you open a PR and the link should guide you through it.

## Opening a PR process

### TL;DR
* Find an issue/feature, make sure that the issue/feature has been approved and is welcomed (also see [Proposal States](#Proposal-States))
* Fork repository
* Create branch
* Implement
* Open a PR
* We merge
* High-fives all-around

### Please consider

#### Tabs vs. Spaces?!
[Tabs](https://www.reddit.com/r/javascript/comments/c8drjo/nobody_talks_about_the_real_reason_to_use_tabs/).

#### Make your changes small, don't keep adding
We love your enthusiasm, but small changes and small PRs are easier to digest. We're all doing this in our spare time, it is easier to review a couple of small things and merge that and iterate from there than to have a PR with 100+ files changed that will sit there forever

#### Added features should have tests and a sample
We like quality as much as the next person, so please provide tests where possible.

In addition, we would want a new feature or change to be as clear as possible for other developers. Please add a sample to the sample app as part of your PR.

## Contributing Code - Best Practices

### Enums
* Always use `Unknown` at index 0 for return types that may have a value that is not known
* Always use `Default` at index 0 for option types that can use the system default option
* Follow naming guidelines for tense... `SensorSpeed` not `SensorSpeeds`
* Assign values (0,1,2,3) for all enums

### Property Names
* Include units only if one of the platforms includes it in their implementation. For instance HeadingMagneticNorth implies degrees on all platforms, but PressureInHectopascals is needed since platforms don't provide a consistent API for this.

### Units
* Use the standard units and most well accepted units when possible. For instance Hectopascals are used on UWP/Android and iOS uses Kilopascals so we have chosen Hectopascals.

### Style
* Prefer using `==` when checking for null instead of `is`

<!-- ### Exceptions

We currently prefer different ways of indicating that nothing can be done:

 - do nothing
 - throw `FeatureNotSupportedException`
 - throw `PlatformNotSupportedException`
 - throw `FeatureNotEnabledException`

One case where we throw `FeatureNotSupportedException` is with the sensors: if there is no sensor X, then we throw.

One case (and the only case so far) where we throw `PlatformNotSupportedException` is in Android's text-to-speech API: if we try and speak, but we couldn't initialize, then we throw.

So far, I was able to determine that we throw `FeatureNotSupportedException` for:
 - the sensors on all platforms if we aren't able to access the hardware
    - we throw in the start and the stop (this one may be overkill, we can probably first check to see if it is started, and if not then just do nothing)
 - the Android external browser if there was no browser installed
 - the email API
    - Android: if there is no `message/rfc822` intent handler
    - iOS: (if the mail VC can't send, or if the `mailto:` doesn't have an app, or if trying to send HTML over the `mailto:` protocol
    - UWP: if the `EmailManager` is not available, or if trying to send HTML
 - the flashlight API on all platforms if there is no camera flash hardware
 - the phone dialler 
    - Android / iOS: if the OS can't handle the `tel:` protocol
    - UWP: the `PhoneCallManager` is missing
 - the sms API
    - Android: if there is no `smsto:` intent handler
    - iOS: (if the message VC can't send
    - UWP: if the `ChatMessageManager` is not available
 - the vibration API on UWP if the `VibrationDevice` is not available or if no hardware was found

We throw a `PlatformNotSupportedException` for:
 - Android when we aren't able to initialize the text-to-speech engine

We throw a `FeatureNotEnabledException` for:
 - Geolocation if no providers are found

We do "nothing":
 - the Vibration API on iOS and android never actually checks, it just starts it
 - the Map API on Android and UWP just starts the URI, assuming that something will be there
 - the Geolocation API always assumes that there is a GPS and throws a `FeatureNotEnabledException` if there was no way to get the hardware
 - the KeepScreenOn feature just assumes the window flag will be honoured (probably is, but is there an api level/hardware limit?)
 - the energy saver API on android pre-Lollipop 

## Documentation - mdoc

This project uses [mdoc](http://www.mono-project.com/docs/tools+libraries/tools/monodoc/generating-documentation/) to document types, members, and to add small code snippets and examples.  mdoc files are simple xml files and there is an msbuild target you can invoke to help generate the xml placeholders.

Read the [Documenting your code with mdoc wiki page](https://github.com/xamarin/Essentials/wiki/Documenting-your-code-with-mdoc) for more information on this process.

Every pull request which affects public types or members should include corresponding mdoc xml file changes.-->

### Bug Fixes

If you're looking for something to fix, please browse [open issues](https://github.com/xamarin/XamarinCommunityToolkit/issues). 

Follow the style used by the [.NET Foundation](https://github.com/dotnet/runtime/blob/master/docs/coding-guidelines/coding-style.md), with two primary exceptions:

- We do not use the `private` keyword as it is the default accessibility level in C#.
- We will **not** use `_` or `s_` as a prefix for internal or private field names
- We will use `camelCaseFieldName` for naming internal or private fields in both instance and static implementations

Read and follow our [Pull Request template](https://github.com/xamarin/XamarinCommunityToolkit/blob/main/.github/PULL_REQUEST_TEMPLATE.md)

### Proposals

To propose a change or new feature, review the guidance below and then [open an issue using this template](https://github.com/xamarin/XamarinCommunityToolkit/issues/new).

#### Non-Starter Topics
The following topics should generally not be proposed for discussion as they are non-starters:

* Large renames of APIs
* Large non-backward-compatible breaking changes
* Platform-Specifics which can be accomplished without changing XamarinCommunityToolkit
* Avoid clutter posts like "+1" which do not serve to further the conversation, please use the emoji resonses for that

#### Guiding Principles for New Features

Any proposals for new feature work and new APIs should follow the spirit of these principles:

 * APIs should be simple, direct, and generally implemented with static classes and methods whenever practical
 * New features should have native APIs available to allow implementation on a reasonable subset of the supported platforms, especially  (iOS, Android, UWP)
 * No new external dependencies should be added to support implementation of new feature work (there can be exceptions but they must be thoroughly considered for the value being added)

#### Approval Process
* Provide as much detail as possible so the team and community can have a good discussion about your proposal.
* If the proposal is for a complex issue more detailed specifications might need to be created.
* For especially large proposals consider how it could be broken up into multiple proposals to make it easier to review. 
* When you think your proposal is ready to be implemented ask for approval from the XamarinCommunityToolkit team.
* One or more approvals from XamarinCommunityToolkit team are required to approve a proposal. The number of required approvers will be based on the size and complexity of the proposal.
* Once the proposal is approved by the XamarinCommunityToolkit team you can ask to be assigned the proposal and you can start on a PR.

#### Proposal States
##### Open
Open proposals are still under discussion. Please leave your concrete, constructive feedback on this proposal. +1s and other clutter posts which do not add to the discussion will be removed.

##### Accepted
Accepted proposals are proposals that both the community and core XamarinCommunityToolkit team agree should be a part of this toolkit. These proposals are ready for implementation, but do not yet have a developer actively working on them. These proposals are available for anyone to work on, both community and the core XamarinCommunityToolkit team.

If you wish to start working on an accepted proposal, please reply to the thread so we can mark you as the implementor and change the title to In Progress. This helps to avoid multiple people working on the same thing. If you decide to work on this proposal publicly, feel free to post a link to the branch as well for folks to follow along.

###### What "Accepted" does mean
* Any community member is welcome to work on the idea.
* The core XamarinCommunityToolkit team _may_ consider working on this idea on their own, but has not done so until it is marked "In Progress" with a team member assigned as the implementor.
* Any pull request implementing the proposal will be welcomed with an API and code review.

###### What "Accepted" does not mean
* The proposal will ever be implemented, either by a community member or by the core XamarinCommunityToolkit team.
* The core XamarinCommunityToolkit team is committing to implementing a proposal, even if nobody else does. Accepted proposals simply mean that the core XamarinCommunityToolkit team and the community agree that this proposal should be a part of XamarinCommunityToolkit.

##### In Progress
Once a developer has begun work on a proposal, either from the core XamarinCommunityToolkit team or a community member, the proposal is marked as in progress with the implementors name and (possibly) a link to a development branch to follow along with progress.

#### Rejected
Rejected proposals will not be implemented or merged into XamarinCommunityToolkit. Once a proposal is rejected, the thread will be closed and the conversation is considered completed, pending considerable new information or changes.
