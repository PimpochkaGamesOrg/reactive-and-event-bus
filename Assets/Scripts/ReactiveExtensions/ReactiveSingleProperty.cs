using System;
using UniRx;

public class ReactiveSingleProperty<T> : IReactiveProperty<T> where T : class
{
    private ReactiveProperty<T> _property;

    public T Value 
    { 
        get => _property.Value; 
        set
        {
            if (value != null && _property.Value != null)
            {
                throw new Exception($"Attempt to replace single property {_property.Value} with {value}");
            }
            _property.Value = value;
        }
    }
    public bool HasValue => _property.HasValue;

    T IReadOnlyReactiveProperty<T>.Value => Value;

    public IDisposable Subscribe(IObserver<T> observer)
    {
        return _property.Subscribe(observer);
    }
    public ReactiveSingleProperty()
    {
        _property = new();
    }
}