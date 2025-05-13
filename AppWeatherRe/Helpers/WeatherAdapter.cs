#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppWeatherRe.Helpers;

public class WeatherAdapter
{
    static HttpClient client = new HttpClient() { BaseAddress = new Uri(@"https://api.weather.com/") };
    //private static string _systemLanguage;// = UserProfile.GlobalizationPreferences.Languages[0];

    public static async Task<SearchLocationResponse?> GetLocationInformationAsync(string cityName)
    {
        string api = $"v3/location/searchflat?query={cityName}&language={CultureInfo.CurrentCulture.Name}&apiKey=793db2b6128c4bc2bdb2b6128c0bc230&format=json";
        var response = await client.GetAsync(api);
        if (response.IsSuccessStatusCode)
        {
            var responseString = response.Content.ReadAsStringAsync().Result;
            var ret = System.Text.Json.JsonSerializer.Deserialize<SearchLocationResponse>(responseString);

            return ret;
        }
        return null;
    }

    /// <summary>
    /// 获取简易天气数据
    /// </summary>
    /// <param name="placeId"></param>
    /// <returns></returns>
    public static async Task<SimpleWeatherClass> GetSimpleWeatherAsync(string placeId)
    {
        var v3Response = await GetWeatherAsync(placeId);
        //v3Response.Dump();
        //v3Response.v3locationpoint.LocationV3.country +"-"+ v3Response.v3locationpoint.LocationV3.adminDistrict +"-"+ 
        //$"/WeatherIcons/{.IconImagePath}.png"
        return new SimpleWeatherClass() { CityName = v3Response.v3locationpoint.LocationV3.displayName, Temp = v3Response.v3wxobservationscurrent.temperature, State = v3Response.v3wxobservationscurrent.wxPhraseLong, PlaceId = placeId, IconImagePath = $"/WeatherIcons/{v3Response.v3wxobservationscurrent.iconCode}.png" };
    }

    private static async Task<RootV3Response?> GetWeatherAsync(string placeId)
    {
        string api = "v2/aggcommon/v3-wx-observations-current;v3-wx-forecast-hourly-10day;v3-wx-forecast-daily-10day;v3-location-point;v2idxDrySkinDaypart10;v2idxWateringDaypart10;v2idxPollenDaypart10;v2idxRunDaypart10;v2idxDriveDaypart10?format=json&placeid="
                + placeId
                + "&units=" + "m" //Enum.GetName(typeof(WetUnits),0);
                + "&language=" +
                CultureInfo.CurrentCulture.Name + "&apiKey=793db2b6128c4bc2bdb2b6128c0bc230";

        try
        {
            var responseString = await client.GetStringAsync(api);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // 注册时间类型支持所有格式
            options.Converters.Add(new FlexibleDateTimeOffsetConverter());
            options.Converters.Add(new FlexibleNullableDateTimeOffsetConverter());
            options.Converters.Add(new FlexibleDateTimeConverter());

            return System.Text.Json.JsonSerializer.Deserialize<RootV3Response>(responseString, options);
        }
        catch (Exception)
        {
            return null;
            //throw;
        }
    }
}

public record SimpleWeatherClass
{
    public string CityName { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public double Temp { get; set; }
    public double MinTemp { get; set; }
    public double MaxTemp { get; set; }
    public long TimeStamp { get; set; }
    public string PlaceId { get; set; } = string.Empty;
    public string IconImagePath { get; set; } = string.Empty;
}

#region RootV3Response

public class RootV3Response
{
    public string id { get; set; }

    [JsonPropertyName("v3-wx-observations-current")]
    public V3WxObservationsCurrent v3wxobservationscurrent { get; set; }

    [JsonPropertyName("v3-wx-forecast-daily-15day")]
    public V3WxForecastDaily v3wxforecastdaily15day { get; set; }

    [JsonPropertyName("v3-wx-forecast-daily-10day")]
    public V3WxForecastDaily v3wxforecastdaily10day { get; set; }

