using System;
using System.Reflection;
using Weather.App.Configuration;

namespace Weather.App.Services;

public static class EventService
{
    public static void Invoke(string eventName, object[] objects = null)
    {
        var type = typeof(EventService);

        var eventField = type.GetField(eventName, BindingFlags.Static | BindingFlags.NonPublic);

        if (eventField is null || !typeof(Delegate).IsAssignableFrom(eventField.FieldType))
        {
            throw new ArgumentException($"No event found with the name '{eventName}'.", nameof(eventName));
        }

        var @delegate = eventField.GetValue(null) as Delegate;

        @delegate?.DynamicInvoke(objects);
    }


    public delegate void SelectLocationOverHandler();

    public static event SelectLocationOverHandler SelectLocationOver = new(() => { });


    public delegate void SelectedLocationChangedHandler(string location);

    public static event SelectedLocationChangedHandler SelectedLocationChanged = new(
        location => AppConfig.Instance.SelectedLocation = location
    );
}
