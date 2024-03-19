namespace Weather.Core.Standards.WebApi;

public enum ApiProtocols
{
    Http = 1,
    Https = 2,
}

public static class ApiProtocolsExtensions
{
    public static string GetName(this ApiProtocols protocol)
    {
        return protocol switch
        {
            ApiProtocols.Http => "http",
            ApiProtocols.Https => "https",
            _ => throw new NotSupportedException($"The protocol {protocol} is not supported."),
        };
    }
}
