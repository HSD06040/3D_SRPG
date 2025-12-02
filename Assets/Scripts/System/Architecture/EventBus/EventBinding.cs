using System;
using System.Collections.Generic;
internal interface IEventBinding<T>
{
    public Action<T> OnEvent { get; set; }
    public Action OnEventNoArgs { get; set; }
}

public class EventBinding<T> : IEventBinding<T> where T : IEvent
{
    Action<T> onEvent = null;
    Action onEventNoArgs = null;

    Action<T> IEventBinding<T>.OnEvent
    {
        get => onEvent;
        set => onEvent = value;
    }

    Action IEventBinding<T>.OnEventNoArgs
    {
        get => onEventNoArgs;
        set => onEventNoArgs = value;
    }

    public EventBinding(Action<T> onEvent) => this.onEvent = onEvent;
    public EventBinding(Action onEventNoArgs) => this.onEventNoArgs = onEventNoArgs;
    public EventBinding() { }

    public void Add(Action onEvent) => onEventNoArgs += onEvent;
    public void Remove(Action onEvent) => onEventNoArgs -= onEvent;

    public void Add(Action<T> onEvent) => this.onEvent += onEvent;
    public void Remove(Action<T> onEvent) => this.onEvent -= onEvent;

    public IEnumerable<Delegate> GetAllDelegates()
    {
        if (onEvent != null)
        {
            foreach (var del in onEvent.GetInvocationList())
                yield return del;
        }

        if (onEventNoArgs != null)
        {
            foreach (var del in onEventNoArgs.GetInvocationList())
                yield return del;
        }
    }
}