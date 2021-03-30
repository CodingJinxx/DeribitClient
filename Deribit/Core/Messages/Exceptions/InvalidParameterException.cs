using System;

namespace Deribit.Core.Messages.Exceptions
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(string objectName, object value, string parameter) : base($"{value} is invalid on {objectName} for {parameter}")
        {

        }
    }
}
