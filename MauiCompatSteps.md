# Maui Compat Steps

The following steps are used to create the MauiCompat libraries

### 1. Create Xamarin.CommunityToolit.MauiCompat Branch From Latest Tag

1. `git checkout -b Xamarin.CommunityToolkit.MauiCompat-[version] [tag]`
> **Example:** `git checkout -b Xamarin.CommunityToolkit.MauiCompat-1.3.0-pre2 1.3.0-pre2`
2. `git push`

### 2. Create `Xamarin.CommunityToolkit.MauiCompat` and `Xamarin.CommunityToolkit.Markup.MauiCompat` 

1. `dotnet new maui -n Xamarin.CommunityToolkit.MauiCompat ./src/CommunityToolkit`
2. `dotnet new maui -n Xamarin.CommunityToolkit.Markup.MauiCompat ./src/Markup`

### 3. Copy/Paste `Xamarin.CommunityToolkit` to `Xamarin.CommunityToolkit.MauiCompat`

*TBD*

### 4. Update Namespaces

*TBD*

### 5. Remove Deprecated Code

*TBD*