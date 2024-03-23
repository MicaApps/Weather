using Weather.Core.Standards.WebApi;

namespace Weather.Adapter.QWeather.Utils;

public static class WeatherApiUtil
{
    public static IApiConfigProvider InitializeWeatherApi(this IApiConfigProvider apiConfig)
    {
        apiConfig.Protocol = ApiProtocols.Https;

#if !DEBUG
        apiConfig.Host = "api.qweather.com";
#else
        apiConfig.Host = "devapi.qweather.com";
#endif

        apiConfig.Version = "v7";

        return apiConfig;
    }
}
