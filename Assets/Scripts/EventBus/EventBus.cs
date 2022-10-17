using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static Dictionary<Type, List<IGlobalSubscriber>> _subscribers = new();

    private static Dictionary<Type, List<Type>> _cachedSubscriberTypes = new();

    public static void Subscribe(IGlobalSubscriber subscriber)
    {
        var subscriberTypes = GetSubscribersTypes(subscriber);
        foreach (Type t in subscriberTypes)
        {
            if (!_subscribers.ContainsKey(t))
            {
                _subscribers[t] = new List<IGlobalSubscriber>();
            }

            _subscribers[t].Add(subscriber);
        }
    }
    public static void Unsubscribe(IGlobalSubscriber subscriber)
    {
        var subscriberTypes = GetSubscribersTypes(subscriber);
        foreach (Type t in subscriberTypes)
        {
            if (_subscribers.ContainsKey(t))
            {
                _subscribers[t].Remove(subscriber);
            }
        }
    }

    public static void RaiseEvent<TSubscriber>(Action<TSubscriber> action)
        where TSubscriber : class, IGlobalSubscriber
    {
        if (!_subscribers.TryGetValue(typeof(TSubscriber), out var subscribers))
        {
            Debug.LogWarning($"No subscribers of type {typeof(TSubscriber)}");
            return;
        }

        foreach (IGlobalSubscriber subscriber in subscribers)
        {
            try
            {
                action.Invoke(subscriber as TSubscriber);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
    private static List<Type> GetSubscribersTypes(IGlobalSubscriber globalSubscriber)
    {
        var type = globalSubscriber.GetType();

        if (_cachedSubscriberTypes.ContainsKey(type))
        {
            return _cachedSubscriberTypes[type];
        }

        var subscriberTypes = type
            .GetInterfaces()
            .Where(it =>
                    typeof(IGlobalSubscriber).IsAssignableFrom(it) &&
                    it != typeof(IGlobalSubscriber))
            .ToList();

        _cachedSubscriberTypes[type] = subscriberTypes;

        return subscriberTypes;
    }
}