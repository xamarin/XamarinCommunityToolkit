# Converters

Add your namespace

```xml
xmlns:toolkit:"clr-namespace:XamarinCommunityToolkit.Converters;assembly=XamarinCommunityToolkit"
```

Add the converter to the `ResourceDictionary`

```xml
<Application.Resources>
    <ResourceDictionary>
        <toolkit:#Converter x:Key="#Converter">
    </ResourceDictionary>
</Application.Resources>
```

## BoolToObjectConverter

```xml
<toolkit:BoolToObjectConverter x:Key="BoolToObjectConverter">
```

Use it in your View:

```xml
<Span Text="{Binding SampleBool, Converter={StaticResource BoolToObjectConverter}}"/> 
```

## DoubleToIntConverter

## EqualConverter

## IndexToArrayItemConverter

## IntToBoolConverter

## InvertedBoolConverter

```xml
<toolkit:InvertedBoolConverter x:Key="InvertedBoolConverter">
```

```xml
<Span Text="{Binding SampleBool, Converter={StaticResource BoolToObjectConverter}}"/> 
```

## IsNotNullOrEmptyConverter

## IsNullOrEmptyConverter

## ListIsNotNullOrEmptyConverter

## ListIsNullOrEmptyConverter

## NotEqualConverter

## TextCaseConverter

## TextCaseConverterType
