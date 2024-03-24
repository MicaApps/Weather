using System.IO.Compression;
using System.Text;
using Newtonsoft.Json.Linq;
using Weather.Adapter.QWeather.Utils;
using Weather.Core.Models;
using Weather.Core.Standards.Query;
using Weather.Core.Standards.WebApi;

namespace Weather.Adapter.QWeather;

public class CityQueryer : ICityQueryer
{
    public string GetAdapterIdentity() => ConstantTable.AdapterIdentity;

    public string GetAdapterDiscription() => ConstantTable.AdapterDiscription;

    public async Task<IEnumerable<CityInfo>?> FuzzyQuery(string location, IApiConfigProvider apiConfig)
    {
        using var http = new HttpClient();

        apiConfig.InitializeGeoApi().Path = "/city/lookup";

        apiConfig.ApiArguments.InitializeDefaultArguments()
            .AppendArgument("key", apiConfig.Key)
            .AppendArgument("location", location)
            ;

        var url = apiConfig.GetApiUrl();

        var response = await http.GetAsync(url);

        if (response.IsSuccessStatusCode == false) return null;

        using var stream = await response.Content.ReadAsStreamAsync();

        using var gzip = new GZipStream(stream, CompressionMode.Decompress);

        using var reader = new StreamReader(gzip, Encoding.UTF8);

        var body = await reader.ReadToEndAsync();

        dynamic json = JObject.Parse(body);

        if ((json.code as string)?.Equals("200") == false || json.location is null) return null;

        var refer_sources = new List<string>();

        var refer_license = new List<string>();

        if (json.refer is not null)
        {
            if (json.refer.sources is not null)
                foreach (var item in json.refer.sources)
                    refer_sources.Add(item as string ?? "Converting failed.");

            if (json.refer.license is not null)
                foreach (var item in json.refer.license)
                    refer_license.Add(item as string ?? "Converting failed.");
        }

        var result = new List<CityInfo>();

        foreach (var jsonCity in json.location)
        {
            var city = new CityInfo()
            {
                Name = jsonCity.name,
                Id = jsonCity.id,
                Country = jsonCity.country,
                OriginData = jsonCity.ToString(),
                Rank = jsonCity.rank,
                Location = new CityLocation()
                {
                    IsDst = (jsonCity.isDst as string)?.Equals("1"),
                    Longitude = jsonCity.lon,
                    Latitude = jsonCity.lat,
                    TimeZone = jsonCity.tz,
                    UtcOffset = jsonCity.utcOffset,
                }
            };

            city.Reference.Sources = refer_sources;
            city.Reference.License = refer_license;
            city.Reference.Link = [jsonCity.fxLink];

            result.Add(city);
        }

        return result;
    }

    public Task<IEnumerable<CityInfo>?> QueryTopCities(int count, IApiConfigProvider apiConfig)
    {
        throw new NotImplementedException();
    }
}
