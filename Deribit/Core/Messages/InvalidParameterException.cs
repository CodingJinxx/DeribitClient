using System;
using System.Collections.Generic;
using System.Text;

namespace Deribit.Core.Messages
{
    public class InvalidParameterException : Exception
    {
        public InvalidParameterException(string objectName, string value, string parameter) : base($"{value} is invalid on {objectName} for {parameter}")
        {

        }
    }
}