    [JsonPropertyName("v3-wx-forecast-hourly-10day")]
    public V3WxForecastHourly v3wxforecasthourly10day { get; set; }

    public object v3alertsHeadlines { get; set; }

    [JsonPropertyName("v3-location-point")] public V3LocationPoint v3locationpoint { get; set; }

    [JsonPropertyName("v2idxDriveDaypart10")] public V2idxDriveDaypartResult v2idxDriveDaypart10days { get; set; }

    [JsonPropertyName("v2idxRunDaypart10")] public V2idxRunDaypartResult v2idxRunDaypart10days { get; set; }

    [JsonPropertyName("v2idxPollenDaypart10")] public V2idxPollenDaypartResult v2idxPollenDaypart10days { get; set; }

    [JsonPropertyName("v2idxWateringDaypart10")]
    public V2idxWateringDaypartResult V2IdxWateringDaypart10days { get; set; }

    [JsonPropertyName("v2idxDrySkinDaypart10")]
    public V2idxDrySkinDaypartResult V2IdxDrySkinDaypart10days { get; set; }
}

public class Metadata
{
    public string language { get; set; }
    public string transactionId { get; set; }
    public string version { get; set; }
    public double latitude { get; set; }
    public double longitude { get; set; }
    public int expireTimeGmt { get; set; }
    public int statusCode { get; set; }
}

public class V2idxWateringDaypartResult
{
    public Metadata metadata { get; set; }

    [JsonPropertyName("wateringNeedsIndex12hour")]
    public WateringNeedsIndexDaypart WateringNeedsIndexDaypart { get; set; }
}

public class WateringNeedsIndexDaypart
{
    public List<int> fcstValid { get; set; }
    public List<DateTime> fcstValidLocal { get; set; }
    public List<string> dayInd { get; set; }
    public List<int> num { get; set; }
    public List<string> daypartName { get; set; }
    public List<int> wateringNeedsIndex { get; set; }
    public List<string> wateringNeedsCategory { get; set; }
}

public class V2idxDrySkinDaypartResult
{
    public Metadata metadata { get; set; }

    [JsonPropertyName("drySkinIndex12hour")] public DrySkinIndexDaypart DrySkinIndexDaypart { get; set; }
}

//dry skin index

public class DrySkinIndexDaypart
{
    public List<int> fcstValid { get; set; }
    public List<DateTime> fcstValidLocal { get; set; }
    public List<string> dayInd { get; set; }
    public List<int> num { get; set; }
    public List<string> daypartName { get; set; }
    public List<int> drySkinIndex { get; set; }
    public List<string> drySkinCategory { get; set; }
}

public class V2idxPollenDaypartResult
{
    public Metadata metadata { get; set; }

    [JsonPropertyName("pollenForecast12hour")] public PollenForecastDaypart PollenForecastDaypart { get; set; }
}

//pollen index

public class PollenForecastDaypart
{
    public List<int> fcstValid { get; set; }
    public List<DateTime> fcstValidLocal { get; set; }
    public List<string> dayInd { get; set; }
    public List<int> num { get; set; }
    public List<string> daypartName { get; set; }
    public List<int> grassPollenIndex { get; set; }
    public List<string> grassPollenCategory { get; set; }
    public List<int> treePollenIndex { get; set; }
    public List<string> treePollenCategory { get; set; }
    public List<int> ragweedPollenIndex { get; set; }
    public List<string> ragweedPollenCategory { get; set; }
}

public class V2idxRunDaypartResult
{
    public Metadata metadata { get; set; }

