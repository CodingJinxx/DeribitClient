using System;
using System.Collections.Generic;

namespace Deribit.Core
{
    public class Receiver : IObserver<string>
    {
        public Queue<string> Values;
        public bool Done;
        public bool Received;
        public bool ErrorOcurred;
        public string Error;

        public Receiver()
        {
            Values = new Queue<string>();
            Done = false;
            Error = "";
            ErrorOcurred = false;
            Received = false;
        }
        public void OnCompleted()
        {
            Done = true;
        }

        public void OnError(Exception error)
        {
            this.Error = error.Message;
            this.ErrorOcurred = true;
        }

        public void OnNext(string value)
        {
            this.Values.Enqueue(value);
            this.Received = true;
        }
    }
}