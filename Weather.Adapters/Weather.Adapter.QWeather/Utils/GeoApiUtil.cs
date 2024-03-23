using Weather.Core.Standards.WebApi;

namespace Weather.Adapter.QWeather.Utils;

internal static class GeoApiUtil
{
    public static IApiConfigProvider InitializeGeoApi(this IApiConfigProvider apiConfig)
    {
        apiConfig.Protocol = ApiProtocols.Https;

        apiConfig.Host = "geoapi.qweather.com";

        apiConfig.Version = "v2";

        return apiConfig;
    }
}
