
Xamarin.Forms.DataGrid
======================

[![Build Status](https://www.bitrise.io/app/e72afbfab5432e1d/status.svg?token=Sxj77MEd-bke8RUGugsR2Q)](https://www.bitrise.io/app/e72afbfab5432e1d)

DataGrid library for Xamarin.Forms Application.

>You can install the package from **NuGet Package Manager**  [(Xamarin.Forms.DataGrid)](https://www.nuget.org/packages/Xamarin.Forms.DataGrid/)

```sh
    pm> Install-Package Xamarin.Forms.DataGrid
```
    
> **Supported Platforms**
  >- Android
  >- iOS
  >- UWP
  >- Windows Store(WinRT)
  >- Windows Phone

#### <i class="icon-pencil"></i> Usage ([See all](https://github.com/akgulebubekir/Xamarin.Forms.DataGrid/blob/master/Samples.md))

> You should call `Xamarin.Forms.DataGrid.DataGridComponent.Init()` before using from Xaml.


```xaml
    xmlns:dg="clr-namespace:Xamarin.Forms.DataGrid;assembly=Xamarin.Forms.DataGrid"
    xmlns:conv="clr-namespace:DataGridSample.Views.Converters;assembly=DataGridSample"

  <dg:DataGrid ItemsSource="{Binding Teams}" SelectionEnabled="True" SelectedItem="{Binding SelectedTeam}"
               RowHeight="70" HeaderHeight="50" BorderColor="#CCCCCC" HeaderBackground="#E0E6F8"
               PullToRefreshCommand="{Binding RefreshCommand}" IsRefreshing="{Binding IsRefreshing}">
  <dg:DataGrid.HeaderFontSize>
    <OnIdiom  x:TypeArguments="x:Double">
      <OnIdiom.Tablet>15</OnIdiom.Tablet>
      <OnIdiom.Phone>13</OnIdiom.Phone>
    </OnIdiom>
  </dg:DataGrid.HeaderFontSize>
  <dg:DataGrid.Columns>
        <dg:DataGridColumn Title="Logo" PropertyName="Logo" Width="100">
          <dg:DataGridColumn.CellTemplate>
            <DataTemplate>
              <Image Source="{Binding Logo}" HorizontalOptions="Center" VerticalOptions="Center" Aspect="AspectFit" HeightRequest="60" />
            </DataTemplate>
          </dg:DataGridColumn.CellTemplate>
        </dg:DataGridColumn>
        <dg:DataGridColumn Title="Team" PropertyName="Name" Width="2*"/>
        <dg:DataGridColumn Title="Win" PropertyName="Win" Width="0.95*"/>
        <dg:DataGridColumn Title="Loose" PropertyName="Loose"  Width="1*"/>
        <dg:DataGridColumn Title="Home" PropertyName="Home"/>
        <dg:DataGridColumn Title="Percentage" PropertyName="Percentage" StringFormat="{}{0:0.00}" />
        <dg:DataGridColumn Title="Streak" PropertyName="Streak" Width="0.7*">
          <dg:DataGridColumn.CellTemplate>
            <DataTemplate>
              <ContentView HorizontalOptions="Fill" VerticalOptions="Fill" 
                           BackgroundColor="{Binding Streak,Converter={StaticResource StreakToColorConverter}}">
                <Label Text="{Binding Streak}" HorizontalOptions="Center" VerticalOptions="Center" TextColor="Black"/>
              </ContentView>
            </DataTemplate>
          </dg:DataGridColumn.CellTemplate>
        </dg:DataGridColumn>
  </dg:DataGrid.Columns>
  <dg:DataGrid.RowsBackgroundColorPalette>
    <dg:PaletteCollection>
      <Color>#F2F2F2</Color>
      <Color>#FFFFFF</Color>
    </dg:PaletteCollection>
  </dg:DataGrid.RowsBackgroundColorPalette>
  <dg:DataGrid.Resources>
    <ResourceDictionary>
      <conv:StreakToColorConverter x:Key="StreakToColorConverter"/>
    </ResourceDictionary>
  </dg:DataGrid.Resources>
</dg:DataGrid>
```
Screenshots ([see all](https://github.com/akgulebubekir/Xamarin.Forms.DataGrid/tree/master/Screenshots))
-------------

![Screenshots](https://raw.githubusercontent.com/akgulebubekir/Xamarin.Forms.DataGrid/master/Screenshots/AllinOne.png)