using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
public static class EventBusMonitor
{
    private static readonly Dictionary<Type, int> busBindingCount = new();
    private static readonly Dictionary<Type, List<string>> busBindingMethods = new();

    private static readonly List<string> eventLogs = new();

    public static IReadOnlyDictionary<Type, int> BusBindingCounts => busBindingCount;
    public static IReadOnlyDictionary<Type, List<string>> BusBindingMethods => busBindingMethods;
    public static IReadOnlyList<string> EventLogs => eventLogs;

    private const int MaxLogCount = 80;

    public static void OnRegister(Type eventType, Delegate handler)
    {
        if (!busBindingCount.ContainsKey(eventType))
            busBindingCount[eventType] = 0;

        busBindingCount[eventType]++;

        if (!busBindingMethods.ContainsKey(eventType))
            busBindingMethods[eventType] = new List<string>();

        string methodInfo = FormatBindingInfo(handler);
        busBindingMethods[eventType].Add(methodInfo);

        AddLog($"Bind   | {eventType.Name}  ->  {methodInfo}");
    }

    public static void OnDeregister(Type eventType, Delegate handler)
    {
        if (!busBindingCount.ContainsKey(eventType))
            return;

        busBindingCount[eventType]--;

        if (busBindingCount[eventType] < 0)
            busBindingCount[eventType] = 0;

        if (busBindingMethods.ContainsKey(eventType))
        {
            string methodInfo = FormatBindingInfo(handler);
            busBindingMethods[eventType].Remove(methodInfo);
        }

        AddLog($"Unbind | {eventType.Name}  ->  {handler.Method.Name}");
    }

    public static void OnRaise(Type eventType, object eventObj)
    {
        string log = $"Raise  | {eventType.Name} -> {eventObj}";
        AddLog(log);
    }

    public static void OnClear(Type eventType)
    {
        if (busBindingCount.ContainsKey(eventType))
            busBindingCount[eventType] = 0;

        if (busBindingMethods.ContainsKey(eventType))
            busBindingMethods[eventType].Clear();

        AddLog($"Clear  | {eventType.Name}");
    }

    private static void AddLog(string content)
    {
        string log = $"[{DateTime.Now:HH:mm:ss}] {content}";
        eventLogs.Add(log);

        if (eventLogs.Count > MaxLogCount)
            eventLogs.RemoveAt(0);
    }

    private static string FormatBindingInfo(Delegate handler)
    {
        string method = handler.Method.Name;
        string className = handler.Target != null ? handler.Target.GetType().Name : "static";
        string parameters = string.Join(", ", handler.Method.GetParameters().Select(p => p.ParameterType.Name));

        string methodSignature = $"{method}({parameters})";

        string goName = "";

        if (handler.Target is MonoBehaviour mono)
        {
            if (mono.gameObject != null)
                goName = $" <color=#888888>[{mono.gameObject.name}]</color>";
        }

        return $"{className}.{methodSignature}{goName}";
    }
}
#endif
