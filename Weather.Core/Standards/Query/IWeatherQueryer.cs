using System.ComponentModel.Composition;
using Weather.Core.Models;
using Weather.Core.Standards.WebApi;

namespace Weather.Core.Standards.Query;

[InheritedExport]
public interface IWeatherQueryer : IAdapter
{
    public Task<WeatherInfo?> QueryCurrentWeather(string location, IApiConfigProvider apiConfig);
}
