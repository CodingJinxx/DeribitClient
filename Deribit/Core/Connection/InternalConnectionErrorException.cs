using System;

namespace Deribit.Core.Connection
{
    public class InternalConnectionErrorException : Exception
    {
        public InternalConnectionErrorException(string reason) : base($"{reason}")
        {

        }
    }
}