# ValueConverterGroup

Multiple converters.

Conversion values using the sequential conversion of each of the converters from the first to the last.

## Syntax

Define in ResourceDictionary:
```xml
<converters:ValueConverterGroup x:Key="IsEmptyInverse">
    <converters:IsEmptyConverter />
    <converters:InvertedBooleanConverter />
</converters:ValueConverterGroup>
```

## Example

```xml
<Button Text="Save"
        Command="{Binding AddItemCommand}"
        IsVisible="{Binding ItemName, Converter={StaticResource IsEmptyInverse}}" />
```

