using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public static class EventBusExtensions
{
    private class DisposableSubscription : IDisposable
    {
        private bool _disposed;
        private IGlobalSubscriber _subscriber;

        public DisposableSubscription(IGlobalSubscriber subscriber)
        {
            _subscriber = subscriber;
            EventBus.Subscribe(_subscriber);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            EventBus.Unsubscribe(_subscriber);
        }
    }
    public static void SubscribeToEventBus(this CompositeDisposable compositeDisposable, IGlobalSubscriber subscriber)
    {
        compositeDisposable.Add(new DisposableSubscription(subscriber));
    }

    public static void SubscribeToEventBus(this GameObject gameObject, IGlobalSubscriber subscriber)
    {
        var trigger = gameObject.GetComponent<ObservableDestroyTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<ObservableDestroyTrigger>();
        }
        trigger.AddDisposableOnDestroy(new DisposableSubscription(subscriber));
    }

    public static void SubscribeToEventBusOnEnable(this GameObject gameObject, IGlobalSubscriber subscriber)
    {
        if (gameObject.activeSelf)
        {
            EventBus.Subscribe(subscriber);
        }

        var trigger = gameObject.GetComponent<ObservableEnableTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<ObservableEnableTrigger>();
        }


        trigger.OnEnableAsObservable().Subscribe(_ =>
        {
            EventBus.Subscribe(subscriber);
        }).AddTo(gameObject);

        trigger.OnDisableAsObservable().Subscribe(_ =>
        {
            EventBus.Unsubscribe(subscriber);
        }).AddTo(gameObject);
    }
}

