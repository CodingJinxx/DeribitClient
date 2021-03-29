using System;
using System.Collections;
using System.Collections.Generic;

namespace Deribit.Core
{
    internal class Unsubscriber<T, C> : IDisposable where C : ICollection
    {
        private readonly C _observers;
        private readonly IObserver<T> _observer;
        private readonly Guid _observerId;
        

        internal Unsubscriber (C observers, IObserver<T> observer, Guid observerId)
        {
            this._observers = observers;
            this._observer = observer;
            this._observerId = observerId;
        }

        public void Dispose()
        {
            if (_observers is Dictionary<Guid, IObserver<T>> dict)
            {
                if (dict.ContainsKey(_observerId))
                {
                    dict.Remove(_observerId);
                }
            }
            else if (_observers is List<IObserver<T>> list)
            {
                if (list.Contains(_observer))
                {
                    list.Remove(_observer);
                }
            }
            else
            {
                throw new Exception("Unsupported Collection Type for Unsubscriber");
            }
        }
    }
}
