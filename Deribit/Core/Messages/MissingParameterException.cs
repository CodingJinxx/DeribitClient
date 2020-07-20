using System;
using System.Collections.Generic;
using System.Text;

namespace Deribit.Core.Messages
{
    public class MissingParameterException : Exception
    {
        public MissingParameterException(string objectName, string parameter) : base($"{parameter} is missing for {objectName}")
        {

        }
    }
}
