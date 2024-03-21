using System.ComponentModel.Composition;
using System.Text;

namespace Weather.Core.Standards.WebApi;

[InheritedExport]
public interface IApiConfigProvider : IAdapter
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

    public static readonly DefaultApiConfigProvider Default = new();
}

public class DefaultApiConfigProvider : IApiConfigProvider
{
    public string GetAdapterIdentity() => "Weather.Core.Default";

    public string Key { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;

    public string Version { get; set; } = string.Empty;

    public ApiProtocols Protocol { get; set; } = ApiProtocols.Https;

    public string Path { get; set; } = string.Empty;

    public IApiArgumentsProvider ApiArguments { get; set; } = IApiArgumentsProvider.Default;
}
