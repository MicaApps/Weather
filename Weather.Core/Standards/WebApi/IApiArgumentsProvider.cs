using System.ComponentModel.Composition;
using System.Text;

namespace Weather.Core.Standards.WebApi;

[InheritedExport]
public interface IApiArgumentsProvider : IAdapter
{
    public IDictionary<string, string> Arguments { get; set; }

    public bool HasArguments => Arguments.Count > 0;

    public IApiArgumentsProvider InitializeArguments<T>() where T : IDictionary<string, string>, new()
    {
        Arguments = new T();

        return this;
    }

    public IApiArgumentsProvider AppendArgument(string key, string value)
    {
        if (Arguments.ContainsKey(key))
            throw new InvalidOperationException($"The key {key} is already existed.");

        Arguments.Add(key, value);

        return this;
    }

    public string GetQueryString()
    {
        var sb = new StringBuilder();

        foreach (var item in Arguments)
        {
            sb.Append($"{item.Key}={item.Value}");

            if (item.Key.Equals(Arguments.Last().Key) == false)
                sb.Append('&');
        }

        return sb.ToString();
    }

    public IApiArgumentsProvider GetQueryString(out string query)
    {
        query = GetQueryString();

        return this;
    }

    public static readonly DefaultApiArgumentsProvider Default = new();
}

public class DefaultApiArgumentsProvider : IApiArgumentsProvider
{
    public string GetAdapterIdentity() => "Weather.Core.Default";

    public IDictionary<string, string> Arguments { get; set; } = new Dictionary<string, string>();
}
