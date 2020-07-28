using System;

namespace Deribit.Core.Messages
{
    public class MissingParameterException : Exception
    {
        public MissingParameterException(string objectName, string parameter) : base($"{parameter} is missing for {objectName}")
        {

        }
    }
}
