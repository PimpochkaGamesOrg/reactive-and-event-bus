using System;
using UniRx;

public class ReactiveCounter : IReadOnlyReactiveProperty<int>
{
    private ReactiveProperty<int> _value = new(0);
    private bool _enabled;

    public int Value => _value.Value;
    public bool HasValue => _value.HasValue;
    public bool IsEnabled => _enabled;
    public void Increase()
    {
        _enabled = true;
        _value.Value += 1;
    }
    public void Decrease()
    {
        _enabled = true;
        _value.Value -= 1;
    }
    public void Reset()
    {
        _value.Value = -1;
        _enabled = false;
    }

    public IDisposable Subscribe(IObserver<int> observer)
    {
        return _value.Subscribe(observer);
    }
}