    [JsonPropertyName("runWeatherIndex12hour")]
    public RunWeatherIndexDaypart RunWeatherIndexDaypart { get; set; }
}

//running index

public class RunWeatherIndexDaypart
{
    public List<int> fcstValid { get; set; }
    public List<DateTimeOffset> fcstValidLocal { get; set; }
    public List<string> dayInd { get; set; }
    public List<int> num { get; set; }
    public List<string> daypartName { get; set; }
    public List<int> longRunWeatherIndex { get; set; }
    public List<string> longRunWeatherCategory { get; set; }
    public List<int> shortRunWeatherIndex { get; set; }
    public List<string> shortRunWeatherCategory { get; set; }
}

public class V2idxDriveDaypartResult
{
    public Metadata metadata { get; set; }

    [JsonPropertyName("drivingDifficultyIndex12hour")]
    public DrivingDifficultyIndexDaypart drivingDifficultyIndex12hour { get; set; }
}

//driving difficulty index

public class DrivingDifficultyIndexDaypart
{
    public List<int> fcstValid { get; set; }
    public List<DateTimeOffset> fcstValidLocal { get; set; }
    public List<string> dayInd { get; set; }
    public List<int> num { get; set; }
    public List<string> daypartName { get; set; }
    public List<int> drivingDifficultyIndex { get; set; }
    public List<string> drivingDifficultyCategory { get; set; }
}

public class V3LocationPoint
{
    [JsonPropertyName("location")] public LocationV3 LocationV3 { get; set; }
}

public class LocationV3
{
    public double latitude { get; set; }
    public double longitude { get; set; }
    public string city { get; set; }
    public Locale locale { get; set; }
    public string neighborhood { get; set; }
    public string adminDistrict { get; set; }
    public string adminDistrictCode { get; set; }
    public string postalCode { get; set; }
    public string postalKey { get; set; }
    public string country { get; set; }
    public string countryCode { get; set; }
    public string ianaTimeZone { get; set; }
    public string displayName { get; set; }
    public DateTimeOffset? dstEnd { get; set; }
    public DateTimeOffset? dstStart { get; set; }
    public string dmaCd { get; set; }
    public string placeId { get; set; }
    public bool disputedArea { get; set; }
    public object disputedCountries { get; set; }
    public object disputedCountryCodes { get; set; }
    public object disputedCustomers { get; set; }
    public List<bool> disputedShowCountry { get; set; }
    public string canonicalCityId { get; set; }
    public string countyId { get; set; }
    public string locId { get; set; }
    public object locationCategory { get; set; }
    public string pollenId { get; set; }
    public string pwsId { get; set; }
    public string regionalSatellite { get; set; }
    public object tideId { get; set; }
    public string type { get; set; }
    public string zoneId { get; set; }
}

public class V3WxForecastHourly
{
    public List<int> cloudCover { get; set; }
    public List<string> dayOfWeek { get; set; }
    public List<string> dayOrNight { get; set; }
    public List<int> expirationTimeUtc { get; set; }
    public List<int> iconCode { get; set; }
    public List<int> iconCodeExtend { get; set; }
    public List<int> precipChance { get; set; }
    public List<string> precipType { get; set; }
    public List<double> pressureMeanSeaLevel { get; set; }
    public List<double> qpf { get; set; }
    public List<double> qpfSnow { get; set; }
    public List<int> relativeHumidity { get; set; }
    public List<int> temperature { get; set; }
    public List<int> temperatureDewPoint { get; set; }
    public List<int?> temperatureFeelsLike { get; set; }
    public List<int> temperatureHeatIndex { get; set; }
    public List<int> temperatureWindChill { get; set; }
    public List<string> uvDescription { get; set; }
    public List<int> uvIndex { get; set; }
    public List<DateTimeOffset> validTimeLocal { get; set; }
    public List<int> validTimeUtc { get; set; }
    public List<double> visibility { get; set; }
    public List<int> windDirection { get; set; }
    public List<string> windDirectionCardinal { get; set; }
    public List<int?> windGust { get; set; }
    public List<int> windSpeed { get; set; }
    public List<string> wxPhraseLong { get; set; }
    public List<string> wxPhraseShort { get; set; }
    public List<int> wxSeverity { get; set; }
}

public class V3WxForecastDaily
{
    public List<int?> calendarDayTemperatureMax { get; set; }
    public List<int> calendarDayTemperatureMin { get; set; }
    public List<string> dayOfWeek { get; set; }
    public List<int> expirationTimeUtc { get; set; }
    public List<string> moonPhase { get; set; }
    public List<string> moonPhaseCode { get; set; }
    public List<int> moonPhaseDay { get; set; }
    public List<DateTimeOffset?> moonriseTimeLocal { get; set; }
    public List<int?> moonriseTimeUtc { get; set; }
    public List<DateTimeOffset?> moonsetTimeLocal { get; set; }
    public List<int?> moonsetTimeUtc { get; set; }
    public List<string> narrative { get; set; }
    public List<double?> qpf { get; set; }
    public List<double> qpfSnow { get; set; }
    public List<DateTimeOffset?> sunriseTimeLocal { get; set; }
    public List<int?> sunriseTimeUtc { get; set; }
    public List<DateTimeOffset?> sunsetTimeLocal { get; set; }
    public List<int?> sunsetTimeUtc { get; set; }
    public List<int?> temperatureMax { get; set; }
    public List<int> temperatureMin { get; set; }
    public List<DateTimeOffset> validTimeLocal { get; set; }
    public List<int> validTimeUtc { get; set; }
    public List<Daypart> daypart { get; set; }
}

public class Daypart
{
    public List<int?> cloudCover { get; set; }
    public List<string> dayOrNight { get; set; }
    public List<string> daypartName { get; set; }
    public List<int?> iconCode { get; set; }
    public List<int?> iconCodeExtend { get; set; }
    public List<string> narrative { get; set; }
    public List<int?> precipChance { get; set; }
    public List<string> precipType { get; set; }
    public List<double?> qpf { get; set; }
    public List<double?> qpfSnow { get; set; }
    public List<string> qualifierCode { get; set; }
    public List<string> qualifierPhrase { get; set; }
    public List<int?> relativeHumidity { get; set; }
    public List<string> snowRange { get; set; }
    public List<int?> temperature { get; set; }
    public List<int?> temperatureHeatIndex { get; set; }
    public List<int?> temperatureWindChill { get; set; }
    public List<string> thunderCategory { get; set; }
    public List<int?> thunderIndex { get; set; }
    public List<string> uvDescription { get; set; }
    public List<int?> uvIndex { get; set; }
    public List<int?> windDirection { get; set; }
    public List<string> windDirectionCardinal { get; set; }
    public List<string> windPhrase { get; set; }
    public List<int?> windSpeed { get; set; }
    public List<string> wxPhraseLong { get; set; }
    public List<string> wxPhraseShort { get; set; }
}

public class V3WxObservationsCurrent
{
    public int? cloudCeiling { get; set; }
    public string cloudCoverPhrase { get; set; }
    public string dayOfWeek { get; set; }
    public string dayOrNight { get; set; }
    public int expirationTimeUtc { get; set; }
    public int iconCode { get; set; }
    public int iconCodeExtend { get; set; }
    public object obsQualifierCode { get; set; }
    public object obsQualifierSeverity { get; set; }
    public double precip1Hour { get; set; }
    public double precip6Hour { get; set; }
    public double precip24Hour { get; set; }
    public double pressureAltimeter { get; set; }
    public double pressureChange { get; set; }
    public double pressureMeanSeaLevel { get; set; }
    public int pressureTendencyCode { get; set; }
    public string pressureTendencyTrend { get; set; }
    public int relativeHumidity { get; set; }
    public double snow1Hour { get; set; }
    public double snow6Hour { get; set; }
    public double snow24Hour { get; set; }
    public DateTimeOffset sunriseTimeLocal { get; set; }
    public int sunriseTimeUtc { get; set; }
    public DateTimeOffset sunsetTimeLocal { get; set; }
    public int sunsetTimeUtc { get; set; }
    public int temperature { get; set; }
    public int temperatureChange24Hour { get; set; }
    public int temperatureDewPoint { get; set; }
    public int temperatureFeelsLike { get; set; }
    public int temperatureHeatIndex { get; set; }
    public int temperatureMax24Hour { get; set; }
    public int temperatureMaxSince7Am { get; set; }
    public int temperatureMin24Hour { get; set; }
    public int temperatureWindChill { get; set; }
    public string uvDescription { get; set; }
    public int uvIndex { get; set; }
    public DateTimeOffset validTimeLocal { get; set; }
    public int validTimeUtc { get; set; }
    public double visibility { get; set; }
    public int windDirection { get; set; }
    public string windDirectionCardinal { get; set; }
    public int? windGust { get; set; }
    public int windSpeed { get; set; }
    public string wxPhraseLong { get; set; }
    public object wxPhraseMedium { get; set; }
    public object wxPhraseShort { get; set; }
}

//"sunriseTimeLocal": "2025-05-09T05:02:50+0800"
public class FlexibleDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();

