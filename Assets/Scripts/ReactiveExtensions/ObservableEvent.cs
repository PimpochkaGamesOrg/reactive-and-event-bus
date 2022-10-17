using UniRx;
using System;

namespace ReactiveAndEventBus
{
    public class ObservableEvent : IObservable<Unit>
    {
        private Subject<Unit> _subject;

        public void Raise()
        {
            _subject.OnNext(Unit.Default);
        }

        public IDisposable Subscribe(IObserver<Unit> observer)
        {
            return (_subject ?? (_subject = new Subject<Unit>())).Subscribe(observer);
        }
    }
}