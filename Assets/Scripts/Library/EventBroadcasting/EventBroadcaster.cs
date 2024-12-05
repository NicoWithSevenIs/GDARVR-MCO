using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventBroadcaster
{
    #region Singleton
        private static EventBroadcaster instance = null;
        public static EventBroadcaster Instance
        {
            get => instance ?? (instance = new EventBroadcaster());
        }
    #endregion

    private EventBroadcaster()
    {
        SceneManager.sceneUnloaded += s => {
            Debug.Log("Cleaning Up Listeners");
            events.Clear(); 
        };
        SceneManager.sceneLoaded += (s, t) => Debug.Log(events.Count);
    }

    private Dictionary<string, Action<Dictionary<string, object>> > events = new();

    public static void AddObserver(string EventName, Action<Dictionary<string, object>> parameters)
    {
        if (!Instance.events.ContainsKey(EventName))
            Instance.events.Add(EventName, parameters);
        else Instance.events[EventName] += parameters;
    }

    public static void RemoveObserver(string EventName, Action<Dictionary<string, object>> parameters)
    {
        if (Instance.events.ContainsKey(EventName))
            Instance.events[EventName] -= parameters;
    }

    public static void Clear()
    {
        Instance.events.Clear();
    }

    public static void InvokeEvent(string EventName, Dictionary<string, object> parameters = null)
    {
        if (Instance.events.ContainsKey(EventName))
            Instance.events[EventName]?.Invoke(parameters);
    }
}
