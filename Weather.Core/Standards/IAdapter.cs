using System.ComponentModel.Composition;

namespace Weather.Core.Standards;

[InheritedExport]
public interface IAdapter
{
    public string GetAdapterIdentity();

    public string GetAdapterDiscription();
}
