using System;

namespace Deribit.Core.Messages
{
    public class ExcessiveParameterException : Exception
    {
        public ExcessiveParameterException(string objectName, string parameter) : base(
            $"{parameter} is should not be included on {objectName}")
        {

        }
    }
}