        if (str?.Length > 5 && (str[^5] == '+' || str[^5] == '-'))
        {
            str = str.Insert(str.Length - 2, ":");
        }

        if (DateTimeOffset.TryParse(str, null, DateTimeStyles.RoundtripKind, out var dto))
            return dto;

        throw new JsonException($"无法解析 DateTimeOffset：{str}");
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
    }
}

public class FlexibleNullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
{
    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;

        var str = reader.GetString();

        if (string.IsNullOrWhiteSpace(str))
            return null;  // 处理空字符串、空格、Tab等

        if (str?.Length > 5 && (str[^5] == '+' || str[^5] == '-'))
            str = str.Insert(str.Length - 2, ":");

        if (DateTimeOffset.TryParse(str, null, DateTimeStyles.RoundtripKind, out var dto))
            return dto;

        throw new JsonException($"无法解析可空 DateTimeOffset：{str}");
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-ddTHH:mm:sszzz"));
        else
            writer.WriteNullValue();
    }
}
public class FlexibleDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();

        if (string.IsNullOrWhiteSpace(str))
            throw new JsonException("DateTime 字符串为空");

        if (str.Length > 5 && (str[^5] == '+' || str[^5] == '-'))
            str = str.Insert(str.Length - 2, ":");

        if (DateTime.TryParse(str, null, DateTimeStyles.RoundtripKind, out var dt))
            return dt;

        throw new JsonException($"无法解析 DateTime：'{str}'");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:sszzz")); // 保留时区
    }
}

