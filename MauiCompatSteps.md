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
1. Remove `InvariantGlobalization`, `RuntimeIdentifier` and `UseInterpreter` from `Xamarin.CommunityToolkit.MauiCompat.csproj`:
    ```bash
    awk '!/InvariantGlobalization/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj > temp && mv temp ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj
    awk '!/RuntimeIdentifier/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj > temp && mv temp ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj
    awk '!/UseInterpreter/' ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj > temp && mv temp ./src/CommunityToolkit/Xamarin.CommunityToolkit.MauiCompat/Xamarin.CommunityToolkit.MauiCompat.csproj
    ```

2. Remove `InvariantGlobalization`, `RuntimeIdentifier` and `UseInterpreter` from `Xamarin.CommunityToolkit.Markup.MauiCompat.csproj`:
    ```bash
    awk '!/InvariantGlobalization/' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj > temp && mv temp ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj
    awk '!/RuntimeIdentifier/' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj > temp && mv temp ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj
    awk '!/UseInterpreter/' ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj > temp && mv temp ./src/Markup/Xamarin.CommunityToolkit.Markup.MauiCompat/Xamarin.CommunityToolkit.Markup.MauiCompat.csproj
    ```

## 4. Create `Directory.build.props`

1. Create `Directory.build.props` for `Xamarin.CommunityToolkit.MauiCompat`
    ```bash
    printf > ./src/CommunityToolkit/Directory.build.props "<PropertyGroup>
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
    </PropertyGroup>

    <ItemGroup>
        <EnableDefaultCompileItems>false</EnableDefaultCompileItems>
        <Compile Include=\"**/*.shared.cs\" />
        <Compile Include=\"**/*.shared.*.cs\" />    
        <None Include=\"..\..\LICENSE\" PackagePath=\"\" Pack=\"true\" />
        <None Include=\"..\..\assets\XamarinCommunityToolkit_128x128.png\" PackagePath=\"icon.png\" Pack=\"true\" />
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
    </ItemGroup>"
    ```

2. Create `Directory.build.props` for `Xamarin.CommunityToolkit.Markup.MauiCompat`
    ```bash
    printf > ./src/Markup/Directory.build.props "<PropertyGroup>
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

### 5. Update Namespaces

*TBD*

### 6. Remove Deprecated Code

*TBD*