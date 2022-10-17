using UniRx;
using System;

namespace ReactiveAndEventBus
{
    public class ObservableEvent : IObservable<Unit>
    {
        private Subject<Unit> _subject;

        private Subject<Unit> Instance => (_subject ?? (_subject = new Subject<Unit>()));
        public void Raise()
        {
            Instance.OnNext(Unit.Default);
        }

        public IDisposable Subscribe(IObserver<Unit> observer)
        {
            return Instance.Subscribe(observer);
        }
    }
}