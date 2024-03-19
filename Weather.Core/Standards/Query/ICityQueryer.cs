using System.ComponentModel.Composition;
using Weather.Core.Models;
using Weather.Core.Standards.WebApi;

namespace Weather.Core.Standards.Query;

[InheritedExport]
public interface ICityQueryer
{
    public Task<IEnumerable<CityInfo>> FuzzyQuery(string location, IApiConfigProvider apiConfig);

    public Task<IEnumerable<CityInfo>> QueryTopCities(int count, IApiConfigProvider apiConfig);
}
