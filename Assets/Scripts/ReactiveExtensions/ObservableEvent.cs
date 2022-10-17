using UniRx;
using System;

namespace ReactiveAndEventBus
{
    public class ObservableEvent : IObservable<Unit>
    {
        private Subject<Unit> _subject;
        public IDisposable Subscribe(IObserver<Unit> observer)
        {
            return (_subject ?? (_subject = new Subject<Unit>())).Subscribe(observer);
        }
    }
}