using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using Weather.Core.Standards;

namespace Weather.App.Services;

public class PluginsService
{

    private static object _instance = new();

    public static PluginsService Instance
    {
        get
        {
            if (_instance is not PluginsService)
                _instance = new PluginsService();

            return _instance as PluginsService;
        }
    }

    private string WorkBase { get; set; } = Path.GetDirectoryName(Environment.ProcessPath);

    private string SearchPattern { get; set; } = "Weather.Adapter.*.dll";

    private CompositionContainer Container { get; set; }

    public PluginsService Initialize()
    {
        var catalog = new DirectoryCatalog(WorkBase, SearchPattern);

        var container = new CompositionContainer(catalog);

        Container = container;

        return this;
    }

    public PluginsService ResetWorkBase(string workBase)
    {
        WorkBase = workBase;

        return this;
    }

    public PluginsService ResetSearchPattern(string pattern)
    {
        SearchPattern = pattern;

        return this;
    }

    public IEnumerable<T> RequestPlugins<T>() where T : IAdapter
    {
        var sub = Container.GetExportedValues<T>();

        return sub;
    }
}
