# Weather

A fluent designed weather app built with WinUI 3.

With plugins system, you can custom weather data source and the UI.

## Requirements

Development environment:

- Windows 10 1809 or later (including Windows 11)
- Visual Studio:
    - .NET Desktop Development workload:
        - dotnet sdk 6.0
        - dotnet sdk 8.0 (optional)
    - Universal Windows Platform Development workload
    - C++ Desktop Development workload
    - Single Components:
        - Windows 10 SDK (10.0.18362.0)
        - Windows 11 SDK (10.0.22621.0)

To test functions:

- Api Key for below api providers:
    - [QWeather](https://www.qweather.com/)

## Structure

- Weather.App: The main project with WinUI 3 GUI
- Weather.App (Package): The package project for Weather.App
- Weather.Core: The core project with data models and plugin interfaces
- Weather.Adapters |
    - Weather.Adapter.QWeather: The adapter project with QWeather APIs

As you can see, the project is designed with a plugin system.

You can create your own adapter project to support more weather data sources.