#endregion

#region SearchLocationResponse

public class Locale
{
    public string? locale1 { get; set; }
    public string? locale2 { get; set; }
    public object? locale3 { get; set; }
    public object? locale4 { get; set; }
}

public class SearchedLocation
{
    public string? address { get; set; }
    public Locale? locale { get; set; }
    public string? displayName { get; set; }
    public string? ianaTimeZone { get; set; }
    public string? adminDistrict { get; set; }
    public object? adminDistrictCode { get; set; }
    public string? city { get; set; }
    public double? longitude { get; set; }
    public string? postalCode { get; set; }
    public double? latitude { get; set; }
    public string? placeId { get; set; }
    public object? neighborhood { get; set; }
    public string? country { get; set; }
    public string? postalKey { get; set; }
    public string? countryCode { get; set; }
    public bool disputedArea { get; set; }
    public object? disputedCountries { get; set; }
    public object? disputedCountryCodes { get; set; }
    public object? disputedCustomers { get; set; }
    public List<bool>? disputedShowCountry { get; set; }
    public string? iataCode { get; set; }
    public string? icaoCode { get; set; }
    public string? locId { get; set; }
    public object? locationCategory { get; set; }
    public string? pwsId { get; set; }
    public string? type { get; set; }
}

public class SearchLocationResponse
{
    public List<SearchedLocation>? location { get; set; }
}
#endregion