using System.IO.Compression;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Weather.Adapter.QWeather.Utils;
using Weather.Core.Models;
using Weather.Core.Models.Units;
using Weather.Core.Standards.Query;
using Weather.Core.Standards.WebApi;

namespace Weather.Adapter.QWeather;

public class WeatherQueryer : IWeatherQueryer
{
    public string GetAdapterIdentity() => ConstantTable.AdapterIdentity;

    public string GetAdapterDiscription() => ConstantTable.AdapterDiscription;

    public async Task<WeatherInfo?> QueryCurrentWeather(string location, IApiConfigProvider apiConfig)
    {
        using var http = new HttpClient();

        apiConfig.InitializeWeatherApi().Path = "/weather/now";

        apiConfig.ApiArguments.InitializeDefaultArguments()
            .AppendArgument("key", apiConfig.Key)
            .AppendArgument("location", location)
            ;

        var url = apiConfig.GetApiUrl();

        var response = await http.GetAsync(url);

        if (response.IsSuccessStatusCode == false)
            throw new HttpRequestException($"The request to {url} failed with status code {response.StatusCode}");

        using var stream = await response.Content.ReadAsStreamAsync();

        using var gzip = new GZipStream(stream, CompressionMode.Decompress);

        using var reader = new StreamReader(gzip, Encoding.UTF8);

        var body = await reader.ReadToEndAsync();

        dynamic json = JObject.Parse(body);

        if ((json.code as string)?.Equals("200") == false)
            return null;

        var result = new WeatherInfo();

        if (json.refer is not null)
        {
            if (json.refer.sources is not null)
                foreach (var item in json.refer.sources)
                    result.Reference.AppendSource(item as string ?? "Convertion failed.");

            if (json.refer.license is not null)
                foreach (var item in json.refer.license)
                    result.Reference.AppendLicense(item as string ?? "Convertion failed.");
        }

        if (json.fxLink as string is not null)
            result.Reference.AppendLink((json.fxLink as string)!);

        result.UpdateTime = JsonSerializer.Deserialize<DateTime>($"\"{json.updateTime}\"");

        var now = json.now;

        result.ObservationTime = JsonSerializer.Deserialize<DateTime>($"\"{now.obsTime}\"");
        result.Text = now.text;
        result.Temperature = new()
        {
            Value = now.temp,
            Unit = TemperatureUnits.Celsius,
        };
        result.FeelsLike = new()
        {
            Value = now.feelsLike,
            Unit = TemperatureUnits.Celsius,
        };
        result.Wind360 = now.wind360;
        result.WindDirection = now.windDir;
        result.WindScale = now.windScale;
        result.WindSpeedKmPerHour = now.windSpeed;
        result.Humidity = now.humidity;
        result.PrecipMillimeter = now.precip;
        result.PressureHpa = now.pressure;
        result.VisibilityKm = now.vis;
        result.Cloud = now.cloud ?? double.NaN;
        result.Dew = now.dew ?? double.NaN;

        return result;
    }

    public async Task<WeatherInfo?> QueryWeatherForecast(string location, IApiConfigProvider apiConfig)
    {
        using var http = new HttpClient();

        apiConfig.InitializeWeatherApi().Path = "/weather/30d";

        apiConfig.ApiArguments.InitializeDefaultArguments()
            .AppendArgument("key", apiConfig.Key)
            .AppendArgument("location", location)
            ;

        var url = apiConfig.GetApiUrl();

        var response = await http.GetAsync(url);

        if (response.IsSuccessStatusCode == false)
            throw new HttpRequestException($"The request to {url} failed with status code {response.StatusCode}");

        using var stream = await response.Content.ReadAsStreamAsync();

        using var gzip = new GZipStream(stream, CompressionMode.Decompress);

        using var reader = new StreamReader(gzip, Encoding.UTF8);

        var body = await reader.ReadToEndAsync();

        dynamic json = JObject.Parse(body);

        if ((json.code as string)?.Equals("200") == false)
            return null;

        var result = new List<WeatherInfo>();

        var refer_sources = new List<string>();

        var refer_license = new List<string>();

        if (json.refer is not null)
        {
            if (json.refer.sources is not null)
                foreach (var item in json.refer.sources)
                    refer_sources.Add(item as string ?? "Convertion failed.");

            if (json.refer.license is not null)
                foreach (var item in json.refer.license)
                    refer_license.Add(item as string ?? "Convertion failed.");
        }

        if (json.fxLink as string is not null)
            result.Reference.AppendLink((json.fxLink as string)!);

        result.UpdateTime = JsonSerializer.Deserialize<DateTime>($"\"{json.updateTime}\"");

        var now = json.now;

        result.ObservationTime = JsonSerializer.Deserialize<DateTime>($"\"{now.obsTime}\"");
        result.Text = now.text;
        result.Temperature = new()
        {
            Value = now.temp,
            Unit = TemperatureUnits.Celsius,
        };
        result.FeelsLike = new()
        {
            Value = now.feelsLike,
            Unit = TemperatureUnits.Celsius,
        };
        result.Wind360 = now.wind360;
        result.WindDirection = now.windDir;
        result.WindScale = now.windScale;
        result.WindSpeedKmPerHour = now.windSpeed;
        result.Humidity = now.humidity;
        result.PrecipMillimeter = now.precip;
        result.PressureHpa = now.pressure;
        result.VisibilityKm = now.vis;
        result.Cloud = now.cloud ?? double.NaN;
        result.Dew = now.dew ?? double.NaN;

        return result;
    }
}
