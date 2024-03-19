using System.ComponentModel.Composition;
using System.Text;

namespace Weather.Core.Standards.WebApi;

[InheritedExport]
public interface IApiConfigProvider
{
    public string Key { get; set; }

    public string Host { get; set; }

    public string Version { get; set; }

    public ApiProtocols Protocol { get; set; }

    public string Path { get; set; }

    public IApiArgumentsProvider ApiArguments { get; set; }

    public string GetApiUrl() => new StringBuilder()
        .Append($"{Protocol.GetName()}://{Host}/{Version}{Path}")
        .Append(ApiArguments.HasArguments ? "?" + ApiArguments.GetQueryString() : "")
        .ToString();
}
