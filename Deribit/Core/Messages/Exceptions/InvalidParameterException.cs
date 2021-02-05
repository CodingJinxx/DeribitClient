using System;

namespace Deribit.Core.Messages.Exceptions
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(string objectName, string value, string parameter) : base($"{value} is invalid on {objectName} for {parameter}")
        {

        }
    }
}
