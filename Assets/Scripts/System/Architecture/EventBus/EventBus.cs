using System.Collections.Generic;

public static class EventBus<T> where T : IEvent
{
    static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

    public static void Register(EventBinding<T> binding)
    {
        bindings.Add(binding);

#if UNITY_EDITOR
        foreach (var del in binding.GetAllDelegates())
        {
            EventBusMonitor.OnRegister(typeof(T), del);
        }
#endif
    }

    public static void Deregister(EventBinding<T> binding)
    {
        bindings.Remove(binding);

#if UNITY_EDITOR
        foreach (var del in binding.GetAllDelegates())
        {
            EventBusMonitor.OnDeregister(typeof(T), del);
        }
#endif
    }

    public static void Raise(T @event)
    {
#if UNITY_EDITOR
        EventBusMonitor.OnRaise(typeof(T), @event);
#endif

        foreach (var binding in bindings)
        {
            binding.OnEvent?.Invoke(@event);
            binding.OnEventNoArgs?.Invoke();
        }
    }

    public static void Raise(EventBinding<T> binding, T @event)
    {
#if UNITY_EDITOR
        EventBusMonitor.OnRaise(typeof(T), @event);
#endif
        IEventBinding<T> eventBinding = binding;

        eventBinding.OnEvent?.Invoke(@event);
        eventBinding.OnEventNoArgs?.Invoke();
    }

    static void Clear()
    {
#if UNITY_EDITOR
        EventBusMonitor.OnClear(typeof(T));
#endif

        bindings.Clear();
    }

}
