namespace Weather.Core.Models;

public class DataReference
{
    public IEnumerable<string?>? Link { get; set; }

    public IEnumerable<string?>? Sources { get; set; }

    public IEnumerable<string?>? License { get; set; }

    public DataReference Initialize<T>() where T : IEnumerable<string>, new()
    {
        Link = new T();

        Sources = new T();

        License = new T();

        return this;
    }

    public DataReference InitializeDefault() => Initialize<List<string>>();

    public DataReference AppendLink(string link)
    {
        Link = Link?.Append(link);

        return this;
    }

    public DataReference AppendSource(string sources)
    {
        Sources = Sources?.Append(sources);

        return this;
    }

    public DataReference AppendLicense(string license)
    {
        License = License?.Append(license);

        return this;
    }
}
