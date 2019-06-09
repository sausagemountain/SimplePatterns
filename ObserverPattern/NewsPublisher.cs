using System;
using System.Collections.Generic;

namespace ObserverPattern
{
    class NewsPublisher: IObservable
    {
        private Dictionary<IObserver, EventHandler<string>> _functions = 
            new Dictionary<IObserver, EventHandler<string>>();

        private event EventHandler<string> Notify;

        public void AddObserver(IObserver o)
        {
            if (!_functions.ContainsKey(o)) {
                _functions[o] = (sender, args) => o.Update();
                Notify += _functions[o];
            }
        }

        public void RemoveObserver(IObserver o)
        {
            Notify -= _functions[o];
            _functions.Remove(o);
        }

        public void NotifyObservers()
        {
            Notify?.Invoke(this, "test");
        }
    }
}