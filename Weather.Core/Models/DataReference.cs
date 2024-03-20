namespace Weather.Core.Models;

public class DataReference
{
    public IEnumerable<string?>? Link { get; set; }

    public IEnumerable<string?>? Sources { get; set; }

    public IEnumerable<string?>? License { get; set; }
}
