namespace Temp

open Xamarin.Forms

type App() =
    inherit Application()

    let stack = StackLayout(VerticalOptions = LayoutOptions.Center)
    let label = Label(XAlign = TextAlignment.Center, Text = "Welcome to F# Xamarin.Forms!")
    do
        stack.Children.Add(label)
        base.MainPage <- ContentPage(Content = stack)
