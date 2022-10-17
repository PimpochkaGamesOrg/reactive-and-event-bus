using System;
using System.Collections.Generic;
using UniRx;

public static class ReactiveExtensions
{
    public static IDisposable SubscribeAndInit<T>(this IReactiveProperty<T> property, Action<T> onNext)
    {
        onNext(property.Value);
        return property.Subscribe(onNext);
    }
}