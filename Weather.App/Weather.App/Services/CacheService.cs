using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Weather.Core.Models;

namespace Weather.App.Services;

public class CacheService
{
    private static object _instance = new();

    public static CacheService Instance
    {
        get
        {
            if (_instance is not CacheService)
                _instance = new CacheService();

            return _instance as CacheService;
        }
    }

    public static string CacheFolder => Path.GetDirectoryName(Environment.ProcessPath);

    private static readonly JsonSerializerOptions serializerOptions = new()
    {
        WriteIndented = true,
        IncludeFields = true,
    };

    private CacheService()
    {
        var cityCache = Path.Combine(CacheFolder, "Weather.App.Cache.Cities.json");
        var weatherCache = Path.Combine(CacheFolder, "Weather.App.Cache.Weather.json");
        var dailyWeatherCache = Path.Combine(CacheFolder, "Weather.App.Cache.DailyWeather.json");

        if (File.Exists(cityCache))
            cityCaches = JsonSerializer.Deserialize<Dictionary<string, CityInfo>>(File.ReadAllText(cityCache));

        if (File.Exists(weatherCache))
            weatherCaches = JsonSerializer.Deserialize<Dictionary<string, WeatherCache>>(File.ReadAllText(weatherCache));

        if (File.Exists(dailyWeatherCache))
            dailyWeatherForecastCaches = JsonSerializer.Deserialize<Dictionary<string, List<WeatherCache>>>(File.ReadAllText(dailyWeatherCache));
    }

    public CacheService Initialize()
    {
        if (Directory.Exists(CacheFolder) == false)
            Directory.CreateDirectory(CacheFolder);

        return this;
    }

    public CacheService SaveCache()
    {
        var cityCache = Path.Combine(CacheFolder, "Weather.App.Cache.Cities.json");
        var weatherCache = Path.Combine(CacheFolder, "Weather.App.Cache.Weather.json");
        var dailyWeatherCache = Path.Combine(CacheFolder, "Weather.App.Cache.DailyWeather.json");

        File.WriteAllText(cityCache, JsonSerializer.Serialize(cityCaches, serializerOptions));
        File.WriteAllText(weatherCache, JsonSerializer.Serialize(weatherCaches, serializerOptions));
        File.WriteAllText(dailyWeatherCache, JsonSerializer.Serialize(dailyWeatherForecastCaches, serializerOptions));

        return this;
    }

    private readonly Dictionary<string, CityInfo> cityCaches = [];

    public CacheService AddCity(string id, CityInfo cityInfo)
    {
        if (cityCaches.ContainsKey(id))
            cityCaches[id] = cityInfo;
        else
            cityCaches.Add(id, cityInfo);

        SaveCache();

        return this;
    }

    public bool TryQueryCity(string id, out CityInfo info)
    {
        if (cityCaches.ContainsKey(id))
        {
            info = cityCaches[id];

            return true;
        }

        info = null;

        return false;
    }

    private readonly Dictionary<string, WeatherCache> weatherCaches = [];

    public CacheService AddWeather(string location, WeatherInfo weatherInfo)
    {
        if (weatherCaches.ContainsKey(location))
            weatherCaches[location].Refresh(weatherInfo);
        else
        {
            weatherCaches.Add(
                location,
                new WeatherCache
                {
                    Location = location,
                }.Refresh(weatherInfo)
            );
        }

        SaveCache();

        return this;
    }

    public bool TryQueryWeather(string location, out WeatherInfo info)
    {
        if (weatherCaches.ContainsKey(location))
        {
            info = weatherCaches[location].WeatherInfo;

            return true;
        }

        info = null;

        return false;
    }

    private readonly Dictionary<string, List<WeatherCache>> dailyWeatherForecastCaches = [];

    public CacheService AddDailyWeatherForecast(string location, IEnumerable<WeatherInfo> weatherInfos)
    {
        var list = weatherInfos.Select(
            x => new WeatherCache
            {
                Location = location,
            }.Refresh(x)
        ).ToList();

        if (dailyWeatherForecastCaches.ContainsKey(location))
            dailyWeatherForecastCaches[location] = list;
        else
            dailyWeatherForecastCaches.Add(location, list);

        SaveCache();

        return this;
    }

    public bool TryQueryDailyWeatherForecast(string location, out IEnumerable<WeatherInfo> infos)
    {
        if (dailyWeatherForecastCaches.ContainsKey(location))
        {
            infos = dailyWeatherForecastCaches[location].Select(x => x.WeatherInfo);

            return true;
        }

        infos = null;

        return false;
    }
}
